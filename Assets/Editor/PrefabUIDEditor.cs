using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(PrefabUID))]
public class PrefabUIDEditor : Editor
{
    private PrefabUID UID;
    public SerializedProperty stringID;

    void OnEnable()
    {
        UID = (PrefabUID)target;

        stringID = serializedObject.FindProperty("IDName");
    }

    public override void OnInspectorGUI()
    {
        // set default ID to -1
        // check if ID is -1, if it is, get all ID's that are set, then give it a new highest value

        base.OnInspectorGUI();

        //if(UID.m_locked)
        //{
        //    EditorGUILayout.BeginHorizontal();            
        //    EditorGUILayout.LabelField("String ID: ", EditorStyles.boldLabel);
        //    EditorGUILayout.LabelField(UID.m_IDName);
        //    EditorGUILayout.EndHorizontal();

        //    if (GUILayout.Button("UNLOCK ID"))
        //    {
        //        UID.m_locked = false;
        //    }
        //}
        //else
        //{
        //    EditorGUILayout.BeginHorizontal();
        //    serializedObject.Update();
        //    EditorGUILayout.LabelField("String ID: ", EditorStyles.boldLabel);
        //    stringID.stringValue = EditorGUILayout.TextArea(stringID.stringValue);
        //    serializedObject.ApplyModifiedProperties();
        //    EditorGUILayout.EndHorizontal();

        //    if (GUILayout.Button("LOCK ID"))
        //    {
        //        UID.m_locked = true;
        //    }
        //}

        //if (GUILayout.Button("Print ID"))
        //{
        //    Debug.Log(UID.IDName);
        //}

        //EditorGUILayout.Space();

        //UID.CheckForExistingID();

        //if (GUILayout.Button("Reset All ID's"))
        //{
        //    UID.ResetIDNumbers();
        //    UID.IDNumber = 0;            
        //}

        //if (GUILayout.Button("Rebuild All Prefabs"))
        //{
        //    UID.RebuildAllPrefabs();            
        //}

        //if (GUILayout.Button("Clear Scene"))
        //{
        //    UID.ClearPrefabs();
        //}

        //UID.m_IDstring = "" + UID.m_IDNumber;
        //EditorGUILayout.BeginHorizontal();
        //EditorGUILayout.LabelField("Old Numbered UID: ", EditorStyles.boldLabel);
        //EditorGUILayout.LabelField(UID.m_IDstring);
        //this.Repaint();

        //EditorGUILayout.EndHorizontal();

    }

}
