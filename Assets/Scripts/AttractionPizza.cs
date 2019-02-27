using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttractionPizza : Attraction
{
    public GameObject food;
    public GameObject standPosition;
    public GameObject waistress;
    
    private bool canRotate = false;
    private float rotationSpeed = 60f;
    private float _rotationTime = 0f;

    protected override void GoInside(Visitor visitor)
    {
        visitor.MoveForward(standPosition.transform.position);
    }

    protected override void GoOutside(Visitor visitor)
    {
        visitor.AddFood(food);
        visitor.ExitAttraction();
    }

    private void Update()
    {
        // ROTATE WAISTRESS
        if (!isAttractionAvailable && !canRotate)
        {
            _rotationTime = 0;
            canRotate = true;
        }

        if (isAttractionAvailable)
        {
            canRotate = false;
        }

        if (canRotate)
        {
            _rotationTime += Time.deltaTime;
            if (_rotationTime < duration / 2)
            {
                waistress.transform.Rotate(0f, -rotationSpeed * Time.deltaTime, 0f);
            }
            else if (_rotationTime < duration)
            {
                waistress.transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
            }
        }
    }

}
