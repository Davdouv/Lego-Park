using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// This visitor is just going through random places on the navmesh
public class VisitorVisiting : Visitor {

    private int range;

    private void Start()
    {
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
        //goal = RandomNavmeshLocation(range);
        goal = RandomDestinations.Instance.GetRandomDestination();
        agent.SetDestination(goal);
    }

    // Not used anymore --> Moved into RandomDestinations to store a lot of destinations instead of calculating for every visitor visiting
    /*
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
    */
}
