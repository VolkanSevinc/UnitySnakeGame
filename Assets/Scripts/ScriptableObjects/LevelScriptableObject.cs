using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Managers;
using UnityEngine;
using Util;
using Util.Enums;

// ReSharper disable InconsistentNaming

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Level/LevelData", order = 1)]
    public class LevelScriptableObject : ScriptableObject
    {
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private Vector2Int snakeLocation;

        [SerializeField] private int snakeSize;
        [SerializeField] private Direction snakeDirection;

        [SerializeField] private List<Goal> goals;

        [SerializeField] private List<GameObject> ambientParticlePrefabs;
        [SerializeField] private List<GameObject> modelPattern;
        [CanBeNull] [SerializeField] private LevelScriptableObject nextLevel;

        [HideInInspector] [SerializeField] public List<InteractableItemWrapper> interactableObjects;

        public Vector2Int GridSize => gridSize;
        public List<Goal> Goals => goals;

        public List<InteractableItemWrapper> InteractableObjects
        {
            get => interactableObjects;
            set => interactableObjects = value;
        }

        [CanBeNull] public LevelScriptableObject NextLevel => nextLevel;
        public List<GameObject> AmbientParticlePrefabs => ambientParticlePrefabs;

        public Vector2Int SnakeLocation
        {
            get => snakeLocation;
            set => snakeLocation = value;
        }

        public int SnakeSize
        {
            get => snakeSize;
            set => snakeSize = value;
        }

        public Direction SnakeDirection => snakeDirection;

        public void ResetGoals()
        {
            foreach (var goal in goals)
            {
                goal.CollectedAmount = 0;
                goal.CurrentAmountOnScreen = 0;
            }
        }

        public GameObject NextModelInLevelPattern(GameObject defaultModel, int x, int y)
        {
            var nextModelInLevelPattern =
                modelPattern.Count == 0 ? defaultModel : modelPattern[(x + y) % modelPattern.Count];

            return nextModelInLevelPattern;
        }

        public Snake.Snake CreateSnake()
        {
            Vector2Int tempSnakeLocation = snakeLocation;

            bool isCoordinateValid = GridManager.Instance.IsCoordinateInGrid(snakeLocation, snakeSize, snakeDirection);

            if (isCoordinateValid == false)
            {
                tempSnakeLocation = GridManager.Instance.GetMiddleLocation();
            }

            return new Snake.Snake(tempSnakeLocation, snakeSize, snakeDirection);
        }

        public bool CheckGoals()
        {
            return goals.All(goal => goal.IsCompleted());
        }
    }
}