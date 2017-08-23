using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class SceneUpdate : MonoBehaviour {

    public Transform[] everyChild;
    public List<Transform> allChildren = new List<Transform>();
    public GameObject testObject;    
    Dictionary<int, PrefabUID> m_currentIDs = new Dictionary<int, PrefabUID>();
    TileData[] m_tiles;
    Collider[] prefabColliders;
    PrefabCache prefabCache;    
    List<GameObject> toDelete = new List<GameObject>();

    public void Start()
    {
        prefabCache = FindObjectOfType<PrefabCache>();
    }    

    // destroy the old object   
    // loop through the ID's and get the matching ID prefab
    // store a reference to the objects parent
    // delete the old object
    // instantiate an instance of the master prefab
    // parent the instance to the parent
    // reparent the children

    public void ReplaceObject(GameObject obj)
    {
        // pass in the game object to replace    
        int numChildren = 0;
        List<GameObject> needsReparenting = new List<GameObject>();
        if (obj.transform.childCount > 0)
        {
            numChildren = obj.transform.childCount;            
            for (int i = 0; i < numChildren; i++)
            {                
                // Recurse to the next child down
                ReplaceObject(obj.transform.GetChild(i).gameObject);
                // Add children to reparent them later, after returning from recursive function                
                needsReparenting.Add(obj.transform.GetChild(i).gameObject);

                // Get the prefab that matches this objects ID
                if (obj.GetComponent<PrefabUID>() != null)
                {
                    SwapPrefabs(obj, needsReparenting);
                    return;   
                }                
            }           
        }
        else
        {
            SwapPrefabs(obj, needsReparenting);           
        }       

        // check if the game object has any children
        // if it has children, call this function again on the child
        // if it doesn't, replace the objects
    }

    public void SwapPrefabs(GameObject obj, List<GameObject> toReparent)
    {
        PrefabUID childID = obj.GetComponent<PrefabUID>(); // Get this object's ID
        GameObject masterPrefab = FindMatchingID(childID); // prefab to update the current Game Object with                    
        GameObject oldParent = obj.transform.parent.gameObject; // store the old parent and transform data
        Vector3 oldScale = obj.transform.localScale;
        Vector3 oldPosition = obj.transform.position;
        Vector3 oldRotation = obj.transform.eulerAngles;
        GameObject copyPrefab;

        if (masterPrefab != null)
        {
            // Instantiate a copy of the master prefab, apply the transform data and resolve parenting & children
            copyPrefab = Instantiate(masterPrefab);
            copyPrefab.transform.position = oldPosition;
            copyPrefab.transform.localScale = oldScale;
            copyPrefab.transform.eulerAngles = oldRotation;
            copyPrefab.transform.parent = oldParent.transform;

            foreach (GameObject child in toReparent)
            {
                child.transform.parent = copyPrefab.transform;
            }           
        }
        toDelete.Add(obj);        
    }

    public GameObject FindMatchingID(PrefabUID child)
    {
        foreach (PrefabUID ID in prefabCache.prefabsToUpdate)
        {
            if (ID.m_IDName == child.m_IDName)
            {
                return ID.gameObject;
            }
        }
        return null;
    }

    public void RebuildAllPrefabs()
    {        
        m_tiles = FindObjectsOfType<TileData>(); // load the tiles from the resources filder   
        ReplaceObject(m_tiles[0].gameObject);

        foreach(GameObject obj in toDelete)
        {
            DestroyImmediate(obj);
        }       
        
    }   

    public void ClearPrefabs()
    {        
    }   
}
