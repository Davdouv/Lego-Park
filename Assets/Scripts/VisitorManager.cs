using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VisitorManager : MonoBehaviour {

    public GameObject[] visitorAttraction;    // Models of visitor that can do attractions
    public GameObject[] visitorVisiting; // Models of visitor that move randomly
    public Vector3 spawnValues;
    private float spawnWait;
    public float spawnMostWait;
    public float spawnLeastWait;
    public int startWait;

    private int randType;
    public uint numberOfVisitorsAttraction;
    public uint numberOfVisitorsVisiting;
    private uint currentVisitorsAttraction = 0;
    private uint currentVisitorsVisiting = 0;

    // Use this for initialization
    void Start () {
        spawnWait = startWait;
        StartCoroutine(waitSpawner(visitorAttraction, currentVisitorsAttraction, numberOfVisitorsAttraction));
        StartCoroutine(waitSpawner(visitorVisiting, currentVisitorsVisiting, numberOfVisitorsVisiting));
    }
	
	// Update is called once per frame
	void Update () {
        spawnWait = Random.Range(spawnLeastWait, spawnMostWait);
	}

    IEnumerator waitSpawner(GameObject[] visitors, uint currentVisitors, uint numberOfVisitor)
    {
        yield return new WaitForSeconds(startWait);

        while (currentVisitors < numberOfVisitor)
        {
            randType = Random.Range(0, visitors.Length);

            Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), 1, Random.Range(-spawnValues.z, spawnValues.z));

            GameObject visitor = Instantiate(visitors[randType], spawnPosition + transform.TransformPoint(0, 0, 0), gameObject.transform.rotation);
            visitor.transform.SetParent(this.transform);
            visitor.GetComponent<Visitor>().CreateAgent();

            ++currentVisitors;

            yield return new WaitForSeconds(spawnWait);
        }
    }
}
