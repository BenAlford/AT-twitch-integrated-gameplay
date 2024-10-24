using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] Tilemap ground_map;
    [SerializeField] Tile ground;
    [SerializeField] GameObject saw;
    [SerializeField] Tile saw_tile;
    [SerializeField] GameObject bomb;
    Dictionary<Vector2Int,GameObject> objects = new Dictionary<Vector2Int, GameObject>();
    
    public string GetTile(int x, int y)
    {
        TileBase a = tilemap.GetTile(new Vector3Int(x, y, 0));
        if (a != null)
        {
            return a.name;
        }
        return "None";
    }

    public void PlaceTile(string name, string pos)
    {
        int x = pos[0] - 97;
        if (x >= 0 && x < 26)
        {
            string num = pos.Substring(1);
            print(num);
            int total = 0;
            for (int i = 0; i < num.Length - 1; i++)
            {
                total *= 10;
                total += num[i] - 48;
                print(total);
            }
            int y = total;

            if (name == "bomb")
            {
                Instantiate(bomb).transform.position = new Vector3(x, y, 0);
            }
            else if (name == "bigbomb")
            {
                GameObject newbomb = Instantiate(bomb);
                newbomb.transform.position = new Vector3(x, y, 0);
                newbomb.GetComponent<BombBehaviour>().radius = 2;
            }

            if (CanPlace(x, y))
            {
                if (name == "ground")
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), ground);
                    ground_map.SetTile(new Vector3Int(x, y, 0), ground);
                }
                if (name == "saw")
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), saw_tile);
                    GameObject newObj = Instantiate(saw);
                    newObj.transform.position = new Vector3(x, y, 0);
                    objects.Add(new Vector2Int(x,y),newObj);
                }
            }
        }
    }

    public void RemoveTiles(int x, int y, int r)
    {
        r = Mathf.Abs(r);
        for (int i = x-r; i <= x+r; i++)
        {
            for (int j = y-r; j <= y+r; j++)
            {
                //print(new Vector2(i,j));
                TileBase tile = tilemap.GetTile(new Vector3Int(i, j, 0));
                if (tile != null)
                {
                    if (tile.name == "ground")
                    {
                        ground_map.SetTile(new Vector3Int(i, j, 0), null);
                    }
                    else if (tile.name == "saw")
                    {
                        GameObject saw;
                        objects.TryGetValue(new Vector2Int(i, j), out saw);
                        if (saw != null) Destroy(saw);
                        objects.Remove(new Vector2Int(i, j));
                    }
                    tilemap.SetTile(new Vector3Int(i, j, 0), null);
                }
            }
        }
    }

    bool CanPlace(int x, int y)
    {
        if (tilemap.GetTile(new Vector3Int(x, y, 0)) == null)
        {
            return true;
        }
        return false;
    }
}
