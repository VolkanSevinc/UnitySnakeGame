using System.Collections.Generic;
using System.Linq;
using GridBase;
using Interactions;
using Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Util;
using Util.Enums;

namespace Snake
{
    public class Snake
    {
        private readonly List<SnakeBodyPart> _snakeParts = new List<SnakeBodyPart>();

        public Snake(Vector2Int headLocation, int snakeLength, Direction snakeDirection)
        {
            SnakeBodyPart snakeBodyHead = SnakeUtil.CreateSnakeHead(headLocation, snakeDirection);
            _snakeParts.Add(snakeBodyHead);

            for (int i = 1; i < snakeLength - 1; i++)
            {
                SnakeBodyPart snakeBodyBodyPart = SnakeUtil.CreateSnakeBody(headLocation, i, snakeDirection);
                _snakeParts.Add(snakeBodyBodyPart);
            }

            SnakeBodyPart snakeBodyTail = SnakeUtil.CreateSnakeTail(headLocation, snakeLength - 1, snakeDirection);
            _snakeParts.Add(snakeBodyTail);

            GridManager.Instance.PlaceSnakeOnGrid(_snakeParts);
        }

        private void AddSnakePart()
        {
            var snakeTail = _snakeParts.Last();

            SnakeBodyPart snakeBodyPart = SnakeUtil.CreateSnakeBody(_snakeParts.First().CurrentLocation,
                _snakeParts.Count - 2, snakeTail.Direction);

            snakeBodyPart.SetPosition(snakeTail.CurrentGridCell);

            snakeTail.SetPosition(snakeTail.PreviousGridCell);
            snakeTail.SetDirection(snakeTail.PreviousDirection);

            _snakeParts.Insert(_snakeParts.Count - 1, snakeBodyPart);
        }

        public void MoveInDirection(Direction? moveDirection)
        {
            var snakeHead = _snakeParts.First();

            var direction = moveDirection ?? snakeHead.Direction;

            if (direction == snakeHead.Direction.GetOpposite())
            {
                return;
            }

            snakeHead.Direction = direction;

            GridCell newCell = GridManager.Instance.GetGridCell(snakeHead.CurrentLocation + direction.GetVect());

            // if (newCell == null)
            // {
            //     GameManager.Instance.ChangeGameState(InteractionType.Fail);
            // }

            CheckForInteractions(newCell);

            MoveSnakeBody(newCell, direction);

            CleanPreviousGridCell();

            CheckForCurrentInteractions(newCell);
        }

        public void MoveSnakeHeadBy(Vector2Int moveAmount, Direction arrivalDirection)
        {
            var snakeHead = _snakeParts.First();

            var newCell = GridManager.Instance.GetGridCell(snakeHead.CurrentLocation + moveAmount);

            CheckForInteractions(newCell);

            snakeHead.SetPosition(newCell);
            snakeHead.SetDirection(arrivalDirection);
        }

        private void MoveSnakeBody(GridCell newCell, Direction direction)
        {
            MoveSnakeBody(newCell, 0, direction);
        }

        private void MoveSnakeBody(GridCell newCell, int moveStartPos, Direction direction)
        {
            if (newCell)
            {
                for (var index = moveStartPos; index < _snakeParts.Count; index++)
                {
                    var snakePart = _snakeParts[index];

                    snakePart.SetDirection(direction);
                    snakePart.SetPosition(newCell);

                    direction = snakePart.PreviousDirection;
                    newCell = snakePart.PreviousGridCell;
                }
            }
        }

        private void CheckForInteractions(GridCell newCell)
        {
            if (newCell && newCell.GridItems != null)
            {
                foreach (var gridItem in newCell.GridItems)
                {
                    if (gridItem != null)
                    {
                        var interactionList = gridItem.OnNextInteraction(this);

                        foreach (var interaction in interactionList)
                        {
                            ProcessInteraction(interaction);
                        }
                    }
                }
            }
        }

        private void CheckForCurrentInteractions(GridCell newCell)
        {
            if (newCell && newCell.GridItems != null)
            {
                List<InteractableItem> tempList = new List<InteractableItem>();
                tempList.AddRange(newCell.GridItems);

                foreach (var gridItem in tempList)
                {
                    var interactionList = gridItem.OnCurrentInteraction(this);

                    foreach (var interaction in interactionList)
                    {
                        ProcessInteraction(interaction);
                    }
                }
            }
        }

        private void ProcessInteraction(Interactions.Interaction interaction)
        {
            var interactionType = interaction.InteractionType;
            var amount = interaction.Amount;

            if (interactionType == InteractionType.AffectSnake)
            {
                if (amount > 0)
                {
                    AddSnakePart();
                }
                else if (amount < 0)
                {
                    RemoveSnakePart();
                }
            }
            else if (interactionType is InteractionType.Victory or InteractionType.Fail)
            {
                GameManager.Instance.ChangeGameState(interactionType);
            }
            else if (interactionType == InteractionType.AffectSpeed)
            {
                GameManager.Instance.ChangeGameTick(amount);
            }
        }

        private void RemoveSnakePart()
        {
            if (_snakeParts.Count > 2)
            {
                var snakeBodyPart = _snakeParts[1];
                _snakeParts.Remove(snakeBodyPart);

                var snakeTail = _snakeParts.Last();
                snakeTail.CurrentGridCell.GridItems.Remove(snakeTail);

                MoveSnakeBody(snakeBodyPart.CurrentGridCell, 1, snakeBodyPart.Direction);
                Object.Destroy(snakeBodyPart.gameObject);
            }
            else
            {
                DestroySnake(0);
                GameManager.Instance.ChangeGameState(InteractionType.Fail);
            }
        }

        private void DestroySnake(int partIndexesToRemove)
        {
            SnakeBodyPart snakeBodyPartToRemove = _snakeParts[partIndexesToRemove];

            snakeBodyPartToRemove.CurrentGridCell.GridItems.Remove(snakeBodyPartToRemove);
            Object.Destroy(snakeBodyPartToRemove.gameObject);
        }


        private void DestroySnake(List<int> partIndexesToRemove)
        {
            foreach (var snakePartIndex in partIndexesToRemove)
            {
                DestroySnake(snakePartIndex);
            }
        }

        private void CleanPreviousGridCell()
        {
            var snakeTail = _snakeParts.Last();

            if (snakeTail.PreviousGridCell is not null)
            {
                snakeTail.PreviousGridCell.GridItems.Remove(snakeTail);
            }
        }

        public Direction GetDirection()
        {
            return _snakeParts.First().Direction;
        }

        public SnakeBodyPart GetSnakeHead()
        {
            return _snakeParts.First();
        }
    }
}