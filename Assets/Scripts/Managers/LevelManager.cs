using System.Collections.Generic;
using System.Linq;
using Interactions;
using Providers;
using ScriptableObjects;
using UnityEngine;
using Util;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }
        // private static LevelScriptableObject _levelToLoad;

        [SerializeField] private GameObject itemParent; // Parent GameObject for placing items in the scene
        private LevelScriptableObject CurrentLevel { get; set; }

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

        // Called when a goal is completed
        private void OnGoalComplete()
        {
            // Check if all goals are complete and trigger victory if they are
            if (CurrentLevel.CheckGoals())
            {
                GameManager.Instance.ChangeGameState(InteractionType.Victory);
            }
        }

        public void LoadLevel(LevelScriptableObject level)
        {
            CurrentLevel = level;
            CurrentLevel.ResetGoals();
            UIManager.Instance.CreateGoalUI(CurrentLevel.Goals);

            GridManager.Instance.LoadLevel(CurrentLevel);

            PopulateGrid();

            InitAmbientParticles();
            CameraManager.Instance.Initialize(GridManager.Instance.GridBounds);
        }

        private void InitAmbientParticles()
        {
            var currentLevelAmbientParticlePrefabs = CurrentLevel.AmbientParticlePrefabs;
            var middleGridCell = GridManager.Instance.GetMiddleGridCell();

            foreach (var particlePrefab in currentLevelAmbientParticlePrefabs)
            {
                ParticleManager.Instance.PlayParticle(particlePrefab, middleGridCell.transform);
            }
        }

        // Populate the grid with the snake, preset objects, and goal items
        private void PopulateGrid()
        {
            GameManager.Instance.Snake = CurrentLevel.CreateSnake();
            CreatePresetObjects(); // Create preset interactable objects

            // Create goal items for each goal in the level
            foreach (var goal in CurrentLevel.Goals)
            {
                CreateGoalItems(goal);
            }
        }

        private void CreatePresetObjects()
        {
            foreach (var interactableItemWrapper in CurrentLevel.InteractableObjects)
            {
                CreateInteractableObject(interactableItemWrapper.Position, interactableItemWrapper.InteractableObject);
            }
        }

        private void CreateInteractableObject(Vector2Int gridPosition, InteractableItem itemToCreate)
        {
            var interactableObject = Instantiate(itemToCreate, itemParent.transform);
            GridManager.Instance.AddItemAtLocation(gridPosition, interactableObject);

            if (interactableObject is CollectableItem collectableItem)
            {
                var relatedGoal =
                    CurrentLevel.Goals.FirstOrDefault(g => g.IsCollectibleSame(collectableItem));

                if (relatedGoal != null)
                {
                    collectableItem.Goal = relatedGoal;
                    collectableItem.ItemCollected += ItemCollected;
                }
            }
        }

        private void CreateGoalItems(Goal goal)
        {
            //Didn't inline for readability
            List<InteractableItem> createdItems = goal.CreateItems(itemParent);

            foreach (var interactableItem in createdItems)
            {
                var goalItem = (CollectableItem)interactableItem;
                goalItem.ItemCollected += ItemCollected;

                GridManager.Instance.AddItemToRandomLocation(goalItem);
            }
        }

        private void ItemCollected(CollectableItem collectedItem)
        {
            bool goalCompleted = collectedItem.Goal.ItemCollected();

            UIManager.Instance.UpdateGoalUI(collectedItem.Goal);

            // If the goal is not complete, create more items for that goal
            if (goalCompleted == false)
            {
                CreateGoalItems(collectedItem.Goal);
            }
            else
            {
                OnGoalComplete();
            }
        }

        public LevelScriptableObject GetNextLevel()
        {
            return CurrentLevel.NextLevel;
        }
    }
}