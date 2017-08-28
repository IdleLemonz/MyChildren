using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class SceneUpdate : MonoBehaviour
{    
    List<Transform> allChildren = new List<Transform>();    
    Dictionary<int, PrefabUID> m_currentIDs = new Dictionary<int, PrefabUID>();
    public TileData[] m_tiles;    
    List<GameObject> toDelete = new List<GameObject>();
    public PrefabCache prefabCache;
    GameObject replaceObject;    

    public void Start()
    {
        prefabCache = FindObjectOfType<PrefabCache>();
    }

    // HOW IT WORKS - Instructions need to be updated 
    //
    //  1. Destroy the old object   
    //  2. Loop through the ID's and get the matching ID prefab
    //  3. Store a reference to the objects parent
    //  4. Delete the old object
    //  5. Instantiate an instance of the master prefab
    //  6. Parent the instance to the parent
    //  7. Reparent the children

    public void RebuildAllPrefabs()
    {
        toDelete.Clear();
        m_tiles = FindObjectsOfType<TileData>();
        foreach (TileData tile in m_tiles)
        {
            ReplaceObject(tile.gameObject);
        }

        // Automate applying to prefab
        // PrefabUtility.ReplacePrefab(m_tiles[0].gameObject, PrefabUtility.GetPrefabParent(m_tiles[0].gameObject), ReplacePrefabOptions.ConnectToPrefab);

        foreach (GameObject obj in toDelete)
        {
            DestroyImmediate(obj);
        }
        toDelete.Clear();
    }   

    public void ReplaceObject(GameObject obj)
    {     
        // pass in the game object to replace    
        int numChildren = 0;
        List<GameObject> needsReparenting = new List<GameObject>(); // list of children to be reparented later in SwapObjects
        if (obj.transform.childCount > 0)
        {
            numChildren = obj.transform.childCount;
            // for each child of the object
            for (int i = 0; i < numChildren; i++)
            {
                // Recurse to the next child down
                ReplaceObject(obj.transform.GetChild(i).gameObject);
                // Add children to reparent them later, after returning from recursive function    
                if(replaceObject != null)            
                    needsReparenting.Add(replaceObject);                           
            }
            // Get the prefab that matches this objects ID
            if (obj.GetComponent<PrefabUID>() != null)
                replaceObject = SwapPrefabs(obj, needsReparenting);
            else
                replaceObject = obj;            
        }
        else
        {
            // If the object has an ID, replace it and later add it to the list of children to be replaced
            if (obj.GetComponent<PrefabUID>() != null)
            {
                replaceObject = SwapPrefabs(obj, needsReparenting);
            }                
            else
            {
                // If the object has no ID, has no children, it's parent has no ID and it's parent isn't the Tile, swap replaceObject so it can be reparented
                if (obj.transform.childCount == 0 && obj.transform.parent.GetComponent<PrefabUID>() == null && obj.transform.parent.GetComponent<TileData>() == null)
                    replaceObject = obj;
                else // replaceObject is set to null if the object should be ignored and not set to be reparented
                    replaceObject = null; 
            }                
        }        
    }

    public GameObject SwapPrefabs(GameObject obj, List<GameObject> toReparent = null)
    {
        PrefabUID objectID = obj.GetComponent<PrefabUID>(); // Get this object's ID
        GameObject masterPrefab = FindMatchingID(objectID); // prefab to update the current Game Object with   
        if (masterPrefab != null)
        {
            string oldName = masterPrefab.name;                 
            GameObject oldParent = obj.transform.parent.gameObject; // store the old parent and transform data
            Vector3 oldScale = obj.transform.localScale;
            Vector3 oldPosition = obj.transform.position;
            Vector3 oldRotation = obj.transform.eulerAngles;
            GameObject copyPrefab;
       
            // Instantiate a copy of the master prefab, apply the transform data and resolve parenting & children
            copyPrefab = Instantiate(masterPrefab);
            copyPrefab.transform.position = oldPosition;
            copyPrefab.transform.localScale = oldScale;
            copyPrefab.transform.eulerAngles = oldRotation;
            copyPrefab.transform.parent = oldParent.transform;
            copyPrefab.name = oldName;
           
            if(toReparent.Count > 0)
            {
                foreach (GameObject child in toReparent)
                {
                    child.transform.parent = copyPrefab.transform;
                }
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

    public void ClearPrefabs()
    {        
    }   
}
