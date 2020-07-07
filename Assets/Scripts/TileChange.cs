using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileChange : MonoBehaviour
{
    public TileBase emptyCell;
    public TileBase snakeCell;
    public TileBase snakeHeadLeft;
    public TileBase snakeHeadUp;
    public TileBase snakeHeadRight;
    public TileBase snakeHeadDown;
    public TileBase appleCell;
    public Tilemap tilemap;
    LinkedList<Vector3Int> snakePositions;
    Vector3Int headPositionNext;
    Vector3Int snakeDirection;
    BoundsInt boardBounds;
    float gameTickSpeed;
    TileBase snakeHeadTile;
    Vector3Int applePosition;
    private bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        snakePositions = new LinkedList<Vector3Int>();
        snakePositions.AddLast(new Vector3Int(1, 0, 0));
        snakePositions.AddLast(new Vector3Int(0, 0, 0));

        snakeDirection = Vector3Int.right;
        boardBounds = tilemap.cellBounds;
        gameTickSpeed = 0.15f;
        applePosition = snakePositions.First.Value;
        snakeHeadTile = snakeHeadRight;

        tilemap.SetTile(snakePositions.First.Value, snakeHeadTile);
        tilemap.SetTile(snakePositions.Last.Value, snakeCell);
        CreateApple();

        isPaused = true;
        InvokeRepeating("GameTick", 0f, gameTickSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused) {
            float vAxis = Input.GetAxisRaw("Vertical");
            float hAxis = Input.GetAxisRaw("Horizontal");
            
            if (hAxis < 0 && snakeDirection != Vector3Int.right)
            {
                snakeDirection = Vector3Int.left;
                snakeHeadTile = snakeHeadLeft;
                tilemap.SetTile(snakePositions.First.Value, snakeHeadTile);
            }
            else if (vAxis > 0 && snakeDirection != Vector3Int.down)
            {
                snakeDirection = Vector3Int.up;
                snakeHeadTile = snakeHeadUp;
                tilemap.SetTile(snakePositions.First.Value, snakeHeadTile);
            }
            else if (hAxis > 0 && snakeDirection != Vector3Int.left)
            {
                snakeDirection = Vector3Int.right;
                snakeHeadTile = snakeHeadRight;
                tilemap.SetTile(snakePositions.First.Value, snakeHeadTile);
            }
            else if (vAxis < 0 && snakeDirection != Vector3Int.up)
            {
                snakeDirection = Vector3Int.down;
                snakeHeadTile = snakeHeadDown;
                tilemap.SetTile(snakePositions.First.Value, snakeHeadTile);
            }
        }
    }

    void GameTick()
    {
        if (!isPaused) {
            headPositionNext = snakePositions.First.Value + snakeDirection;

            if (boardBounds.Contains(headPositionNext) && !snakePositions.Contains(headPositionNext))
            {
                // Didn't hit the edge and tail
                tilemap.SetTile(snakePositions.First.Value, snakeCell);
                tilemap.SetTile(headPositionNext, snakeHeadTile);
                snakePositions.AddFirst(headPositionNext);
                
                if (headPositionNext == applePosition)
                {
                    // Eaten an apple, grown
                    CreateApple();
                }
                else
                {
                    // Hasn't eaten an apple, not grown
                    tilemap.SetTile(snakePositions.Last.Value, emptyCell);
                    snakePositions.RemoveLast();
                }
            }
            else
            {
                // Hit edge or tail, lost
                Time.timeScale = 0f;
                gameObject.SetActive(false);
            }
        }
    }

    void CreateApple()
    {
        while (snakePositions.Contains(applePosition))
        {
            applePosition = new Vector3Int(
                Random.Range(boardBounds.xMin, boardBounds.xMax),
                Random.Range(boardBounds.yMin, boardBounds.yMax),
                0
            );
        }

        tilemap.SetTile(applePosition, appleCell);
    }

    public void OnPlay()
    {
        isPaused = false;
    }

    public void OnExit()
    {
        Application.Quit();
    }
}
