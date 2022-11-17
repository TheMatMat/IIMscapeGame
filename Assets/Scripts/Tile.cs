using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DIRECTION
{
    TOP,
    RIGHT,
    BOTTOM,
    LEFT
}

public class Tile : MonoBehaviour
{
    [Header("Prefabs")]
    public Wall wallHorizontalPrefab;
    public Wall wallVerticalPrefab;
    public MyLight myLightPrefab;

    [Header("Indexing")]
    public static int nbOfTile = 0;
    [SerializeField]
    private int tileIndex;
    public int posX, posY;
    public int TileIndex { get { return tileIndex; } set { tileIndex = value; } }

    [Header("Components")]
    [SerializeField]
    private SpriteRenderer sr;

    [Header("References")]
    public Board myBoard;
    public GameObject allMyLights;

    // Start is called before the first frame update
    void Start()
    {
        /*sr = GetComponent<SpriteRenderer>();*/

       // Debug.Log("height: " + sizeHeight + " / width: " + sizeWidth);
    }

    public float GetWidth()
    {
        return sr.size.x;
    }
    public float GetHeight()
    {
        return sr.size.y;
    }

    public Wall PlaceWalls(DIRECTION dir, WALL_TYPE wallType = WALL_TYPE.OUTSIDE, bool _isVisible = true, bool _isDangerous = true)
    {
        Wall wall = null;
        string parentPath = "Walls";

        switch (wallType)
        {
            case WALL_TYPE.OUTSIDE:
                parentPath = "Walls/OutsideWalls";
                break;
            case WALL_TYPE.HORIZONTAL:
                parentPath = "Walls/HorizontalWalls";
                break;
            case WALL_TYPE.VERTICAL:
                parentPath = "Walls/VerticalWalls";
                break;
            default:
                parentPath = "Walls";
                break;
        }

        switch (dir)
        {
            case DIRECTION.TOP:
                wall = Instantiate(wallHorizontalPrefab, this.transform.parent.Find(parentPath).transform);
                wall.transform.position = (Vector2)this.transform.position + new Vector2(0, GetHeight() / 2);
                break;
            case DIRECTION.RIGHT:
                wall = Instantiate(wallVerticalPrefab, this.transform.parent.Find(parentPath).transform);
                wall.transform.position = (Vector2)this.transform.position + new Vector2(GetWidth() / 2, 0);
                break;
            case DIRECTION.BOTTOM:
                wall = Instantiate(wallHorizontalPrefab, this.transform.parent.Find(parentPath).transform);
                wall.transform.position = (Vector2)this.transform.position - new Vector2(0, GetHeight() / 2);
                break;
            case DIRECTION.LEFT:
                wall = Instantiate(wallVerticalPrefab, this.transform.parent.Find(parentPath).transform);
                wall.transform.position = (Vector2)this.transform.position - new Vector2(GetWidth() / 2, 0);
                break;
        }

        wall.Init();
        wall.SetAndApplyChanges(_isVisible, _isDangerous);
        return wall;
    }

    public void PlaceLightTopLeft()
    {
        MyLight topLeftLight = Instantiate(myLightPrefab, this.transform.parent.Find("MyLights").transform);
        topLeftLight.transform.position = (Vector2)this.transform.position + new Vector2(-GetWidth() / 2, GetHeight() / 2);
    }
    public void PlaceLightTopRight()
    {
        MyLight topRightLight = Instantiate(myLightPrefab, this.transform.parent.Find("MyLights").transform);
        topRightLight.transform.position = (Vector2)this.transform.position + new Vector2(GetWidth() / 2, GetHeight() / 2);
    }
    public void PlaceLightBottomRight()
    {
        MyLight bottomRightLight = Instantiate(myLightPrefab, this.transform.parent.Find("MyLights").transform);
        bottomRightLight.transform.position = (Vector2)this.transform.position + new Vector2(GetWidth() / 2, - GetHeight() / 2);
    }
    public void PlaceLightBottomLeft()
    {
        MyLight bottomLeftLight = Instantiate(myLightPrefab, this.transform.parent.Find("MyLights").transform);
        bottomLeftLight.transform.position = (Vector2)this.transform.position + new Vector2(- GetWidth() / 2, - GetHeight() / 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
