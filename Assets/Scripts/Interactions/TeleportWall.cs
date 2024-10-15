using System;
using System.Collections.Generic;
using GridBase;
using Managers;
using Unity.VisualScripting;
using UnityEngine;
using Util.Enums;

namespace Interactions
{
    public class TeleportWall : InteractableItem
    {
        [SerializeField] private Direction firstDirection;
        [SerializeField] private Direction secondDirection;
        [SerializeField] private GameObject teleportParticlePrefab;

        public override List<Interaction> OnNextInteraction(Snake.Snake interactingSnake)
        {
            return new List<Interaction>();
        }

        public override List<Interaction> OnCurrentInteraction(Snake.Snake interactingSnake)
        {
            var snakeApproachDirection = interactingSnake.GetDirection().GetOpposite();

            if (snakeApproachDirection == firstDirection || snakeApproachDirection == secondDirection)
            {
                GridCell approachCell = interactingSnake.GetSnakeHead().CurrentGridCell;

                Direction teleportDirection =
                    snakeApproachDirection == firstDirection ? secondDirection : firstDirection;

                InputManager.Instance.MovementInputReceived(teleportDirection);

                interactingSnake.MoveSnakeHeadBy(
                    teleportDirection.GetVect(), teleportDirection);

                approachCell.GridItems.Remove(interactingSnake.GetSnakeHead());

                ParticleManager.Instance.PlayParticle(teleportParticlePrefab, transform);

                return new List<Interaction>();
            }

            return base.OnCurrentInteraction(interactingSnake);
        }
    }
}