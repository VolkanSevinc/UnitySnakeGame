using System;
using System.Collections.Generic;
using System.Linq;
using GridBase;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using Util;
using Util.Enums;
using Random = UnityEngine.Random;

namespace Interactions
{
    public class AvoidantCollectible : CollectableItem
    {
        [SerializeField] private int moveAmount;

        private void Start()
        {
            GameManager.Instance.OnTick += MoveRandom;
        }

        private void MoveRandom()
        {
            GridCell newCell;
            var directionValues = DirectionExtensions.GetValues();

            do
            {
                if (directionValues.Count > 0)
                {
                    Direction moveDirection = directionValues[Random.Range(0, directionValues.Count)];
                    newCell = GridManager.Instance.GetGridCell(CurrentCell.GridPosition +
                                                               moveDirection.GetVect() * moveAmount);
                    directionValues.Remove(moveDirection);
                }
                else
                {
                    return;
                }
            } while (newCell == null || newCell.GridItems.Count != 0);

            MoveItemToNewCell(newCell);
        }

        private void MoveItemToNewCell(GridCell newCell)
        {
            CurrentCell.GridItems.Remove(this);

            CurrentCell = newCell;

            newCell.GridItems.Add(this);
            transform.position = newCell.transform.position;
        }

        public void OnDestroy()
        {
            CurrentCell.GridItems.Remove(this);
            GameManager.Instance.OnTick -= MoveRandom;
        }
    }
}