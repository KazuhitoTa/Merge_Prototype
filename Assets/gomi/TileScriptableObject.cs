using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Collections.Generic; 

public class TileScriptableObject : MonoBehaviour
{
    [SerializeField]Tilemap tilemap;
    [SerializeField]GameObject test;
    [SerializeField]GameObject testNull;
    [SerializeField]TilePos tilePos;
    [SerializeField]int xMin;
    [SerializeField]int xMax;
    [SerializeField]int yMin;
    [SerializeField]int yMax;

    

    private void Start()
    {
        GetTilemapObjectPositions();
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0))GetClickObj();
    }

    private void GetTilemapObjectPositions()
    {
        BoundsInt bounds = tilemap.cellBounds;
        xMax=bounds.xMax;
        xMin=bounds.xMin;
        yMax=bounds.yMax;
        yMin=bounds.yMin;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                TileBase tile = tilemap.GetTile(tilePosition);
                Vector3 worldPosition = tilemap.CellToWorld(tilePosition);
                //Debug.Log("Tile Object Position: " + worldPosition);
                tilePos.temp.Add(worldPosition);
                if (tile != null)
                {
                    tilePos.tmp.Add(0);
                    Instantiate(test,worldPosition,Quaternion.identity);
                    
                }
                else
                {
                    tilePos.tmp.Add(999);
                    Instantiate(testNull,worldPosition,Quaternion.identity);
                }
                 
                
            }
        }
        Debug.Log(tilePos.tmp.Count);
    }

    GameObject GetClickObj()
    {
        GameObject clickedObject = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
    
        if (hit.collider != null) 
        {
            clickedObject = hit.collider.gameObject;
        }
        Debug.Log(clickedObject);
        return clickedObject;
        
    }

    [System.Serializable]
    public class TilePos
    {
        public List<Vector3> temp=new List<Vector3>();
        public List<int> tmp=new List<int>();

    }

   
}


