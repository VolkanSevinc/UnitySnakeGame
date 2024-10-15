using System;
using System.Collections.Generic;
using Interactions;
using TMPro;
using UnityEngine;

namespace GridBase
{
    public class GridCell : MonoBehaviour
    {
        [SerializeField] private TextMeshPro textMesh;
        [SerializeField] private float gridYOffset;

        public Vector2Int GridPosition { get; set; }
        public GameObject CellModel { get; set; }
        public List<InteractableItem> GridItems = new List<InteractableItem>();

        public float GridYOffset => gridYOffset;

        private void Awake()
        {
            textMesh.gameObject.SetActive(Application.platform == RuntimePlatform.WindowsEditor);
        }

        public void SetGridPosition(int x, int y)
        {
            GridPosition = new Vector2Int(x, y);
            textMesh.SetText(x + ", " + y);
        }
    }
}