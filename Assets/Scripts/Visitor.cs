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

    // Distance needed to reach the goal
    protected const float distance = 1.0f;

    // GameObjects the Visitor can take with him
    protected List<GameObject> myDecorations = new List<GameObject>();
    protected GameObject food;

    // To modify the character (hands position for example)
    public VisitorCharacter character;

    // Init method called when the visitor is instanciated by the Factory
    public void CreateAgent()
    {
        Vector3 position = transform.position;

        // Check if the Visitor can be placed on the navmesh before adding a navmeshagent
        NavMeshHit closestHit;
        if (NavMesh.SamplePosition(position, out closestHit, 500, 1))
        {
            transform.position = closestHit.position;
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
        // Go Outside Attraction
        else if (IsExitingAttraction)
        {
            if (HasReachedGoal())
            {
                IsExitingAttraction = false;
                HasReachedAttraction = false;
                GetDestination();
            }
        }
        // In Queue
        else if (IsInQueue)
        {
            FaceAttraction();
        }
        TurnBetter();
    }

    // Turn the object smoothly so it faces the attracion
    private void FaceAttraction()
    {
        Transform target = attractionDest.transform;
        if (this.transform.rotation != target.rotation)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, 2*Time.deltaTime);
        }
    }

    // Modify agent.acceleration so agent can break better if the turn angle is too high and can stop faster
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

    // Called every frame because the queue start can change position at anytime
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

    public bool HasReachedGoal(float dist = distance)
    {
        if ((goal - this.transform.position).sqrMagnitude < dist * dist)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // At this point, the attraction will manage the visitor and tell him what to do
    private void JoinQueue()
    {
        HasReachedAttraction = true;
        IsInQueue = true;
        attractionDest.JoinQueue(this);
    }

    // Move Towards the Exit point of the attraction
    public void ExitAttraction()
    {
        IsExitingAttraction = true;
        MoveForward(attractionDest.GetExit().position);
    }

    // Exit attraction without going through the exit point
    public void ExitAttractionFast()
    {
        HasReachedAttraction = false;
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
