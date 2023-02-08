using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CreateObjFromTile : MonoBehaviour
{
    public Tilemap tilemap;
    // Start is called before the first frame update
    void Start()
    {
        this.tilemap = this.GetComponent<Tilemap>();
        BoundsInt bounds = tilemap.cellBounds;
        
        var availablePlaces = new List<Vector3>();

        for (int n = bounds.xMin; n < bounds.xMax; n++)
        {
            for (int p = bounds.yMin; p < bounds.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int)tilemap.transform.position.y));
                Vector3 place = tilemap.CellToWorld(localPlace);
                if (tilemap.HasTile(localPlace))
                {
                    //Tile at "place"
                    availablePlaces.Add(place);
                }
                else
                {
                    //No tile at "place"
                }
            }
        }
        Transform parent = GameObject.Find("Map").transform;
        foreach (var item in availablePlaces)
        {
            GameObject gameObject = new GameObject();
            gameObject.transform.position = item;
            gameObject.transform.SetParent(parent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
