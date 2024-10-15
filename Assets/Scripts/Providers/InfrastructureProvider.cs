using UnityEngine;

namespace Providers
{
    public class InfrastructureProvider : MonoBehaviour
    {
        public static InfrastructureProvider Instance { get; private set; }

        [SerializeField] private UIProvider uiProvider;
        [SerializeField] private PrefabProvider prefabProvider;

        public PrefabProvider PrefabProvider => prefabProvider;
        public UIProvider UIProvider => uiProvider;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
    }
}