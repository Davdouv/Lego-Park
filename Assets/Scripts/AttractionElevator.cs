using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttractionElevator : Attraction {

    public GameObject wall;
    public GameObject platform;
    public GameObject[] seats;

    public float speed;

    private List<Visitor> visitors;  // We need a list to access the element by index
    private int count = 0;
    private bool hasAttractionStarted = false;

    private float maxHeight;
    private float minHeight;

    private bool movingUp = false;
    private bool pause = false;

    private void Start()
    {
        visitors = new List<Visitor>();
        maxHeight = platform.transform.position.y + wall.transform.lossyScale.y - platform.transform.lossyScale.y;
        minHeight = platform.transform.position.y;
    }

    protected override void GoInside(Visitor visitor)
    {
        if (count < seats.Length)
        {
            visitor.GetAgent().acceleration = 40; // Set a great deceleration speed so he can turn faster
            visitors.Add(visitor);
            visitor.MoveForward(seats[count].transform.position);
            ++count;
        }        
    }
    
    protected override IEnumerator EnjoyAttraction()
    {
        isAttractionAvailable = false; // attraction starts
        hasAttractionStarted = true;
        movingUp = true;
        // We need to disable navMeshAgent in order to lift up the object
        visitors.ForEach(visitor => visitor.GetAgent().enabled = false);
        
        yield return new WaitForSeconds(0);
    }

    protected override void GoOutside(Visitor visitor)
    {
        visitor.ExitAttraction();
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
        count = 0;
        visitors.Clear();
    }

    private void Update()
    {
        if (visitors.Count > 0 && CanStartAttraction())
        {
            StartCoroutine(EnjoyAttraction());
        }
        else if (hasAttractionStarted && !pause)
        {
            //platform.GetComponent<NavMeshModifierVolume>().
            if (movingUp)
            {
                Vector3 moveUp = Vector3.up * Time.deltaTime * speed / 4;
                platform.transform.Translate(moveUp);
                visitors.ForEach(visitor => visitor.transform.Translate(moveUp));
                if (platform.transform.position.y >= maxHeight)
                {
                    pause = true;
                    movingUp = false;
                    StartCoroutine(Wait(1));
                }
            }
            else
            {
                Vector3 moveDown = Vector3.down * Time.deltaTime * speed;
                platform.transform.Translate(moveDown);
                visitors.ForEach(visitor => visitor.transform.Translate(moveDown));
                if (platform.transform.position.y <= minHeight)
                {
                    movingUp = false;
                    hasAttractionStarted = false;
                    visitors.ForEach(visitor => visitor.GetAgent().enabled = true);
                    StartCoroutine(FreeAttraction());
                }
            }
        }
    }

    private IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        pause = false;
    }
}
