using System;
using UnityEngine;
using UnityEngine.UI;
using Util.Enums;

namespace UI
{
    public class TouchUIManager : MonoBehaviour
    {
        [SerializeField] private Button forwardButton;
        [SerializeField] private Button backwardButton;
        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;

        public event Action<Direction> TouchInputReceived;

        private void Awake()
        {
            forwardButton.onClick.AddListener(() => { TouchInputReceived?.Invoke(Direction.Forward); });
            backwardButton.onClick.AddListener(() => { TouchInputReceived?.Invoke(Direction.Backward); });
            leftButton.onClick.AddListener(() => { TouchInputReceived?.Invoke(Direction.Left); });
            rightButton.onClick.AddListener(() => { TouchInputReceived?.Invoke(Direction.Right); });
        }
    }
}