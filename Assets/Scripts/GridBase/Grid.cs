using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GridBase
{
    public class Grid
    {
        public GridCell[,] GridCells { get; }
        public Vector2 GridSize { get; }


        public Grid(int width, int height) : this(new Vector2Int(width, height))
        {
        }

        public Grid(Vector2Int gridSize)
        {
            GridSize = gridSize;
            GridCells = new GridCell[gridSize.x, gridSize.y];
        }

        public GridCell GetRandomEmptyCell()
        {
            List<GridCell> emptyCells = new List<GridCell>();

            foreach (var gridCell in GridCells)
            {
                if (gridCell.GridItems.Count == 0)
                {
                    emptyCells.Add(gridCell);
                }
            }

            return emptyCells[Random.Range(0, emptyCells.Count)];
        }
    }
}