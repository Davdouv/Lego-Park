using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttractionBench : Attraction {

    public GameObject[] seats;
    private List<Visitor> visitors;

    public override bool CanBeJoined()
    {
        return !(visitors.Count == capacity);
    }

    public override void JoinQueue(Visitor visitor)
    {
        visitors.Add(visitor);
        SitDown(visitor);
    }

    private void SitDown(Visitor visitor)
    {
        visitor.GetAgent().enabled = false;
        visitor.transform.position = seats[visitorInAttraction.Count - 1].transform.position;
        visitor.transform.rotation = seats[visitorInAttraction.Count - 1].transform.rotation;
        visitor.GetComponent<VisitorCharacter>().Sit();
        if (Random.value > 0.5f) visitor.GetComponent<VisitorCharacter>().HandsBehind();
    }
}
