using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    [Header("References")]
    public BoardSettings BS;

    [Header("Prefabs")]
    public Tile tilePrefab;

    [Header("Tiles and Walls")]
    public List<Tile> tiles = new List<Tile>();

    public List<Wall> walls = new List<Wall>();
    public List<Wall> outsideWalls = new List<Wall>();
    public List<Wall> horizontalWalls = new List<Wall>();
    public List<Wall> verticalWalls = new List<Wall>();

    // Start is called before the first frame update
    void Start()
    {
        if( BS != null)
            LoadBoardTiles();
    }

    public void LoadBoardTiles()
    {
        //spawn tiles
        for(int i = 0; i < BS.mapWidth * BS.mapHeight; i++)
        {
                Tile newTile = Instantiate(tilePrefab, this.transform);
                newTile.myBoard = this;
                newTile.TileIndex = i;

                tiles.Add(newTile);
        }

        PlaceTiles();
    }

    public void PlaceTiles()
    {
        int index = 0;
        float originXTilePos = - BS.mapWidth / 2;
        float originYTilePos = BS.mapHeight / 2;

        Vector2 tilePos = new Vector2(originXTilePos * tilePrefab.GetWidth(), originYTilePos * tilePrefab.GetWidth());

        //lignes
        for (int i = 0; i < BS.mapHeight; i++)
        {
            //colonnes
            for(int j = 0; j < BS.mapWidth; j++)
            {
                Tile currentTile = tiles[index];

                currentTile.gameObject.transform.position = tilePos + new Vector2(currentTile.GetWidth() / 2, -currentTile.GetHeight() / 2);

                //set lights and outside walls
                currentTile.PlaceLightTopLeft();

                if(i == 0)
                {
                    Wall newWall = currentTile.PlaceWalls(DIRECTION.TOP);
                    walls.Add(newWall);
                    outsideWalls.Add(newWall);
                }
                
                if(j == 0)
                {
                    currentTile.PlaceLightTopRight();

                    Wall newWall = currentTile.PlaceWalls(DIRECTION.LEFT);
                    walls.Add(newWall);
                    outsideWalls.Add(newWall);
                }

                if(i == BS.mapHeight - 1)
                {
                    currentTile.PlaceLightBottomLeft();

                    Wall newWall = currentTile.PlaceWalls(DIRECTION.BOTTOM);
                    walls.Add(newWall);
                    outsideWalls.Add(newWall);
                }

                if(j == BS.mapWidth - 1)
                {
                    currentTile.PlaceLightBottomRight();

                    Wall newWall = currentTile.PlaceWalls(DIRECTION.RIGHT);
                    walls.Add(newWall);
                    outsideWalls.Add(newWall);

                    if (i == 0)
                    {
                        currentTile.PlaceLightTopRight();
                    }
                }


                //place horizontal walls
                if(i != BS.mapHeight - 1)
                {
                    Wall newWall = currentTile.PlaceWalls(DIRECTION.BOTTOM, false, false);
                    walls.Add(newWall);
                    horizontalWalls.Add(newWall);
                }
                
                //place vertical walls
                if(j != BS.mapWidth - 1)
                {
                    Wall newWall = currentTile.PlaceWalls(DIRECTION.RIGHT, false, false);
                    walls.Add(newWall);
                    verticalWalls.Add(newWall);
                }
             

                //tiles[index].PlaceSideWalls(true, true, true, true);

                tilePos.x += tilePrefab.GetHeight();
                
                index++;
            }
            tilePos.x = originXTilePos * tilePrefab.GetWidth();

            tilePos.y -= tilePrefab.GetWidth();
        }
    }
}
