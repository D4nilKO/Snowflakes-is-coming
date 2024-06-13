using Project.Components.Scripts.Level_System.LevelCreatingSystem;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelCreatingParameters))]
public class LevelCreatingParametersEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelCreatingParameters parametersEditor = (LevelCreatingParameters)target;
        if (GUILayout.Button("Calculate Result"))
        {
            Debug.Log("Max combinations: " + parametersEditor.GetMaxCombinationsCount().ToString());
        }
    }
}