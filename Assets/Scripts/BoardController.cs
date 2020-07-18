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

        public Vector3Int SnakeDirection { get; private set; }

        private LinkedList<Vector3Int> snakePositions;
        private TileBase snakeHeadTile;
        private Vector3Int headPositionNext;

        private Vector3Int applePosition;

        private Tilemap tilemap;
        private BoundsInt boardBounds;

        private Dictionary<Vector3Int, TileBase> directionToHeadTile;
        private Dictionary<Vector3Int, TileBase> directionToDeadHeadTile;

        public void InitBoard()
        {
            SnakeDirection = Vector3Int.right;
            snakeHeadTile = snakeHeadRight;

            snakePositions = new LinkedList<Vector3Int>();
            snakePositions.AddFirst(boardBounds.size / 2);
            snakePositions.AddFirst(snakePositions.First.Value + SnakeDirection);

            applePosition = snakePositions.First.Value;

            tilemap.SetTile(snakePositions.First.Value, snakeHeadTile);
            tilemap.SetTile(snakePositions.Last.Value, snakeCell);
            CreateApple();
        }

        public void rotateSnake(Vector3Int direction)
        {
            SnakeDirection = direction;
            snakeHeadTile = directionToHeadTile[direction];
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
            headPositionNext = snakePositions.First.Value + SnakeDirection;

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
                snakeHeadTile = directionToDeadHeadTile[SnakeDirection];
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

            directionToHeadTile = new Dictionary<Vector3Int, TileBase>
            {
                { Vector3Int.left, snakeHeadLeft },
                { Vector3Int.right, snakeHeadRight },
                { Vector3Int.up, snakeHeadUp },
                { Vector3Int.down, snakeHeadDown }
            };

            directionToDeadHeadTile = new Dictionary<Vector3Int, TileBase>
            {
                { Vector3Int.left, snakeHeadDeadLeft },
                { Vector3Int.right, snakeHeadDeadRight },
                { Vector3Int.up, snakeHeadDeadUp },
                { Vector3Int.down, snakeHeadDeadDown }
            };
        }
    }
}
