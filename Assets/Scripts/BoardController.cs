using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Snake
{
    public class BoardController : MonoBehaviour
    {
        public TileBase emptyCell;
        public TileBase snakeCell;
        public TileBase appleCell;

        public TileBase snakeHeadLeft;
        public TileBase snakeHeadUp;
        public TileBase snakeHeadRight;
        public TileBase snakeHeadDown;

        public TileBase snakeHeadDeadLeft;
        public TileBase snakeHeadDeadUp;
        public TileBase snakeHeadDeadRight;
        public TileBase snakeHeadDeadDown;

        private LinkedList<Vector3Int> snakePositions;
        private Vector3Int snakeDirection;
        private TileBase snakeHeadTile;
        private Vector3Int headPositionNext;

        private Vector3Int applePosition;

        private Tilemap tilemap;
        private BoundsInt boardBounds;

        public void InitBoard()
        {
            snakeDirection = Vector3Int.right;
            snakeHeadTile = snakeHeadRight;

            snakePositions = new LinkedList<Vector3Int>();
            snakePositions.AddFirst(boardBounds.size / 2);
            snakePositions.AddFirst(snakePositions.First.Value + snakeDirection);

            applePosition = snakePositions.First.Value;

            tilemap.SetTile(snakePositions.First.Value, snakeHeadTile);
            tilemap.SetTile(snakePositions.Last.Value, snakeCell);
            CreateApple();
        }

        public void rotateSnake(float vAxis, float hAxis)
        {
            if (hAxis < 0 && snakeDirection != Vector3Int.right)
            {
                snakeDirection = Vector3Int.left;
                snakeHeadTile = snakeHeadLeft;
            }
            else if (vAxis > 0 && snakeDirection != Vector3Int.down)
            {
                snakeDirection = Vector3Int.up;
                snakeHeadTile = snakeHeadUp;
            }
            else if (hAxis > 0 && snakeDirection != Vector3Int.left)
            {
                snakeDirection = Vector3Int.right;
                snakeHeadTile = snakeHeadRight;
            }
            else if (vAxis < 0 && snakeDirection != Vector3Int.up)
            {
                snakeDirection = Vector3Int.down;
                snakeHeadTile = snakeHeadDown;
            }

            tilemap.SetTile(snakePositions.First.Value, snakeHeadTile);
        }

        public void CreateApple()
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

        public GameState GetGameState()
        {
            headPositionNext = snakePositions.First.Value + snakeDirection;

            if (headPositionNext == applePosition)
            {
                return GameState.AppleEaten;
            }
            else if (boardBounds.Contains(headPositionNext) && !snakePositions.Contains(headPositionNext))
            {
                return GameState.NothingHappened;
            }
            else
            {
                return GameState.Lost;
            }
        }

        public void UpdateBoard(GameState gameState)
        {
            if (gameState == GameState.Lost)
            {
                if (snakeDirection == Vector3Int.left)
                {
                    snakeHeadTile = snakeHeadDeadLeft;
                }
                else if (snakeDirection == Vector3Int.up)
                {
                    snakeHeadTile = snakeHeadDeadUp;
                }
                else if (snakeDirection == Vector3Int.right)
                {
                    snakeHeadTile = snakeHeadDeadRight;
                }
                else // Down
                {
                    snakeHeadTile = snakeHeadDeadDown;
                }

                tilemap.SetTile(snakePositions.First.Value, snakeHeadTile);
            }
            else
            {
                tilemap.SetTile(snakePositions.First.Value, snakeCell);
                tilemap.SetTile(headPositionNext, snakeHeadTile);
                snakePositions.AddFirst(headPositionNext);

                if (gameState != GameState.AppleEaten)
                {
                    tilemap.SetTile(snakePositions.Last.Value, emptyCell);
                    snakePositions.RemoveLast();
                }
            }
        }

        public void ClearBoard()
        {
            foreach (Vector3Int position in snakePositions)
            {
                tilemap.SetTile(position, emptyCell);
            }

            tilemap.SetTile(applePosition, emptyCell);
        }

        private void Start()
        {
            tilemap = GetComponent<Tilemap>();
            boardBounds = tilemap.cellBounds;
        }
    }
}
