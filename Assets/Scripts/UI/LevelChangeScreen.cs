using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelChangeScreen : MonoBehaviour
    {
        [CanBeNull] [SerializeField] private Button restartButton;
        [CanBeNull] [SerializeField] private Button continueButton;

        public event Action ContinueButtonPressed;
        public event Action RestartButtonPressed;

        private void Start()
        {
            continueButton?.onClick.AddListener(() => { ContinueButtonPressed?.Invoke(); });
            restartButton?.onClick.AddListener(() => { RestartButtonPressed?.Invoke(); });
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}