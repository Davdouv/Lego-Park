using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractionShop : Attraction {

    public List<GameObject> decorations;

    protected override IEnumerator EnjoyAttraction()
    {
        foreach(Visitor visitor in visitorInAttraction)
        {
            int random = Random.Range(0, decorations.Count);
            visitor.AddDecoration(decorations[random]);
        }
        yield return new WaitForSeconds(duration);

        Debug.Log("Attraction is over !");
        // Remove visitor from inside the attraction
        StartCoroutine(FreeAttraction());
    }
}
