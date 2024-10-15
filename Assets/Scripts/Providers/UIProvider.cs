using UI;
using UnityEngine;

namespace Providers
{
    public class UIProvider : MonoBehaviour
    {
        [SerializeField] private LevelChangeScreen nextLevelScreen;
        [SerializeField] private LevelChangeScreen failLevelScreen;
        [SerializeField] private LevelChangeScreen gameStartScreen;

        public LevelChangeScreen NextLevelScreen => nextLevelScreen;

        public LevelChangeScreen FailLevelScreen => failLevelScreen;

        public LevelChangeScreen GameStartScreen => gameStartScreen;
    }
}