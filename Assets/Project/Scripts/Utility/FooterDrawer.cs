#if UNITY_EDITOR
using Project.Utility;
using UnityEditor;
using UnityEngine;

namespace Project.Editor
{
    [CustomPropertyDrawer(typeof(FooterAttribute))]
    public class FooterDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Добавляем высоту для текста и отступа
            var footerAttribute = (FooterAttribute)attribute;
            float footerHeight = string.IsNullOrEmpty(footerAttribute.Text) ? 0 : EditorGUIUtility.singleLineHeight + 5;
            return EditorGUI.GetPropertyHeight(property, label) + footerHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Рисуем поле
            EditorGUI.PropertyField(position, property, label, true);

            // Рисуем текст снизу
            var footerAttribute = (FooterAttribute)attribute;

            if (!string.IsNullOrEmpty(footerAttribute.Text))
            {
                Rect footerRect = new Rect(position.x, position.y + EditorGUI.GetPropertyHeight(property, label) + 2,
                    position.width, EditorGUIUtility.singleLineHeight);

                EditorGUI.LabelField(footerRect, footerAttribute.Text, EditorStyles.miniLabel);
            }
        }
    }
}
#endif