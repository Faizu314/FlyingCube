﻿using UnityEditor;
using UnityEngine;

namespace Phezu.Derek.Editor {

    [CustomPropertyDrawer(typeof(HudButtonAttribute))]
    public class HudButtonAttributeDrawer : PropertyDrawer {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            if (InputManager.Instance == null) {
                Debug.Log("No InputManager instance present, only showing int value of HudButton.");
                property.intValue = EditorGUI.IntField(position, label, property.intValue);
                return;
            }
            if (property.type.CompareTo("int") != 0) {
                Debug.LogError("HudButton attribute should be used only for integers");
                return;
            }

            var buttons = InputManager.Instance.HudButtons;

            GUIContent[] options = new GUIContent[buttons.Length];

            for (int i = 0; i < options.Length; i++)
                options[i] = new(buttons[i]);

            EditorGUI.BeginProperty(position, label, property);

            property.intValue = EditorGUI.Popup(position, label, property.intValue, options);

            EditorGUI.EndProperty();
        }
    }
}