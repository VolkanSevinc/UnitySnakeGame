using System.Collections.Generic;
using GridBase;
using Interactions;
using Providers;
using ScriptableObjects;
using Snake;
using UnityEngine;
using Util.Enums;
using Grid = UnityEngine.Grid;

namespace Managers
{
    public struct GridBounds
    {
        public Vector3 P0;
        public Vector3 P1;
        public Vector3 P2;
        public Vector3 P3;
    }

    public class GridManager : MonoBehaviour
    {
        private static GameObject _modelProviderDefaultGridModel;
        public static GridManager Instance { get; private set; }

        [SerializeField] private GameObject gridCellPrefab;
        [SerializeField] private GameObject gridParent;

        public GridBounds GridBounds { get; private set; }
        private GridBase.Grid _currentGrid;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            _modelProviderDefaultGridModel = InfrastructureProvider.Instance.PrefabProvider.DefaultGridModel;
        }

        public void LoadLevel(LevelScriptableObject currentLevel)
        {
            CreateGrid(currentLevel);
        }

        private void CreateGrid(LevelScriptableObject currentLevel)
        {
            var gridSize = currentLevel.GridSize;
            _currentGrid = new GridBase.Grid(gridSize);

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    var gridCell = CreateGridCell(x, y, out var cellScript);

                    CreateGridCellModel(currentLevel, x, y, gridCell, cellScript);

                    _currentGrid.GridCells[x, y] = cellScript;
                }
            }

            GridBounds = new GridBounds
            {
                P0 = _currentGrid.GridCells[0, 0].transform.position,
                P1 = _currentGrid.GridCells[0, gridSize.y - 1].transform.position,
                P2 = _currentGrid.GridCells[gridSize.x - 1, gridSize.y - 1].transform.position,
                P3 = _currentGrid.GridCells[gridSize.x - 1, 0].transform.position,
            };
        }

        private GameObject CreateGridCell(int x, int y, out GridCell cellScript)
        {
            GameObject gridCell = Instantiate(gridCellPrefab, gridParent.transform);

            gridCell.name = $"GridCell_{x}_{y}";

            gridCell.transform.SetParent(gridParent.transform);
            gridCell.transform.position = new Vector3(x, 0, y);

            cellScript = gridCell.GetComponent<GridCell>();
            cellScript.SetGridPosition(x, y);

            return gridCell;
        }

        private void CreateGridCellModel(LevelScriptableObject currentLevel, int x, int y, GameObject gridCell,
            GridCell cellScript)
        {
            var nextModelInPattern = currentLevel.NextModelInLevelPattern(_modelProviderDefaultGridModel, x, y);

            GameObject gridModel = Instantiate(nextModelInPattern, gridCell.transform);

            gridModel.transform.SetParent(gridCell.transform);
            gridModel.transform.Rotate(new Vector3(0f, 270f, 0f));
            gridModel.transform.Translate(0, cellScript.GridYOffset, 0);

            cellScript.CellModel = gridModel;
        }

        public void AddItemToRandomLocation(InteractableItem interactableItem)
        {
            GridCell randomEmptyCell = _currentGrid.GetRandomEmptyCell();

            GameObject itemGameObject = interactableItem.GetGameObject();

            itemGameObject.name += randomEmptyCell.GridPosition.x + ", " + randomEmptyCell.GridPosition.y;

            itemGameObject.transform.position =
                new Vector3(randomEmptyCell.GridPosition.x, 0, randomEmptyCell.GridPosition.y);

            interactableItem.CurrentCell = randomEmptyCell;

            randomEmptyCell.GridItems.Add(interactableItem);
        }

        public void PlaceSnakeOnGrid(List<SnakeBodyPart> snakeParts)
        {
            if (_currentGrid != null)
            {
                var currentGridGridCells = _currentGrid.GridCells;

                foreach (var snakePart in snakeParts)
                {
                    var snakePartCurrentLocation = snakePart.CurrentLocation;

                    var snakePartGridCell =
                        currentGridGridCells[snakePartCurrentLocation.x, snakePartCurrentLocation.y];

                    snakePart.SetPosition(snakePartGridCell);
                }
            }
        }

        public bool IsCoordinateInGrid(Vector2 gridPosition)
        {
            var gridSize = _currentGrid.GridSize;

            return (gridPosition.x >= 0 && gridPosition.x < gridSize.x) &&
                   ((gridPosition.y >= 0 && gridPosition.y < gridSize.y));
        }

        public GridCell GetMiddleGridCell()
        {
            var middleLoc = GetMiddleLocation();
            return _currentGrid.GridCells[middleLoc.x, middleLoc.y];
        }

        public GridCell GetGridCell(Vector2Int location)
        {
            var isCoordinateInGrid = IsCoordinateInGrid(location);

            GridCell gridCell = null;

            if (isCoordinateInGrid)
            {
                gridCell = _currentGrid.GridCells[location.x, location.y];
            }

            return gridCell;
        }

        public Vector2Int GetMiddleLocation()
        {
            return new Vector2Int((int)(_currentGrid.GridSize.x / 2), (int)(_currentGrid.GridSize.y / 2));
        }

        public void AddItemAtLocation(Vector2Int gridPosition, InteractableItem itemToAdd)
        {
            if (_currentGrid != null)
            {
                var gridCell = _currentGrid.GridCells[gridPosition.x, gridPosition.y];
                gridCell.GridItems.Add(itemToAdd);

                itemToAdd.transform.position =
                    new Vector3(gridCell.GridPosition.x, 0, gridCell.GridPosition.y);
            }
        }

        public bool IsCoordinateInGrid(Vector2Int snakeLocation, int snakeSize, Direction snakeDirection)
        {
            bool isHeadValid = IsCoordinateInGrid(snakeLocation);
            bool isTailValid = IsCoordinateInGrid(snakeLocation + snakeSize * snakeDirection.GetOpposite().GetVect());

            return isHeadValid && isTailValid;
        }
    }
}