using System;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Interactions
{
    public class CollectableItem : InteractableItem
    {
        public event Action<CollectableItem> ItemCollected;

        //Icon to show on HUD
        [SerializeField] private Sprite goalIcon;
        [SerializeField] private String identifier;

        //Related goal
        public Goal Goal { get; set; }

        public Sprite GoalIcon => goalIcon;

        public override List<Interaction> OnNextInteraction(Snake.Snake interactingSnake)
        {
            ItemCollected?.Invoke(this);

            Destroy(gameObject);

            return base.OnNextInteraction(interactingSnake);
        }

        public bool CompareCollectibleItem(CollectableItem collectibleItem)
        {
            return identifier.Equals(collectibleItem.identifier);
        }
    }
}