﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttractionFerrisWheel : Attraction {
    
    public float speed;
    public GameObject[] seats;
    public GameObject structure;
    public GameObject standingPoint;

    public bool hasAttractionStarted = false;
    private bool isVisitorGettingIn = false;

    private Visitor currentVisitor;
    private float previousAngle;

    bool once = true;

    private void Start()
    {
        previousAngle = structure.transform.localEulerAngles.x;
        Debug.Log(previousAngle);
    }

    protected override void GoInside(Visitor visitor)
    {
        if (visitorInAttraction.Count <= capacity)
        {
            visitor.GetAgent().acceleration = 40; // Set a great deceleration speed so he can turn faster
            visitor.MoveForward(standingPoint.transform.position);
            //visitor.MoveForward(seats[visitorInAttraction.Count].transform.position);
            isVisitorGettingIn = true;
            currentVisitor = visitor;   // So we can access the visitor anywhere

            if (visitorInAttraction.Count > 1)
            {
                StartCoroutine(Rotate(2, 45));
            }
        }
    }

    protected override void GoOutside(Visitor visitor)
    {
        currentVisitor.character.Reset();
        visitor.GetAgent().enabled = true;
        visitor.transform.SetParent(VisitorFactory.Instance.transform);
        visitor.transform.position = standingPoint.transform.position;
        visitor.ExitAttraction();
    }

    protected override bool CanJoinAttraction()
    {
        // Wait for the previous visitor to get inside his seat before joining
        if (isVisitorGettingIn == false && visitorInAttraction.Count < capacity && isAttractionAvailable)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected override bool CanStartAttraction()
    {
        return base.CanStartAttraction() && isVisitorGettingIn == false;
    }

    protected override IEnumerator EnjoyAttraction()
    {
        isAttractionAvailable = false;

        yield return Rotate(duration, 360);
        Debug.Log("Rotation finished");
        StartCoroutine(FreeAttraction());
    }

    private void Update()
    {
        if (isVisitorGettingIn)
        {
            if (currentVisitor.HasReachedGoal())
            {
                Debug.Log("GOAL REACHED");
                Sit();
                isVisitorGettingIn = false;

                // Check if we can start Attraction
                if (CanStartAttraction())
                {
                    StartCoroutine(EnjoyAttraction());
                }
                else
                {
                    JoinAttraction();
                }
            }
        }
        
    }

    private void RotateStructure()
    {
        structure.transform.Rotate(Vector3.left * Time.deltaTime * speed);
        for (int i = 0; i < capacity; ++i)
        {
            seats[i].transform.Rotate(Vector3.right * Time.deltaTime * speed);
        }
    }

    private void Sit()
    {
        currentVisitor.GetAgent().enabled = false;
        currentVisitor.transform.SetParent(seats[visitorInAttraction.Count - 1].transform);

        currentVisitor.transform.localPosition = new Vector3(0, -0.875f, -0.3f);
        currentVisitor.transform.rotation = seats[visitorInAttraction.Count - 1].transform.rotation;

        currentVisitor.character.Sit();
        currentVisitor.character.GrabBar();
    }

    IEnumerator Rotate(float duration, float angle)
    {
        Quaternion startRot = structure.transform.rotation;
        Quaternion startRotSeat = seats[0].transform.rotation;
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            structure.transform.rotation = startRot * Quaternion.AngleAxis(t / duration * angle, Vector3.left);
            for (int i = 0; i < capacity; ++i)
            {
                seats[i].transform.rotation = startRotSeat;
            }
            yield return null;
        }

        /*
        structure.transform.rotation = startRot;
        
        for (int i = 0; i < capacity; ++i)
        {
            seats[i].transform.rotation = startRotSeat;
        }
        */
    }

}
