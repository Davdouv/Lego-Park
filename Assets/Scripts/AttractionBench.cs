using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttractionBench : Attraction {

    public struct Seat
    {
        public GameObject seat;
        public bool occupied;
    }
    public GameObject[] seats = new GameObject[2];
    private Visitor[] visitors = new Visitor[2];

    public override bool CanBeJoined()
    {
        return visitors[0] == null || visitors[1] == null;
    }

    public override void JoinQueue(Visitor visitor)
    {
        if (CanBeJoined())
        {
            int num;
            if (visitors[0] == null)
            {
                num = 0;
            }
            else
            {
                num = 1;
            }
            visitors[num] = visitor;
            SitDown(visitor, num);
            StartCoroutine(EnjoyAttraction(num));
        }
        else
        {
            visitor.ExitAttraction();
            visitor.GetDestination();
        }
    }

    private void SitDown(Visitor visitor, int num)
    {
        visitor.GetAgent().enabled = false;
        visitor.transform.position = seats[num].transform.position;
        visitor.transform.rotation = seats[num].transform.rotation;
        visitor.character.Sit();
        if (Random.value > 0.5f) visitor.character.HandsBehind();
    }

    private IEnumerator EnjoyAttraction(int num)
    {
        yield return new WaitForSeconds(duration);

        FreeVisitor(num);
    }

    private void FreeVisitor(int num)
    {
        visitors[num].character.Reset();
        visitors[num].transform.rotation = exit.transform.rotation;
        visitors[num].GetAgent().enabled = true;
        visitors[num].ExitAttraction();

        visitors[num] = null;
    }
}
