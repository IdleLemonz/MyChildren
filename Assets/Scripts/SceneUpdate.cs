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
    GameObject replaceObject;
    public List<GameObject> toDelete = new List<GameObject>();

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
                needsReparenting.Add(replaceObject);                           
            }
            // Get the prefab that matches this objects ID
            if (obj.GetComponent<PrefabUID>() != null)
            {
                replaceObject = SwapPrefabs(obj, needsReparenting);
            }
        }
        else
        {
            replaceObject = SwapPrefabs(obj, needsReparenting);            
        }        
    }

    public GameObject SwapPrefabs(GameObject obj, List<GameObject> toReparent = null)
    {
        PrefabUID childID = obj.GetComponent<PrefabUID>(); // Get this object's ID
        GameObject masterPrefab = FindMatchingID(childID); // prefab to update the current Game Object with   
        string oldName = masterPrefab.name;                 
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
            copyPrefab.name = oldName;
           
            foreach (GameObject child in toReparent)
            {
                child.transform.parent = copyPrefab.transform;
            }               
                     
            toDelete.Add(obj); // add the old object to the list of objects to be deleted
            return copyPrefab;     
        }        
        return null;
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
        toDelete.Clear();
        m_tiles = FindObjectsOfType<TileData>();
        ReplaceObject(m_tiles[0].gameObject);  

        foreach (GameObject obj in toDelete)
        {
            DestroyImmediate(obj);
        }       
        
    }   

    public void ClearPrefabs()
    {        
    }   
}
