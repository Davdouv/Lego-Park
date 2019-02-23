using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VisitorVisiting : Visitor {

    private int range;

    private void Start()
    {
        //agent.radius = 6; // Need to change depending on the model
        //distance = 10.0f;
        range = 100;
        GetRandomGoal();
    }

    private void Update()
    {
        if (HasReachedGoal(10.0f))
        {
            GetRandomGoal();
        }
    }

    private void GetRandomGoal()
    {
        goal = RandomNavmeshLocation(range);
        agent.SetDestination(goal);
    }

    private Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection.y = 0;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        // Finds the closest point on NavMesh within specified range.
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
}
