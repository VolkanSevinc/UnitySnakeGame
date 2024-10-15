using GridBase;
using Interactions;
using UnityEngine;
using Util;
using Util.Enums;

namespace Snake
{
    public class SnakeBodyPart : InteractableItem
    {
        public GridCell CurrentGridCell { get; private set; }
        public GridCell PreviousGridCell { get; private set; }
        public Direction Direction { get; set; }
        public Direction PreviousDirection { get; private set; }

        public Vector2Int CurrentLocation { get; set; }

        private void Awake()
        {
            interactions.Add(new Interaction()
            {
                InteractionType = InteractionType.Fail,
            });
        }

        public void SetPosition(GridCell gridCell)
        {
            if (gridCell != null)
            {
                PreviousGridCell = CurrentGridCell;
                CurrentGridCell = gridCell;

                PreviousGridCell?.GridItems.Remove(this);
                CurrentGridCell.GridItems.Add(this);
                CurrentLocation = gridCell.GridPosition;

                transform.position = gridCell.transform.position;
            }
        }

        public void SetDirection(Direction dir)
        {
            transform.rotation = dir.GetRotation();
            PreviousDirection = Direction;
            Direction = dir;
        }
    }
}