using System;
using System.Collections.Generic;
using Providers;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [SerializeField] private Canvas defaultCanvas;
        [SerializeField] private TouchUIManager touchUIPrefab;
        [SerializeField] private GoalUIElement goalUIPrefab;
        [SerializeField] private bool showMobileInput;

        private VerticalLayoutGroup _verticalLayoutGroup;
        private TouchUIManager _touchUI;

        private readonly Dictionary<Goal, GoalUIElement> _goalUIElements = new Dictionary<Goal, GoalUIElement>();

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
            _verticalLayoutGroup = defaultCanvas.GetComponent<VerticalLayoutGroup>();
            InitTouchControls();
        }

        private void InitTouchControls()
        {
            if (_touchUI == null)
            {
                if ((Application.platform == RuntimePlatform.Android ||
                     Application.platform == RuntimePlatform.IPhonePlayer) ||
                    (Application.platform == RuntimePlatform.WindowsEditor && showMobileInput))
                {
                    _touchUI = Instantiate(touchUIPrefab);

                    _touchUI.TouchInputReceived += direction =>
                        InputManager.Instance.MovementInputReceived(direction);
                }
            }
        }

        public void CreateGoalUI(List<Goal> goals)
        {
            foreach (var goal in goals)
            {
                CreateUIFromGoal(goal);
            }
        }

        private void CreateUIFromGoal(Goal goal)
        {
            if (_verticalLayoutGroup != null)
            {
                var goalUI = Instantiate(goalUIPrefab, _verticalLayoutGroup.transform);
                goal.InitUIElement(goalUI);

                _goalUIElements.Add(goal, goalUI);
            }
        }

        public void UpdateGoalUI(Goal goal)
        {
            if (_goalUIElements.TryGetValue(goal, out var goalUIElement))
            {
                goalUIElement.UpdateGoalCount(goal.CollectedAmount);
            }
            else
            {
                CreateUIFromGoal(goal);
            }
        }

        public void ShowFailScreen(Action tryAgainAction)
        {
            SetUIElementsEnabled(false);

            var failScreen = InfrastructureProvider.Instance.UIProvider.FailLevelScreen;
            failScreen = Instantiate(failScreen.gameObject).GetComponent<LevelChangeScreen>();

            failScreen.Show();
            failScreen.RestartButtonPressed += () =>
            {
                tryAgainAction();
                failScreen.Hide();
                SetUIElementsEnabled(true);
            };
        }

        public void ShowSuccessScreen(Action startNextLevel, Action replayLevel)
        {
            SetUIElementsEnabled(false);

            var successScreen = InfrastructureProvider.Instance.UIProvider.NextLevelScreen;
            successScreen = Instantiate(successScreen.gameObject).GetComponent<LevelChangeScreen>();

            successScreen.Show();

            successScreen.ContinueButtonPressed += () =>
            {
                startNextLevel();
                successScreen.Hide();
                SetUIElementsEnabled(true);
            };

            successScreen.RestartButtonPressed += () =>
            {
                replayLevel();
                successScreen.Hide();
                SetUIElementsEnabled(true);
            };
        }

        public void ShowStartScreen(Action startGame)
        {
            SetUIElementsEnabled(false);

            var startScreen
                = InfrastructureProvider.Instance.UIProvider.GameStartScreen;

            startScreen = Instantiate(startScreen.gameObject).GetComponent<LevelChangeScreen>();

            startScreen.Show();

            startScreen.ContinueButtonPressed += () =>
            {
                startGame();
                startScreen.Hide();
                SetUIElementsEnabled(true);
            };
        }

        public void SetUIElementsEnabled(bool isEnabled)
        {
            _touchUI?.gameObject.SetActive(isEnabled);
        }
    }
}