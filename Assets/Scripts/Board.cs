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
    public Vehicule motoPrefab;

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

        PlaceTilesAndWalls();
        ActivateDangerousWalls();

        SpawnVehicule();
    }

    public void PlaceTilesAndWalls()
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

                currentTile.posX = j;
                currentTile.posY = i;

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
                    Wall newWall = currentTile.PlaceWalls(DIRECTION.BOTTOM, WALL_TYPE.HORIZONTAL, false, false);
                    walls.Add(newWall);
                    horizontalWalls.Add(newWall);
                }
                
                //place vertical walls
                if(j != BS.mapWidth - 1)
                {
                    Wall newWall = currentTile.PlaceWalls(DIRECTION.RIGHT, WALL_TYPE.VERTICAL, false, false);
                    walls.Add(newWall);
                    verticalWalls.Add(newWall);
                }

                tilePos.x += tilePrefab.GetHeight();
                
                index++;
            }
            tilePos.x = originXTilePos * tilePrefab.GetWidth();

            tilePos.y -= tilePrefab.GetWidth();
        }
    }

    public void ActivateDangerousWalls()
    {
        foreach(WallState WS in BS.horizontalDangerousWalls)
        {
            if(WS.index < horizontalWalls.Count)
                horizontalWalls[WS.index].SetAndApplyChanges(WS.isVisible, WS.isDangerous);
        }

        foreach (WallState WS in BS.VerticalDangerousWalls)
        {
            if (WS.index < verticalWalls.Count)
                verticalWalls[WS.index].SetAndApplyChanges(WS.isVisible, WS.isDangerous);
        }
    }

    public void SpawnVehicule(GameObject currentVehicule = null)
    {
        if(currentVehicule != null)
            Destroy(currentVehicule);

        Vehicule vehicule = Instantiate(motoPrefab, tiles[BS.entranceIndex].transform.position, Quaternion.identity);
        vehicule.currentBoard = this;
        vehicule.currentTile = tiles[16];
    }
}
