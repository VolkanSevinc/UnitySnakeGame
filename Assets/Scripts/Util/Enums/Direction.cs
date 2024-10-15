using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Util.Enums
{
    public enum Direction
    {
        Forward,
        Backward,
        Right,
        Left
    }

    public static class DirectionExtensions
    {
        public static Direction GetOpposite(this Direction direction)
        {
            return direction switch
            {
                Direction.Forward => Direction.Backward,
                Direction.Backward => Direction.Forward,
                Direction.Right => Direction.Left,
                Direction.Left => Direction.Right,
                _ => Direction.Forward
            };
        }

        public static Quaternion GetRotation(this Direction direction)
        {
            return direction switch
            {
                Direction.Forward => Quaternion.identity,
                Direction.Backward => Quaternion.Euler(0, 180, 0),
                Direction.Right => Quaternion.Euler(0, 90, 0),
                Direction.Left => Quaternion.Euler(0, -90, 0),
                _ => Quaternion.identity
            };
        }

        public static Vector2Int GetVect(this Direction direction)
        {
            return direction switch
            {
                Direction.Forward => new Vector2Int(0, 1),
                Direction.Backward => new Vector2Int(0, -1),
                Direction.Right => new Vector2Int(1, 0),
                Direction.Left => new Vector2Int(-1, 0),
                _ => new Vector2Int(0, 1)
            };
        }

        public static List<Direction> GetValues()
        {
            return Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
        }
    }
}