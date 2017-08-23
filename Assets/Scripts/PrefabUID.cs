using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PrefabUID : MonoBehaviour {

    [HideInInspector]
    public string m_IDstring = "";   
    [HideInInspector]
    public int m_IDNumber = -1;

    List<PrefabUID> toUpdate = new List<PrefabUID>();

    PrefabUID[] m_existingIDs;
    TileData[] m_tiles;

    [HideInInspector]
    public string IDName = "Default";
    [HideInInspector]
    public bool locked = true;

    public void ResetIDNumbers()
    {
        UpdateIDList();

        foreach (PrefabUID id in m_existingIDs)
        {
            id.m_IDNumber = -1;
        }
    }

    //public void CheckForExistingID()
    //{
    //    if(m_IDNumber == -1)
    //    {
    //        UpdateIDList();

    //        PrefabUID highestID;
    //        highestID = m_existingIDs[0];

    //        foreach (PrefabUID id in m_existingIDs)
    //        {                
    //            if (id.m_IDNumber > highestID.m_IDNumber)
    //            {
    //                highestID = id;
    //            }
    //        }

    //        m_IDNumber = highestID.m_IDNumber + 1;
    //    }       
    //}

    public void UpdateIDList()
    {
        m_existingIDs = Resources.LoadAll<PrefabUID>("");
    }

    public void RebuildAllPrefabs()
    {
        // go through and check all of the children of each tile for their ID's, and match it to the list of prefabs
        UpdateIDList();
        m_tiles = Resources.LoadAll<TileData>("");  

        foreach(TileData tile in m_tiles)
        {
            Transform[] allChildren = GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren)
            {
                if(child.GetComponent<PrefabUID>() != null)
                {
                    PrefabUID childID = child.GetComponent<PrefabUID>();
                    foreach (PrefabUID originalPrefab in m_existingIDs)
                    {
                        if(childID.m_IDNumber == originalPrefab.m_IDNumber)
                        {
                            Transform oldTransform = originalPrefab.gameObject.transform;

                            DestroyImmediate(child.gameObject);
                            GameObject childObject = Instantiate(originalPrefab.gameObject, oldTransform);
                            childObject.transform.parent = tile.transform;

                            break;
                        }
                    }                    
                }
            }
        }


    }

    public void ClearPrefabs()
    {
        GameObject[] objectsToClear = FindObjectsOfType<GameObject>();        

        for (int i = 0; i < objectsToClear.Length; i++)
            DestroyImmediate(objectsToClear[i]);

        toUpdate.Clear();
    }
}
