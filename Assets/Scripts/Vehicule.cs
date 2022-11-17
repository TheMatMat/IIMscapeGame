using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MOVE_DIRECTION
{
    NONE,
    UP, 
    RIGHT, 
    DOWN, 
    LEFT
}

public class Vehicule : MonoBehaviour
{

    [Header("References")]
    public Board currentBoard;
    public Tile currentTile;

    [Header("Movement")]
    public bool isMoving = false;
    public bool canChange = true;
    public bool getOut = false;
    public float moveSpeed = 2.0f;

    public MOVE_DIRECTION direction = MOVE_DIRECTION.NONE;
    public Vector2 dirVector = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(direction != MOVE_DIRECTION.NONE && !isMoving)
        {
            isMoving = true;
            DoMove();
        }

        if (getOut)
        {
            transform.position += (Vector3)dirVector * moveSpeed * Time.deltaTime;
        }
    }

    public void OnChangeDirection(InputAction.CallbackContext callbackContext)
    {
        if (!callbackContext.started)
            return;

        if (!canChange)
            return;


        canChange = false;
        Vector2 value = callbackContext.ReadValue<Vector2>();

        if(value.x > 0 && direction != MOVE_DIRECTION.LEFT) 
        {
            direction = MOVE_DIRECTION.RIGHT;
            dirVector = value.normalized;
        }

        if(value.x < 0 && direction != MOVE_DIRECTION.RIGHT)
        {
            direction = MOVE_DIRECTION.LEFT;
            dirVector = value.normalized;
        }

        if(value.y > 0 && direction != MOVE_DIRECTION.DOWN)
        {
            direction = MOVE_DIRECTION.UP;
            dirVector = value.normalized;
        }

        if(value.y < 0 && direction != MOVE_DIRECTION.UP)
        {
            direction = MOVE_DIRECTION.DOWN;
            dirVector = value.normalized;
        }
    }

    public Tile GetNextTile()
    {
        List<Tile> tiles = currentBoard.tiles;

        switch (direction)
        {
            case MOVE_DIRECTION.UP:
                if (currentTile.posY > 0)
                    return tiles[currentTile.TileIndex - currentBoard.BS.mapWidth];
                break;

            case MOVE_DIRECTION.RIGHT:
                if (currentTile.posX < currentBoard.BS.mapWidth - 1)
                    return tiles[currentTile.TileIndex + 1];
                break;

            case MOVE_DIRECTION.DOWN:
                if(currentTile.posY < currentBoard.BS.mapHeight - 1)
                    return tiles[currentTile.TileIndex + currentBoard.BS.mapWidth];
                break;

            case MOVE_DIRECTION.LEFT:
                if(currentTile.posX > 0)
                    return tiles[currentTile.TileIndex - 1];
                break;
        }

        return null;
    }

    public void DoMove()
    {
        Tile nextTile = GetNextTile();

        if (nextTile != null)
        {
            float distanceBtwnTwoTiles = (nextTile.transform.position - currentTile.transform.position).magnitude;
            currentTile = nextTile;
            transform.DOMove(nextTile.transform.position, distanceBtwnTwoTiles / moveSpeed).SetEase(Ease.Linear).OnComplete(DoMove);
        }
        else
        {
            Debug.Log("get out");
            getOut = true;
        }
            
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.layer);
        canChange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canChange = false;
    }


}
