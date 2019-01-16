// AttractionManager.cs
// This has to be used as a Singleton to get access to all the attractions
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractionManager : MonoBehaviour
{
    // Instance
    private static AttractionManager m_instance;
    // Attractions
    public Attraction[] attractions;
    private int numberOfAttractions;

    public static AttractionManager Instance
    {
        get
        {
            // create logic to create the instance
            if (m_instance == null)
            {
                GameObject go = new GameObject("AttractionManager");
                go.AddComponent<AttractionManager>();
            }
            return m_instance;
        }
    }

    void Awake()
    {
        m_instance = this;
    }

    private void Start()
    {
        numberOfAttractions = attractions.Length;
    }

    public Attraction GetAttraction(int num)
    {
        return attractions[num];
    }

    public Attraction GetAttraction()
    {
        return attractions[Random.Range(0, numberOfAttractions)];
    }
}
