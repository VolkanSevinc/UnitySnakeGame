using System;
using System.Collections.Generic;
using System.Linq;
using Interactions;
using Snake;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    public class InteractableObjectPicker : EditorWindow
    {
        private Vector2 _scrollPos;
        private Action<GameObject> _onObjectSelected;
        private List<GameObject> _interactablePrefabs; // Cache for interactable prefabs

        public static void Show(Action<GameObject> onObjectSelectedCallback)
        {
            InteractableObjectPicker window = GetWindow<InteractableObjectPicker>("Select Interactable Object");
            window._onObjectSelected = onObjectSelectedCallback;
            window.LoadInteractablePrefabs(); // Load prefabs when window is shown
            window.Show();
        }

        private void LoadInteractablePrefabs()
        {
            // Caching interactable prefabs
            _interactablePrefabs = AssetDatabase.FindAssets("t:Prefab")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<GameObject>)
                .Where(prefab => prefab is not null && prefab.GetComponent<InteractableItem>() is not null &&
                                 prefab.GetComponent<SnakeBodyPart>() is null)
                .ToList();
        }

        private void OnGUI()
        {
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            if (_interactablePrefabs is { Count: > 0 })
            {
                foreach (var prefab in _interactablePrefabs)
                {
                    DrawPrefabButton(prefab);
                }
            }
            else
            {
                EditorGUILayout.LabelField("No interactable objects found.");
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawPrefabButton(GameObject prefab)
        {
            if (GUILayout.Button(new GUIContent(prefab.name, AssetPreview.GetAssetPreview(prefab))))
            {
                _onObjectSelected?.Invoke(prefab);
                Close();
            }
        }
    }
}