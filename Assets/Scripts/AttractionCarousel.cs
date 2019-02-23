using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttractionCarousel : Attraction {

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

    protected override void GoInside(Visitor visitor)
    {
        if (visitorInAttraction.Count <= capacity)
        {
            visitor.GetAgent().acceleration = 40; // Set a great deceleration speed so he can turn faster
            visitor.MoveForward(seats[visitorInAttraction.Count-1].seat.transform.position);
        }
    }

    protected override void GoOutside(Visitor visitor)
    {
        visitor.character.Reset();
        visitor.transform.localPosition = new Vector3(0,0,-0.004f);
        visitor.transform.SetParent(VisitorFactory.Instance.transform);        
        visitor.GetAgent().enabled = true;
        //visitor.ExitAttraction();
        visitor.ExitAttractionFast();
    }
    
    protected override bool CanStartAttraction()
    {
        // Check if all visitor in attraction has reached their seats
        bool canStart = true;
        int i = 0;
        foreach (Visitor visitor in visitorInAttraction)
        {
            if (!seats[i].occupied)
            {
                if (!visitor.HasReachedGoal())
                {
                    canStart = false;
                }
                else
                {
                    Sit(visitor, i);
                }
            }
            ++i;
        }

        return base.CanStartAttraction() && canStart;
    }

    protected override IEnumerator EnjoyAttraction()
    {
        isAttractionAvailable = false;
        yield return new WaitForSeconds(1);        

        yield return Rotate(duration, 2*360);
        StartCoroutine(FreeAttraction());
    }

    protected override IEnumerator FreeAttraction()
    {
        yield return new WaitForSeconds(1);

        int count = visitorInAttraction.Count;
        for (int i = 0; i < count; ++i)
        {
            Visitor visitor = visitorInAttraction.Dequeue();
            GoOutside(visitor);
            seats[i].occupied = false;
        }

        yield return new WaitForSeconds(1);

        isAttractionAvailable = true;

        if (CanJoinAttraction())
        {
            JoinAttraction();
        }
    }

    private void Update()
    {
        if (isAttractionAvailable && visitorInAttraction.Count > 0)
        {
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

    private void Sit(Visitor visitor, int i)
    {
        seats[i].occupied = true;

        visitor.GetAgent().enabled = false;
        visitor.transform.SetParent(seats[i].seat.transform);
        
        visitor.transform.localPosition = new Vector3(0, 0.0036f, 0);
        visitor.transform.rotation = seats[i].seat.transform.rotation;

        visitor.character.Sit();
        visitor.character.HoldBar();
    }

    IEnumerator Rotate(float duration, float angle)
    {
        Quaternion startRot = structure.transform.rotation;
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            structure.transform.rotation = startRot * Quaternion.AngleAxis(t / duration * angle, Vector3.up);
            yield return null;
        }
    }

}
