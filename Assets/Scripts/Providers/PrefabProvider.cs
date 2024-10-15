using System.Collections.Generic;
using UnityEngine;

namespace Providers
{
    public class PrefabProvider : MonoBehaviour
    {
        [SerializeField] private GameObject defaultGridModel;
        [SerializeField] private GameObject snakeHeadPrefab;
        [SerializeField] private GameObject snakeTailPrefab;
        [SerializeField] private GameObject snakeBodyPrefab;
        public GameObject DefaultGridModel => defaultGridModel;

        public GameObject SnakeHeadPrefab => snakeHeadPrefab;

        public GameObject SnakeTailPrefab => snakeTailPrefab;

        public GameObject SnakeBodyPrefab => snakeBodyPrefab;

        void Start()
        {
        }

        void Update()
        {
        }
    }
}