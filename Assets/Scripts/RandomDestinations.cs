using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Store some random destination points calculated at the start
public class RandomDestinations : MonoBehaviour
{
    #region Singleton
    // Instance
    private static RandomDestinations m_instance;

    public static RandomDestinations Instance
    {
        get
        {
            // create logic to create the instance
            if (m_instance == null)
            {
                GameObject go = new GameObject("_RandomDestinations");
                go.AddComponent<AttractionManager>();
            }
            return m_instance;
        }
    }

    void Awake()
    {
        m_instance = this;
    }
    #endregion

    // Number of possible random destinations
    const int destinations = 500;
    private Vector3[] randomDestination = new Vector3[destinations];

    // So we know the size of the radius
    public Terrain terrain;

    // Start is called before the first frame update
    void Start()
    {
        float range = terrain.terrainData.size.x / 2;
        for (int i = 0; i < destinations; ++i)
        {
            randomDestination[i] = RandomNavmeshLocation(range);
        }
    }

    // To position the object at the middle of the terrain because we use a radius from this object for random position
    private void PositionAtMiddleOfTerrain()
    {
        transform.position = new Vector3(terrain.terrainData.size.x / 2, 0, terrain.terrainData.size.z / 2);
    }

    // Find a random position on navMesh
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

    public Vector3 GetRandomDestination()
    {
        return randomDestination[Random.Range(0, destinations)];
    }
}
