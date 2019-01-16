﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VisitorManager : MonoBehaviour {

    public GameObject[] visitorType;
    public Vector3 spawnValues;
    public float spawnWait;
    public float spawnMostWait;
    public float spawnLeastWait;
    public int startWait;

    private int randType;
    public int visitors;
    private int currentVisitors;

    // Use this for initialization
    void Start () {
        StartCoroutine(waitSpawner());
	}
	
	// Update is called once per frame
	void Update () {
        spawnWait = Random.Range(spawnLeastWait, spawnMostWait);
	}

    IEnumerator waitSpawner()
    {
        yield return new WaitForSeconds(startWait);

        while (currentVisitors < visitors)
        {
            randType = Random.Range(0, visitorType.Length);

            Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), 1, Random.Range(-spawnValues.z, spawnValues.z));

            GameObject visitor = Instantiate(visitorType[randType], spawnPosition + transform.TransformPoint(0, 0, 0), gameObject.transform.rotation);
            visitor.transform.SetParent(this.transform);
            visitor.GetComponent<Visitor>().CreateAgent();

             ++currentVisitors;

            yield return new WaitForSeconds(spawnWait);
        }
    }
}
