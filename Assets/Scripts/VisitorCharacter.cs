using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A List of method to make the character take various positions
// To add a position, try to do it in the inspector then copy / paste the values of the objects
public class VisitorCharacter : MonoBehaviour {

    public GameObject leftArm;
    public GameObject rightArm;
    public GameObject leftHand;
    public GameObject rightHand;

    public GameObject leftLeg;
    public GameObject rightLeg;

    private Vector3 _leftArmRotation;
    private Vector3 _leftArmPosition;
    private Vector3 _rightArmRotation;
    private Vector3 _rightArmPosition;
    private Vector3 _leftHandRotation;
    private Vector3 _leftHandPosition;
    private Vector3 _rightHandRotation;
    private Vector3 _rightHandosition;

    private Vector3 _leftLegRotation;
    private Vector3 _leftLegPosition;
    private Vector3 _rightLegRotation;
    private Vector3 _rightLegPosition;

    private bool _isHandlingFood = false;
    private float _consommationTime = 15f;
    private float _currentConsommationTime = 0f;

    private GameObject _food;

    // Use this for initialization
    void Start () {
        _leftArmRotation = leftArm.transform.localEulerAngles;
        _rightArmRotation = rightArm.transform.localEulerAngles;
        _leftHandRotation = leftHand.transform.localEulerAngles;
        _rightHandRotation = rightHand.transform.localEulerAngles;
        _leftLegRotation = leftLeg.transform.localEulerAngles;
        _rightLegRotation = rightLeg.transform.localEulerAngles;

        _leftArmPosition = leftArm.transform.localPosition;
        _rightArmPosition = rightArm.transform.localPosition;
        _leftHandPosition = leftHand.transform.localPosition;
        _rightHandosition = rightHand.transform.localPosition;
        _leftLegPosition = leftLeg.transform.localPosition;
        _rightLegPosition = rightLeg.transform.localPosition;
    }

    public void HandleFood()
    {
        _isHandlingFood = true;

        leftArm.transform.localPosition = new Vector3(12.77f, 24.32f, 22.07f);
        rightArm.transform.localPosition = new Vector3(-12.28f, 12.3f, 37.4f);
        leftHand.transform.localPosition = new Vector3(12.769f, 24.320f, 22.070f);
        rightHand.transform.localPosition = new Vector3(-20.71f, 12.29f, 38.57f);

        leftArm.transform.localEulerAngles = new Vector3(-54.4f, 0f, 19.8f);
        rightArm.transform.localEulerAngles = new Vector3(-54.4f, 0f, -16.09f);
        leftHand.transform.localEulerAngles = new Vector3(-54.4f, 0, 19.8f);
        rightHand.transform.localEulerAngles = new Vector3(-54.4f, 0f, -30.04f);

        _food.transform.SetParent(rightHand.transform);
    }

    public void Sit()
    {
        leftLeg.transform.localPosition = new Vector3(0f, 10f, 21f);
        rightLeg.transform.localPosition = new Vector3(0f, 10f, 21f);

        leftLeg.transform.localEulerAngles = new Vector3(-80f, 0, 0);
        rightLeg.transform.localEulerAngles = new Vector3(-80f, 0, 0);
    }

    public void HandsBehind()
    {
        leftArm.transform.localPosition = new Vector3(0f, 10.6f, -24f);
        rightArm.transform.localPosition = new Vector3(0f, 10.6f, -24f);
        leftHand.transform.localPosition = new Vector3(0f, 10.6f, -24f);
        rightHand.transform.localPosition = new Vector3(0f, 10.6f, -24f);

        leftArm.transform.localEulerAngles = new Vector3(33.6f, 0f, 0f);
        rightArm.transform.localEulerAngles = new Vector3(33.6f, 0f, 0f);
        leftHand.transform.localEulerAngles = new Vector3(33.6f, 0f, 0f);
        rightHand.transform.localEulerAngles = new Vector3(33.6f, 0f, 0f);
    }

    public void GrabBar()
    {
        leftArm.transform.localPosition = new Vector3(4.8f, 55.6f, 41.9f);
        rightArm.transform.localPosition = new Vector3(-1.93f, 55.22f, 41.63f);
        leftHand.transform.localPosition = new Vector3(-40.2f, 15.2f, 57.4f);
        rightHand.transform.localPosition = new Vector3(-18.87f, 54.65f, 41.93f);

        leftArm.transform.localEulerAngles = new Vector3(-118.6f, 0f, 0f);
        rightArm.transform.localEulerAngles = new Vector3(-118.6f, 0f, 0f);
        leftHand.transform.localEulerAngles = new Vector3(0f, 91.15f, -116.7f);
        rightHand.transform.localEulerAngles = new Vector3(0f, 91.15f, -139f);
    }

    public void HoldBar()
    {
        leftArm.transform.localPosition = new Vector3(0, 15.6f, 37.3f);
        rightArm.transform.localPosition = new Vector3(0, 15.6f, 37.3f);
        leftHand.transform.localPosition = new Vector3(-34.5f, -12.4f, -9.1f);
        rightHand.transform.localPosition = new Vector3(-16.5f, 9.63f, 31.8f);

        leftArm.transform.localEulerAngles = new Vector3(-56.2f, 0f, 0f);
        rightArm.transform.localEulerAngles = new Vector3(-56.2f, 0f, 0f);
        leftHand.transform.localEulerAngles = new Vector3(0f, 61f, -28.94f);
        rightHand.transform.localEulerAngles = new Vector3(0f, 92.9f, -63.9f);
    }

    public void Reset()
    {
        leftArm.transform.localEulerAngles = _leftArmRotation;
        rightArm.transform.localEulerAngles = _rightArmRotation;
        leftHand.transform.localEulerAngles = _leftHandRotation;
        rightHand.transform.localEulerAngles = _rightHandRotation;
        leftLeg.transform.localEulerAngles = _leftLegRotation;
        rightLeg.transform.localEulerAngles = _rightLegRotation;

        leftArm.transform.localPosition = _leftArmPosition;
        rightArm.transform.localPosition = _rightArmPosition;
        leftHand.transform.localPosition = _leftHandPosition;
        rightHand.transform.localPosition = _rightHandosition;
        leftLeg.transform.localPosition = _leftLegPosition;
        rightLeg.transform.localPosition = _rightLegPosition;
    }

    public void GiveFood(GameObject newFood)
    {
        _food = newFood;
    }

    private void EatFood()
    {
        if (_food)
        {
            Destroy(_food);
        }
        _isHandlingFood = false;
        Reset();
    }

    // Update is called once per frame
    void Update () {
		if (_isHandlingFood)
        {
            _currentConsommationTime += Time.deltaTime;
            if (_currentConsommationTime > _consommationTime)
            {
                EatFood();
                _currentConsommationTime = 0;
            }
        }
	}
}
