using System;
using System.Collections.Generic;
using Interactions;
using UI;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Util
{
    [Serializable]
    public class Goal
    {
        [SerializeField] private int amountToReach;
        [SerializeField] private CollectableItem goalItem;
        [SerializeField] private int maxAmountOnScreen;

        public int CurrentAmountOnScreen { get; set; }
        public int CollectedAmount { get; set; }

        public List<InteractableItem> CreateItems(GameObject itemParent)
        {
            List<InteractableItem> createdObjects = new List<InteractableItem>();

            //Calculate how many items are needed to reach the goal
            int spawnAmount = Math.Min(amountToReach - CollectedAmount - CurrentAmountOnScreen,
                maxAmountOnScreen - CurrentAmountOnScreen);

            for (int i = 0; i < spawnAmount; i++)
            {
                if (itemParent != null)
                {
                    CollectableItem itemInstance = Object.Instantiate(goalItem, itemParent.transform);

                    itemInstance.Goal = this;

                    CurrentAmountOnScreen++;

                    createdObjects.Add(itemInstance);
                }
            }

            return createdObjects;
        }

        public bool ItemCollected()
        {
            CollectedAmount++;
            CurrentAmountOnScreen--;

            return IsCompleted();
        }

        public bool IsCompleted()
        {
            return CollectedAmount == amountToReach;
        }

        public void InitUIElement(GoalUIElement uiElement)
        {
            uiElement.Init(goalItem.GoalIcon, CollectedAmount, amountToReach);
        }

        public bool IsCollectibleSame(CollectableItem collectableItem)
        {
            return collectableItem.CompareCollectibleItem(goalItem);
        }
    }
}