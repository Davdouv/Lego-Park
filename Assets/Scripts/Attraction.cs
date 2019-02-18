using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attraction : MonoBehaviour
{
    // Number of visitors that can go inside the attraction at the same time
    public uint capacity;
    // Max number of visitor the queue can contains
    public uint queueCapacity;
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
    public virtual void JoinQueue(Visitor visitor)
    {
        if (CanBeJoined())
        {
            visitorQueue.Enqueue(visitor);
            // Move the begining of the Queue backward
            MoveQueueBackward();
            if (CanJoinAttraction())
            {
                JoinAttraction();
            }
        }
        else
        {
            visitor.GetDestination();
        }
    }

    protected virtual bool CanJoinAttraction()
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

    // Called when the queue can get in the attraction or when the attraction ended
    protected void JoinAttraction()
    {
        // Visitor is leaving queue
        Visitor visitor = visitorQueue.Dequeue();
        visitor.IsInQueue = false;
        // Move the queue forward
        MoveQueueForward(visitor);
        // Join Attraction
        visitorInAttraction.Enqueue(visitor);
        // Make the visitor go inside the attraction
        GoInside(visitor);

        // Start Attraction asap
        if (CanStartAttraction())
        {
            StartCoroutine(EnjoyAttraction());
        }
        // More people can come in the attraction
        else if (IsQueueFilled() && CanJoinAttraction())
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
        visitor.ExitAttraction();
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
        isAttractionAvailable = false;
        yield return new WaitForSeconds(duration);
        
        // Remove visitor from inside the attraction
        StartCoroutine(FreeAttraction());
    }

    protected IEnumerator FreeAttraction()
    {
        int inAttraction = visitorInAttraction.Count;
        for (int i = 0; i < inAttraction; ++i)
        {
            if (visitorInAttraction.Count > 0)
            {
                Visitor visitor = visitorInAttraction.Dequeue();
                GoOutside(visitor);
                // Wait a moment so the visitors don't go out at the same time
                yield return new WaitForSeconds(0.25f);
            }            
        }
        ClearAttraction();
        isAttractionAvailable = true;

        yield return new WaitForSeconds(1);
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

    // When a visitor is entering the attraction
    void MoveQueueForward(Visitor previousVisitor = null)
    {
        // Move queue start location
        Vector3 translation = new Vector3(0, 0, queueStep);
        queueStart.transform.Translate(translation);

        // Move each visitor forward
        Vector3 oldPosition = entry.transform.position;
        Transform newGoal = entry.transform;
        foreach (Visitor visitor in visitorQueue)
        {
            visitor.MoveForward(newGoal.position);
            newGoal.Translate(-translation);
        }
        entry.transform.position = oldPosition;
    }

    // When a visitor joins the queue
    void MoveQueueBackward()
    {
        // Move queue start location
        Vector3 translation = new Vector3(0, 0, -queueStep);
        queueStart.transform.Translate(translation);
    }

    public virtual bool CanBeJoined()
    {
        return !(visitorQueue.Count == queueCapacity);
    }
}
