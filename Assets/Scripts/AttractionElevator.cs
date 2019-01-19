using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractionElevator : Attraction {

    public GameObject[] seats;
    private List<Visitor> visitors;  // We need a list to access the element by index
    private int count = 0;

    private void Start()
    {
        visitors = new List<Visitor>();
    }

    protected override void GoInside(Visitor visitor)
    {
        visitor.GetAgent().acceleration = 40; // Set a great deceleration speed so he can turn faster
        visitors.Add(visitor);
        visitor.MoveForward(seats[count].transform.position);
        ++count;
    }

    /*
    protected override IEnumerator EnjoyAttraction()
    {
        isAttractionAvailable = false; // attraction already started at this point
        foreach (Visitor visitor in visitorInAttraction)
        {
            
        }
        yield return new WaitForSeconds(duration);

        Debug.Log("Attraction is over !");
        // Remove visitor from inside the attraction
        StartCoroutine(FreeAttraction());
    }
    */

    protected override void GoOutside(Visitor visitor)
    {
        visitor.GetAgent().acceleration = 15;
        visitor.transform.position = GetExit().position;
        visitor.gameObject.SetActive(true);
        --count;
    }

    private void Update()
    {
        if (visitors.Count > 0 && CanStartAttraction())
        {
            Debug.Log("YEEAAH ELEVATOR");
            StartCoroutine(EnjoyAttraction());
        }
        else if (visitors.Count > 0)
        {
            visitors.ForEach(visitor => Debug.Log(Vector3.Distance(visitor.transform.position, visitor.GetGoal())));
        }
    }

    protected override bool CanStartAttraction()
    {
        if (isAttractionAvailable && visitors.TrueForAll(visitor => visitor.HasReachedGoal()))
        {
            return true;
        }
        else
        {
            return false;
        }        
    }

    protected override void ClearAttraction()
    {
        visitors.Clear();
    }
}
