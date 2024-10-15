using UnityEngine;
using Util.Enums;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        private Direction? _lastInput;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        void Update()
        {
            ProcessInput();
        }

        private void ProcessInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                MovementInputReceived(Direction.Forward);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                MovementInputReceived(Direction.Backward);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                MovementInputReceived(Direction.Left);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                MovementInputReceived(Direction.Right);
            }
        }

        public void MovementInputReceived(Direction movementDirection)
        {
            if (movementDirection.GetOpposite() != _lastInput)
            {
                _lastInput = movementDirection;
            }
        }

        public Direction? ConsumeInput()
        {
            return _lastInput;
        }
    }
}