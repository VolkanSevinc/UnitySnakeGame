using GridBase;
using Providers;
using UnityEngine;
using Util.Enums;

namespace Snake
{
    public static class SnakeUtil
    {
        public static SnakeBodyPart CreateSnakeHead(Vector2Int headLocation, Direction snakeDirection)
        {
            var snakeHead = CreateSnakePartFromPrefab(InfrastructureProvider.Instance.PrefabProvider.SnakeHeadPrefab,
                headLocation,
                snakeDirection);

            snakeHead.gameObject.name = "SnakeHead";

            return snakeHead;
        }

        public static SnakeBodyPart CreateSnakeBody(Vector2Int headLocation, int bodyOffset,
            Direction snakeDirection)
        {
            var newLocation = headLocation + snakeDirection.GetOpposite().GetVect() * bodyOffset;
            var snakeBody = CreateSnakePartFromPrefab(InfrastructureProvider.Instance.PrefabProvider.SnakeBodyPrefab,
                newLocation, snakeDirection);
            
            snakeBody.gameObject.name = "SnakeBody " + newLocation.x + "," + newLocation.y;

            return snakeBody;
        }

        public static SnakeBodyPart CreateSnakeTail(Vector2Int headLocation, int snakeLength,
            Direction snakeDirection)
        {
            var newLocation = headLocation + snakeDirection.GetOpposite().GetVect() * snakeLength;
            var snakeTail = CreateSnakePartFromPrefab(InfrastructureProvider.Instance.PrefabProvider.SnakeTailPrefab,
                newLocation, snakeDirection);

            snakeTail.gameObject.name = "SnakeTail";

            return snakeTail;
        }

        private static SnakeBodyPart CreateSnakePartFromPrefab(GameObject prefab, Vector2Int gridLocation,
            Direction snakeDirection)
        {
            var snakeObject = Object.Instantiate(prefab);
            var snakePart = snakeObject.GetComponent<SnakeBodyPart>();

            snakePart.SetDirection(snakeDirection);
            snakePart.CurrentLocation = gridLocation;

            return snakePart;
        }
    }
}