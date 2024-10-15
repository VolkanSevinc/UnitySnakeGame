using System;
using UnityEngine;

namespace Managers
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance;

        [SerializeField] private Camera currentCamera;
        [SerializeField] private float cameraOffset;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public void Initialize(GridBounds gridBounds)
        {
            var gridCenter = (gridBounds.P0 + gridBounds.P2) / 2f;

            var gridDiagonal02 = gridBounds.P0 - gridBounds.P2;
            var gridDiagonal13 = gridBounds.P1 - gridBounds.P3;

            var desiredFrustumRectWidth = Mathf.Max(Mathf.Abs(Vector3.Dot(gridDiagonal02, transform.right)),
                Mathf.Abs(Vector3.Dot(gridDiagonal13, transform.right)));
            var desiredFrustumRectHeight = Mathf.Max(Mathf.Abs(Vector3.Dot(gridDiagonal02, transform.up)),
                Mathf.Abs(Vector3.Dot(gridDiagonal13, transform.up)));

            currentCamera.orthographicSize =
                Mathf.Max(desiredFrustumRectWidth / currentCamera.aspect, desiredFrustumRectHeight) / 2f
                + cameraOffset;

            currentCamera.transform.position = gridCenter - (new Vector3(1, -0.66f, 1) * 100);
        }
    }
}