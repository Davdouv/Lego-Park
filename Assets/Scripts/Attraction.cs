﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attraction : MonoBehaviour
{
    // Number of visitors that can go inside the attraction at the same time
    public uint capacity;
    // Time of the attraction in seconds
    public float duration;
    // List of visitors in the queue
    protected Queue<Visitor> visitorQueue = new Queue<Visitor>();
    // List of visitors inside the attraction
    protected Queue<Visitor> visitorInAttraction = new Queue<Visitor>();
    // Entry and exit point
    public GameObject entry;
    public GameObject exit;
    // Position of the start of the queue
    protected GameObject queueStart;
    // Distance between each visitor in the queue
    public float queueStep = 5f;
    // Check if people are in attraction or not
    protected bool isAttractionAvailable;

    private void Awake()
    {
        queueStart = new GameObject("QueueStart");
        queueStart.transform.SetParent(this.transform);
        queueStart.transform.position = entry.transform.position;
        queueStart.transform.rotation = entry.transform.rotation;

        isAttractionAvailable = true;
    }

    public Transform GetQueueStart()
    {
        return queueStart.transform;
    }

    public Transform GetEntry()
    {
        return entry.transform;
    }

    public Transform GetExit()
    {
        return exit.transform;
    }

    // Called when a visitor reaches the queue
    public void JoinQueue(Visitor visitor)
    {
        visitorQueue.Enqueue(visitor);
        // Move the begining of the Queue backward
        MoveQueueBackward();
        if (CanJoinAttraction())
        {
            JoinAttraction();
        }
    }

    private bool CanJoinAttraction()
    {
        // If the attraction has not started yet & there is enough capacity, join the attraction
        if (visitorInAttraction.Count < capacity && isAttractionAvailable)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void LeaveQueue()
    {
    }

    // Called when the queue can get in the attraction or when the attraction ended
    protected void JoinAttraction()
    {
        // Visitor is leaving queue
        Visitor visitor = visitorQueue.Dequeue();
        // Move the queue forward
        //StartCoroutine(MoveQueueForward(visitor));
        MoveQueueForward(visitor);
        // Join Attraction
        visitorInAttraction.Enqueue(visitor);
        // Make the visitor go inside the attraction
        GoInside(visitor);

        // Start Attraction asap
        if (CanStartAttraction())
        {
            isAttractionAvailable = false;
            StartCoroutine(EnjoyAttraction());
        }
        // More people can come in the attraction
        else if (IsQueueFilled() && isAttractionAvailable)
        {
            JoinAttraction();
        }
    }

    protected virtual void GoInside(Visitor visitor)
    {
        visitor.gameObject.SetActive(false);
    }

    protected virtual void GoOutside(Visitor visitor)
    {        
        visitor.transform.position = GetExit().position;
        visitor.gameObject.SetActive(true);
    }

    protected virtual bool CanStartAttraction()
    {
        if (isAttractionAvailable && (visitorInAttraction.Count == capacity || (visitorQueue.Count == 0 && visitorInAttraction.Count > 0)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected virtual IEnumerator EnjoyAttraction()
    {
        yield return new WaitForSeconds(duration);

        Debug.Log("Attraction is over !");
        // Remove visitor from inside the attraction
        StartCoroutine(FreeAttraction());
    }

    protected IEnumerator FreeAttraction()
    {
        int inAttraction = visitorInAttraction.Count;
        for (int i = 0; i < inAttraction; ++i)
        {
            Visitor visitor = visitorInAttraction.Dequeue();
            GoOutside(visitor);
            visitor.ExitAttraction();
            // Wait a moment so the visitors don't go out at the same time
            yield return new WaitForSeconds(1);
            Debug.Log("Visitor is free");
        }
        ClearAttraction();
        Debug.Log("Attraction is Available !");
        isAttractionAvailable = true;

        // After the attraction ended, if there are still visitors in the queue, make them join the attraction
        if (IsQueueFilled())
        {
            JoinAttraction();
        }
    }

    private bool IsQueueFilled()
    {
        return (visitorQueue.Count > 0);
    }

    protected virtual void ClearAttraction()
    { }

    void MoveQueueForward(Visitor previousVisitor = null)
    {
        // Move visitors towards the attraction
        Vector3 newPosition = previousVisitor.transform.position;
        foreach (Visitor visitor in visitorQueue)
        {
            visitor.MoveForward(newPosition);
            newPosition = visitor.transform.position;
        }

        // Wait for the visitor to reach their new position before moving the queue starting point
        //yield return new WaitForSeconds(queueStep/5);

        // Move queue start location
        Vector3 translation = new Vector3(0, 0, queueStep);
        queueStart.transform.Translate(translation);
    }

    void MoveQueueBackward()
    {
        // Move queue start location
        Vector3 translation = new Vector3(0, 0, -queueStep);
        queueStart.transform.Translate(translation);
    }
}
