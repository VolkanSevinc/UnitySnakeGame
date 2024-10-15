using Interactions;
using UnityEngine;

namespace Util
{
    [System.Serializable]
    public class InteractableItemWrapper
    {
        [SerializeField] private Vector2Int position;
        [SerializeField] private InteractableItem interactableObject;

        public Vector2Int Position
        {
            get => position;
            set => position = value;
        }

        public InteractableItem InteractableObject
        {
            get => interactableObject;
            set => interactableObject = value;
        }
    }
}