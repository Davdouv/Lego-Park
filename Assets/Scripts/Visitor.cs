using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Visitor : MonoBehaviour
{
    protected NavMeshAgent agent;

    // Destination
    private Attraction attractionDest;
    protected Vector3 goal;
    // State
    bool HasReachedAttraction { get; set; }

    protected float distance = 1.0f;
    protected GameObject myDecoration = null;

    public void CreateAgent()
    {
        Vector3 position = transform.position;
        // Check if the Visitor can be placed on the navmesh before adding a navmeshagent
        NavMeshHit closestHit;
        if (NavMesh.SamplePosition(position, out closestHit, 500, 1))
        {
            transform.position = closestHit.position;
            //gameObject.AddComponent<NavMeshAgent>();
            agent = gameObject.GetComponent<NavMeshAgent>();
        }
    }

    public NavMeshAgent GetAgent()
    {
        return agent;
    }

    public void GetDestination()
    {
        attractionDest = AttractionManager.Instance.GetAttraction();
    }

    private void Update()
    {
        // Go to Attraction
        if (!HasReachedAttraction)
        {
            MoveToAttraction();
            if (HasReachedGoal())
            {
                // Join the queue and enjoy the attraction
                JoinQueue();
            }
            else if (agent.hasPath)
            {
                Vector3 toTarget = agent.steeringTarget - this.transform.position;
                float turnAngle = Vector3.Angle(this.transform.forward, toTarget);
                agent.acceleration = turnAngle * agent.speed;   // Update deceleration
            }
        }
    }

    private void MoveToAttraction()
    {
        if (attractionDest)
        {
            goal = attractionDest.GetQueueStart().position;
            agent.SetDestination(goal);
        }
        else
        {
            GetDestination();
        }
    }

    public bool HasReachedGoal()
    {
        //if (!goal) return false;
        // remainingDistance
        if (Vector3.Distance(goal, this.transform.position) < distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void JoinQueue()
    {
        HasReachedAttraction = true;
        attractionDest.JoinQueue(this);
    }

    public void ExitAttraction()
    {
        HasReachedAttraction = false;
        //this.gameObject.SetActive(true);
        //this.transform.position = attractionDest.GetExit().position;
        GetDestination();
    }

    // Take the position of the previous visitor
    public void MoveForward(Vector3 position)
    {
        goal = position;
        agent.SetDestination(position);
    }

    public Vector3 GetGoal()
    {
        return goal;
    }

    public void AddDecoration(GameObject decoration)
    {
        // If he had already a decoration, destroy it to get a new one
        if (myDecoration)
        {
            Destroy(myDecoration);
        }
        myDecoration = Instantiate(decoration, transform);
        myDecoration.transform.SetParent(this.transform);
    }

    // Check Dist
    // (a-b).sqrMagnitude < Eps*Eps
    // Eps*Eps --> constante
}
