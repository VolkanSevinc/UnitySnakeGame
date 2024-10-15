using System;
using System.Collections;
using ScriptableObjects;
using Snake;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;
using Util.Enums;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private float tickRate = 1f;
        [SerializeField] private LevelScriptableObject firstLevel;

        public event Action OnTick;
        public Snake.Snake Snake { get; set; }
        private static LevelScriptableObject _levelToLoad;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (_levelToLoad == null)
            {
                UIManager.Instance.ShowStartScreen(StartGame);
            }
            else
            {
                LevelManager.Instance.LoadLevel(_levelToLoad);
            }

            StartCoroutine(GameLoop());
        }

        private void Tick()
        {
            var input = InputManager.Instance.ConsumeInput();

            OnTick?.Invoke();

            if (Snake != null)
            {
                Snake.MoveInDirection(input);
            }
        }

        private IEnumerator GameLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f / tickRate);
                Tick();
            }
        }

        public void ChangeGameState(InteractionType interactionType)
        {
            if (interactionType == InteractionType.Fail)
            {
                EndGame();
            }
            else if (interactionType == InteractionType.Victory)
            {
                OnLevelSuccess();
            }
        }

        private void LoadNewLevel()
        {
            _levelToLoad = LevelManager.Instance.GetNextLevel();
            SceneManager.LoadScene($"SampleScene");
        }

        private void EndGame()
        {
            StopGame();
            UIManager.Instance.ShowFailScreen(RestartLevel);
        }

        private void RestartLevel()
        {
            SceneManager.LoadScene($"SampleScene");
        }

        private void OnLevelSuccess()
        {
            StopGame();
            UIManager.Instance.ShowSuccessScreen(LoadNewLevel, RestartLevel);
        }

        private void StopGame()
        {
            UIManager.Instance.SetUIElementsEnabled(false);
            StopAllCoroutines();
        }

        public void ChangeGameTick(float amount)
        {
            tickRate *= amount;
        }

        private void StartGame()
        {
            _levelToLoad = firstLevel;
            LevelManager.Instance.LoadLevel(_levelToLoad);
        }
    }
}