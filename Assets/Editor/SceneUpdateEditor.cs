using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(SceneUpdate))]
public class SceneUpdateEditor : Editor
{

    private SceneUpdate updater;

    void OnEnable()
    {
        updater = (SceneUpdate)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //if (GUILayout.Button("Update Children"))
        //{
        //    updater.ClearPrefabs();
        //}

        //if (GUILayout.Button("Replace Colliders"))
        //{
        //    //updater.RebuildAllPrefabs();
        //    updater.ReplaceColliders();
        //}

        if (GUILayout.Button("Rebuild Prefabs"))
        {
            updater.RebuildAllPrefabs();
            //updater.ReplaceColliders();
        }

        if (GUILayout.Button("Clear Scene"))
        {
            updater.ClearPrefabs();
        }
    }
}
