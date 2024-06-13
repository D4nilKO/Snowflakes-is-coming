#if UNITY_EDITOR
using Project.Components.Scripts.Utility;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(FolderPathAttribute))]
public class FolderPathDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        FolderPathAttribute folderPathAttribute = (FolderPathAttribute)attribute;

        EditorGUI.BeginProperty(position, label, property);

        if (property.propertyType == SerializedPropertyType.String)
        {
            EditorGUI.BeginChangeCheck();
            string newValue = EditorGUI.TextField(position, label, property.stringValue);

            if (EditorGUI.EndChangeCheck())
            {
                property.stringValue = newValue;
            }

            if (GUI.Button(new Rect(position.xMax - 25f, position.y, 25f, position.height), "..."))
            {
                string selectedPath =
                    EditorUtility.OpenFolderPanel("Select Folder", folderPathAttribute.DefaultPath, string.Empty);

                if (string.IsNullOrEmpty(selectedPath) == false)
                {
                    property.stringValue = FileUtils.RemoveLocalPath(selectedPath);
                }
            }
        }
        else
        {
            EditorGUI.LabelField(position, label.text, "Use FolderPath with string.");
        }

        EditorGUI.EndProperty();
    }
}
#endif