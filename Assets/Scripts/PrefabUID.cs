using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PrefabUID : MonoBehaviour
{    
    List<PrefabUID> toUpdate = new List<PrefabUID>();

    PrefabUID[] m_existingIDs;
    TileData[] m_tiles;
        
    public string m_IDName = "Default";
    [HideInInspector]
    public bool m_locked = true;    

    public void UpdateIDList()
    {
        m_existingIDs = Resources.LoadAll<PrefabUID>("");
    }    

    public void ClearPrefabs()
    {
        GameObject[] objectsToClear = FindObjectsOfType<GameObject>();        

        for (int i = 0; i < objectsToClear.Length; i++)
            DestroyImmediate(objectsToClear[i]);

        toUpdate.Clear();
    }
}
