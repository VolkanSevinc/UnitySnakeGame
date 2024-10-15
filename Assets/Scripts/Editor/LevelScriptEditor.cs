using System.Collections.Generic;
using Editors;
using Interactions;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using Util;
using Util.Enums;

namespace Editor
{
    [CustomEditor(typeof(LevelScriptableObject))]
    public class LevelScriptableObjectEditor : UnityEditor.Editor
    {
        private LevelScriptableObject _levelData;
        private List<InteractableItemWrapper> _interactableObjects;
        private const int GridCellSize = 60;
        private const int GridCellPadding = 5;
        private Vector2 _scrollPos;
        private bool _removeMode = false;
        private Vector2Int _selectedGridCoordinate; // Re-added declaration for selectedGridCoordinate

        private GUIStyle _coordinateStyle;

        private void OnEnable()
        {
            _levelData = (LevelScriptableObject)target;
            _interactableObjects = _levelData.InteractableObjects ?? new List<InteractableItemWrapper>();
            InitializeStyles();
            ClampSnakeToGrid();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();
            if (GUI.changed) ClampSnakeToGrid();
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Grid Editor", EditorStyles.boldLabel);
            DrawGridEditor();
            DrawButtons();
        }

        private void InitializeStyles()
        {
            _coordinateStyle = new GUIStyle
                { fontSize = 9, normal = { textColor = Color.white }, alignment = TextAnchor.UpperLeft };
        }

        private void ClampSnakeToGrid()
        {
            _levelData.SnakeLocation = new Vector2Int(
                Mathf.Clamp(_levelData.SnakeLocation.x, 0, _levelData.GridSize.x - 1),
                Mathf.Clamp(_levelData.SnakeLocation.y, 0, _levelData.GridSize.y - 1)
            );

            _levelData.SnakeSize = Mathf.Clamp(_levelData.SnakeSize, 1, GetMaxSnakeSize());
            EditorUtility.SetDirty(_levelData);
        }

        private int GetMaxSnakeSize()
        {
            var head = _levelData.SnakeLocation;
            var direction = _levelData.SnakeDirection;
            return direction switch
            {
                Direction.Forward => head.y + 1,
                Direction.Backward => _levelData.GridSize.y - head.y,
                Direction.Left => head.x + 1,
                Direction.Right => _levelData.GridSize.x - head.x,
                _ => 1,
            };
        }

        private void DrawButtons()
        {
            EditorGUILayout.BeginHorizontal();
            _removeMode = GUILayout.Toggle(_removeMode, "Remove Mode", "Button", GUILayout.Width(100),
                GUILayout.Height(25));
            if (GUILayout.Button("Clear All", GUILayout.Width(100), GUILayout.Height(25)))
            {
                _interactableObjects.Clear();
                _levelData.InteractableObjects = _interactableObjects; // Ensure changes are saved
                EditorUtility.SetDirty(_levelData); // Mark the scriptable object as dirty
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawGridEditor()
        {
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos,
                GUILayout.Width(EditorGUIUtility.currentViewWidth - 30),
                GUILayout.Height(450));
            DrawGrid();
            EditorGUILayout.EndScrollView();
        }

        private void DrawGrid()
        {
            if (_levelData.GridSize.x <= 0 || _levelData.GridSize.y <= 0)
            {
                EditorGUILayout.HelpBox("Grid size must be greater than 0.", MessageType.Warning);
                return;
            }

            for (int y = _levelData.GridSize.y - 1; y >= 0; y--)
            {
                EditorGUILayout.BeginHorizontal();
                for (int x = 0; x < _levelData.GridSize.x; x++)
                {
                    Vector2Int currentCoord = new Vector2Int(x, y);
                    DrawGridCell(currentCoord);
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawGridCell(Vector2Int coord)
        {
            GUI.backgroundColor = GetCellColor(coord);
            GUILayout.Space(GridCellPadding);
            Rect cellRect = GUILayoutUtility.GetRect(GridCellSize, GridCellSize);

            if (IsSnakePart(coord))
                GUI.Box(cellRect, GetSnakeContent(coord == _levelData.SnakeLocation));
            else if (GUI.Button(cellRect, GetButtonContent(coord)))
            {
                if (_removeMode) RemoveObjectAt(coord);
                else OnGridCellClicked(coord);
            }

            GUI.Label(new Rect(cellRect.x + 5, cellRect.y + 5, 30, 20), $"{coord.x},{coord.y}", _coordinateStyle);
            GUI.backgroundColor = Color.white;
        }

        private Color GetCellColor(Vector2Int coord)
        {
            if (IsSnakePart(coord)) return Color.yellow;
            if (_interactableObjects.Exists(entry => entry.Position == coord)) return Color.green;
            return Color.gray;
        }

        private bool IsSnakePart(Vector2Int coord)
        {
            if (coord == _levelData.SnakeLocation) return true;
            var head = _levelData.SnakeLocation;
            var direction = _levelData.SnakeDirection;
            for (int i = 1; i < _levelData.SnakeSize; i++)
                if (coord == head + GetOffsetForDirection(direction, i))
                    return true;
            return false;
        }

        private Vector2Int GetOffsetForDirection(Direction direction, int index)
        {
            return direction switch
            {
                Direction.Forward => new Vector2Int(0, -index),
                Direction.Backward => new Vector2Int(0, index),
                Direction.Left => new Vector2Int(-index, 0),
                Direction.Right => new Vector2Int(index, 0),
                _ => Vector2Int.zero,
            };
        }

        private GUIContent GetButtonContent(Vector2Int coord)
        {
            var entry = _interactableObjects.Find(e => e.Position == coord);
            return entry != null
                ? new GUIContent(AssetPreview.GetAssetPreview(entry.InteractableObject.gameObject),
                    entry.InteractableObject.name)
                : new GUIContent("Empty");
        }

        private GUIContent GetSnakeContent(bool isHead)
        {
            return new GUIContent(isHead ? "H" : "B", isHead ? "Snake Head" : "Snake Body");
        }

        private void OnGridCellClicked(Vector2Int coord)
        {
            _selectedGridCoordinate = coord;
            InteractableObjectPicker.Show(OnObjectSelected);
        }

        private void OnObjectSelected(GameObject selectedObject)
        {
            if (selectedObject == null) return;

            InteractableItem interactableItem = selectedObject.GetComponent<InteractableItem>();
            if (interactableItem == null)
            {
                Debug.LogWarning("Selected object does not have an InteractableObject component.");
                return;
            }

            _interactableObjects.Add(new InteractableItemWrapper
                { Position = _selectedGridCoordinate, InteractableObject = interactableItem });
            _levelData.InteractableObjects = _interactableObjects; // Assign back to the serialized field
            EditorUtility.SetDirty(_levelData); // Mark the scriptable object as dirty
        }

        private void RemoveObjectAt(Vector2Int coord)
        {
            var entryToRemove = _interactableObjects.Find(entry => entry.Position == coord);
            if (entryToRemove != null)
            {
                _interactableObjects.Remove(entryToRemove);
                _levelData.InteractableObjects = _interactableObjects; // Assign back to the serialized field
                EditorUtility.SetDirty(_levelData); // Mark the scriptable object as dirty
            }
        }
    }
}