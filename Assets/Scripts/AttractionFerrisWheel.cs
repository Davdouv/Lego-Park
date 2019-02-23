using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttractionFerrisWheel : Attraction {

    [System.Serializable]
    public struct Seat
    {
        public GameObject seat;
        public bool occupied;
    }

    public float speed;
    public Seat[] seats;
    public GameObject structure;
    public GameObject standingPoint;

    public bool hasAttractionStarted = false;
    private bool isVisitorGettingIn = false;

    private Visitor currentVisitor;

    int currentSeat = 0;


    protected override void GoInside(Visitor visitor)
    {
        if (visitorInAttraction.Count <= capacity)
        {
            visitor.GetAgent().acceleration = 40; // Set a great deceleration speed so he can turn faster
            visitor.MoveForward(standingPoint.transform.position);
            isVisitorGettingIn = true;
            currentVisitor = visitor;   // So we can access the visitor anywhere

            if (visitorInAttraction.Count > 1 && isAttractionAvailable)
            {
                StartCoroutine(Rotate(2, 45));
            }
        }
    }

    protected override void GoOutside(Visitor visitor)
    {
        visitor.character.Reset();
        visitor.transform.SetParent(VisitorFactory.Instance.transform);
        visitor.transform.position = standingPoint.transform.position;
        visitor.GetAgent().enabled = true;
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
        yield return new WaitForSeconds(1);
        isAttractionAvailable = false;

        yield return Rotate(duration, 360);
        StartCoroutine(FreeAttraction());
    }

    protected override IEnumerator FreeAttraction()
    {
        yield return new WaitForSeconds(1);
        //int inAttraction = visitorInAttraction.Count;
        int occupiedSeats = currentSeat;
        for (int i = 0; i < capacity; ++i)
        {
            yield return Rotate(2, 45);
            // if 1 occupied seat then we start at index 0 + 1
            int num = occupiedSeats + i;
            // if 8 occupied seats then we start at index 0
            if (num > 7)
            {
                num = num - 8;
            }

            currentSeat = num;  // Store the currentSeat num so if a visitor wants to join he know which seat to take
            if (seats[num].occupied)
            {
                Visitor visitor = visitorInAttraction.Dequeue();
                GoOutside(visitor);
                seats[num].occupied = false;
            }
            if (!seats[num].occupied)
            {
                // Before the next visitor can go out, make a new visitor get inside
                yield return JoinAttractionWait();
                // Wait a moment so the visitors don't go out at the same time
                Debug.Log("ROTATE");
                yield return new WaitForSeconds(0.25f);
            }
        }

        yield return new WaitForSeconds(1);

        isAttractionAvailable = true;

        // After the attraction ended, if there are still visitors in the queue, make them join the attraction
        if (CanStartAttraction())
        {
            StartCoroutine(EnjoyAttraction());
        }
        else if (CanJoinAttraction())
        {
            JoinAttraction();
        }
    }

    IEnumerator JoinAttractionWait()
    {
        if (visitorQueue.Count > 0)
        {
            JoinAttraction();
        }
        while (isVisitorGettingIn)
        {
            yield return null;
        }
        Debug.Log("REACHED");
        yield return null;
    }

    private void Update()
    {
        if (isVisitorGettingIn)
        {
            if (currentVisitor.HasReachedGoal())
            {
                Sit();
                isVisitorGettingIn = false;
                seats[currentSeat].occupied = true;
                IncrementSeat();

                // Check if we can start Attraction
                if (CanStartAttraction())
                {
                    StartCoroutine(EnjoyAttraction());
                }
                else if (CanJoinAttraction())
                {
                    JoinAttraction();
                }
            }
        }        
    }

    /*
    private void RotateStructure()
    {
        structure.transform.Rotate(Vector3.left * Time.deltaTime * speed);
        for (int i = 0; i < capacity; ++i)
        {
            seats[i].seat.transform.Rotate(Vector3.right * Time.deltaTime * speed);
        }
    }
    */

    private void Sit()
    {
        currentVisitor.GetAgent().enabled = false;
        currentVisitor.transform.SetParent(seats[currentSeat].seat.transform);

        currentVisitor.transform.localPosition = new Vector3(0, -0.875f, -0.3f);
        currentVisitor.transform.rotation = seats[currentSeat].seat.transform.rotation;

        currentVisitor.character.Sit();
        currentVisitor.character.GrabBar();
    }

    IEnumerator Rotate(float duration, float angle)
    {
        Quaternion startRot = structure.transform.rotation;
        Quaternion startRotSeat = seats[0].seat.transform.rotation;
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            structure.transform.rotation = startRot * Quaternion.AngleAxis(t / duration * angle, Vector3.left);
            for (int i = 0; i < capacity; ++i)
            {
                seats[i].seat.transform.rotation = startRotSeat;
            }
            yield return null;
        }
    }

    private void IncrementSeat()
    {
        ++currentSeat;
        if (currentSeat == capacity)
        {
            currentSeat = 0;
        }
    }

}
