using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    public GameObject explosion;

    public CinemachineVirtualCamera virtualCamera;

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
        virtualCamera = GameObject.FindGameObjectWithTag("Vcam").GetComponent<CinemachineVirtualCamera>();
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
                if (currentTile.posX < currentBoard.BS.mapWidth)
                    return tiles[currentTile.TileIndex + 1];
                break;

            case MOVE_DIRECTION.DOWN:
                if(currentTile.posY < currentBoard.BS.mapHeight)
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
        if (currentTile.TileIndex == currentBoard.BS.exitIndex)
            SceneManager.LoadScene("WinScene");

        Tile nextTile = GetNextTile();

        RotateVehicule();

        if (nextTile != null)
        {
            float distanceBtwnTwoTiles = (nextTile.transform.position - currentTile.transform.position).magnitude;
            currentTile = nextTile;
            transform.DOMove(nextTile.transform.position, distanceBtwnTwoTiles / moveSpeed).SetEase(Ease.Linear).OnComplete(DoMove);
        }
        else
        {
            Debug.Log(currentTile.posX + " : " + currentTile.posY);
            getOut = true;
        }
            
    }

    public void RotateVehicule()
    {
        //rotate
        switch (direction)
        {
            case MOVE_DIRECTION.UP:
                this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                break;

            case MOVE_DIRECTION.RIGHT:
                this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                break;

            case MOVE_DIRECTION.DOWN:
                this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                break;

            case MOVE_DIRECTION.LEFT:
                this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.layer);
        if(collision.gameObject.layer == 6)
        {
            this.dirVector = Vector2.zero;
            Debug.Log(collision.gameObject.ToString());

            collision.gameObject.GetComponent<Wall>().Activate();
            DestroyAnim();
            
        }

        canChange = true;
    }

    public void DestroyAnim()
    {
        Instantiate(explosion, this.transform.position, Quaternion.identity);
        currentBoard.SpawnVehicule(this.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canChange = false;
    }


}
