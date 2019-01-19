using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Go : MonoBehaviour {

    public GameObject Dest;

	// Use this for initialization
	void Start () {
        GetComponent<NavMeshAgent>().SetDestination(Dest.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
