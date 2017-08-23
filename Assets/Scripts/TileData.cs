using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TileData : MonoBehaviour
{

    public enum TileType
    {
        Joined,
        Unjoined,
        DeadEnd
    };

    public enum EnvironmentType
    {
        Empty,
        Standard,
        ForestCave,
        IceCave
    };

    public enum Rotate
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    };

    //public enum MapOrTile
    //{
    //    Map,
    //    Tile        
    //};

    public TileType tileType = TileType.Joined;
    public EnvironmentType environmentType = EnvironmentType.Empty;
    public Rotate rotate = Rotate.LEFT;
    //public MapOrTile mapOrTile = MapOrTile.Tile;

    // Update is called once per frame
    void Update()
    {
        //if (rotate == Rotate.UP)
        //{
        //    Vector3 newRotation = new Vector3(0, 0, 0);
        //    gameObject.transform.eulerAngles = newRotation;
        //}
        //else if (rotate == Rotate.DOWN)
        //{
        //    Vector3 newRotation = new Vector3(0, 180, 0);
        //    gameObject.transform.eulerAngles = newRotation;
        //}
        //else if (rotate == Rotate.LEFT)
        //{
        //    Vector3 newRotation = new Vector3(0, -90, 0);
        //    gameObject.transform.eulerAngles = newRotation;
        //}
        //else if (rotate == Rotate.RIGHT)
        //{
        //    Vector3 newRotation = new Vector3(0, 90, 0);
        //    gameObject.transform.eulerAngles = newRotation;
        //}
    }
}
