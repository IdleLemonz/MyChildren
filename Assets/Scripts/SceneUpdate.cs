using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class SceneUpdate : MonoBehaviour {

    public Transform[] everyChild;
    public List<Transform> allChildren = new List<Transform>();
    public GameObject testObject;
    PrefabUID[] m_existingIDArray;
    Dictionary<int, PrefabUID> m_currentIDs = new Dictionary<int, PrefabUID>();
    TileData[] m_tiles;
    Collider[] prefabColliders;
    public void ReplaceColliders()
    {
        UpdateIDList(); // Get all master prefabs with ID's from the resources folder
        m_tiles = GameObject.FindObjectsOfType<TileData>(); // load the tiles from the resources filder       

        GetChildren(m_tiles[0].gameObject);

        // Get cache of all prefabs and get a list of all the ones that have the PrefabUID script
        PrefabCache prefabCache = FindObjectOfType<PrefabCache>();
        //List<PrefabUID> idList = new List<PrefabUID>();
        //foreach (GameObject prefab in prefabCache.prefabsToUpdate)
        //{
        //    if (prefab.GetComponent<PrefabUID>() != null)
        //        idList.Add(prefab.GetComponent<PrefabUID>());
        //}

        // Go through each child, see if it has an ID, and then find a match to the ID list of master prefabs
       
        foreach (Transform child in allChildren)
        {
            
            PrefabUID childID = child.GetComponent<PrefabUID>();
            for (int i = 0; i < prefabCache.prefabsToUpdate.Count; i++)
            {
                if(childID.m_IDstring == prefabCache.prefabsToUpdate[i].m_IDstring)
                {
                    // When you find a match, get all colliders attached to the prefab and make a copy
                    prefabColliders = prefabCache.prefabsToUpdate[i].gameObject.GetComponents<Collider>();
                }
            }            
                
            // get all colliders attached to the child, destroy them, then replace them with the prefabs colliders
            Collider [] colliders = child.GetComponents<Collider>();
            foreach (Collider col in colliders)
            {
                DestroyImmediate(col);
            }

            foreach(Collider col in prefabColliders)
            {
                if(col.GetType() == typeof(BoxCollider))
                {
                    BoxCollider box = (BoxCollider)col;
                    BoxCollider newBox = child.gameObject.AddComponent<BoxCollider>();
                    newBox = box;
                }
                
            }
        }
    }

    public void GetChildren(GameObject obj)
    {
        allChildren.Clear();
        // loop through and get every child at the first layer    
        everyChild = obj.GetComponentsInChildren<Transform>();

        foreach (Transform child in everyChild)
        {
            if (child != obj.transform)
                allChildren.Add(child);
        }
    }

    public void ReplaceObject(GameObject obj)
    {
        // pass in the game object to replace
        int numChildren = 0;
        if(obj.transform.childCount > 0)
        {
            numChildren = obj.transform.childCount;
            for (int i = 0; i < numChildren; i++)
            {
                //ReplaceObject(obj.transform.GetChild(i).gameObject);

                GameObject test = Instantiate(obj);
            }
        }
        else
        {
            obj.transform.parent.gameObject.SetActive(false);
        }       

        // check if the game object has any children

        // if it has children, call this function again on the child

        // if it doesn't, replace the objects
    }

    public void RebuildAllPrefabs()
    {        
        UpdateIDList(); // Get all master prefabs with ID's from the resources folder
        m_tiles = FindObjectsOfType<TileData>(); // load the tiles from the resources filder       

        //GetChildren(m_tiles[0].gameObject);

        //everyChild = m_tiles[0].GetComponentsInChildren<Transform>();

        ReplaceObject(m_tiles[0].gameObject);

        // loop through each tile, it's children, and then compare the child's ID to the master prefab ID list
        //foreach(TileData tile  in m_tiles)
        //{
        //    GameObject tempObject = Instantiate(tile.gameObject); 
        //    PrefabUID[] allChildren = tempObject.GetComponentsInChildren<PrefabUID>();
        //    foreach (PrefabUID child in allChildren)
        //    {
        //        foreach (PrefabUID originalPrefab in m_existingIDArray)
        //        {
        //            if (child.m_IDNumber == originalPrefab.m_IDNumber)
        //            {
        //                // save the transfrom data, destroy the old child, then make the new one
        //                Vector3 oldPosition = child.transform.position;
        //                Vector3 oldScale = child.transform.localScale;
        //                Vector3 oldRotation = child.transform.eulerAngles;
        //                DestroyImmediate(child.gameObject);

        //                GameObject childObject = Instantiate(originalPrefab.gameObject);
        //                childObject.transform.position = oldPosition;
        //                childObject.transform.localScale = oldScale;
        //                childObject.transform.eulerAngles = oldRotation;
        //                childObject.transform.parent = tile.transform;

        //                break;
        //            }
        //        }
        //    }

            //PrefabUtility.SetPropertyModifications(tile.gameObject, PropertyModification)
            //PrefabUtility.ReplacePrefab(tile.gameObject, PrefabUtility.GetPrefabParent(tile), ReplacePrefabOptions.ConnectToPrefab);
        //}
        
    }

    public void UpdateIDList()
    {
        m_existingIDArray = Resources.LoadAll<PrefabUID>("");

        foreach (PrefabUID ID in m_existingIDArray)
            if (!m_currentIDs.ContainsKey(ID.m_IDNumber))
                m_currentIDs.Add(ID.m_IDNumber, ID);
    }

    public void ClearPrefabs()
    {
        //GameObject[] objectsToClear = FindObjectsOfType<GameObject>();

        //for (int i = 0; i < objectsToClear.Length; i++)
        //    DestroyImmediate(objectsToClear[i]);

        //toUpdate.Clear();
    }
}
