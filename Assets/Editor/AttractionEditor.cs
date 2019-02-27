using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Attraction), true, isFallback = true)]
public class AttractionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Attraction attraction = (Attraction)target;

        if (attraction.queueStep < 5)
        {
            EditorGUILayout.HelpBox("Queue step must be >= 5", MessageType.Error);
        }
    }
}
