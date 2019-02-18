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
    public bool HasReachedAttraction { get; set; }
    public bool IsInQueue { get; set; }
    public bool IsExitingAttraction { get; set; }

    protected float distance = 1.0f;
    protected List<GameObject> myDecorations = new List<GameObject>();
    protected GameObject food;

    public VisitorCharacter character;

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
        }
        else if (IsExitingAttraction)
        {
            if (HasReachedGoal())
            {
                IsExitingAttraction = false;
                HasReachedAttraction = false;
                GetDestination();
            }
        }
        else if (IsInQueue)
        {
            FaceAttraction();
        }
        TurnBetter();
    }

    private void FaceAttraction()
    {
        //Transform target = attractionDest.GetEntry();
        Transform target = attractionDest.transform;
        if (this.transform.rotation != target.rotation)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, 2*Time.deltaTime);
        }
    }

    private void TurnBetter()
    {
        if (agent.hasPath && !HasReachedGoal())
        {
            float previousAcceleration = agent.acceleration;
            Vector3 toTarget = agent.steeringTarget - this.transform.position;
            float turnAngle = Vector3.Angle(this.transform.forward, toTarget);
            agent.acceleration = turnAngle * agent.speed;   // Update deceleration
            if (agent.acceleration <= 10)
            {
                agent.acceleration = previousAcceleration;
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
        if ((goal - this.transform.position).sqrMagnitude < distance*distance)
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
        IsInQueue = true;
        attractionDest.JoinQueue(this);
    }

    public void ExitAttraction()
    {
        IsExitingAttraction = true;
        MoveForward(attractionDest.GetExit().position);
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
        // If we don't have this type of decoration, add it
        if (myDecorations.TrueForAll(myDecoration => myDecoration.tag != decoration.tag))
        {
            myDecorations.Add(Instantiate(decoration, transform));
            myDecorations[myDecorations.Count-1].transform.SetParent(this.transform);
        }
    }

    public void AddFood(GameObject newFood)
    {
        if (food)
        {
            Destroy(food);
        }
        food = Instantiate(newFood, transform);
        food.transform.SetParent(this.transform);
        character.GiveFood(food);
        character.HandleFood();
    }
}
