using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemData))]
public class ItemDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty itemName = serializedObject.FindProperty("itemName");
        SerializedProperty puzzleNumber = serializedObject.FindProperty("puzzleNumber");
        SerializedProperty typeInput = serializedObject.FindProperty("typeInput");
        SerializedProperty sprite = serializedObject.FindProperty("sprite");
        SerializedProperty canBeInspected = serializedObject.FindProperty("canBeInspected");
        SerializedProperty itemInspectUI = serializedObject.FindProperty("itemInspectUI");
        SerializedProperty keyID = serializedObject.FindProperty("keyID");

        EditorGUILayout.PropertyField(itemName);
        EditorGUILayout.PropertyField(puzzleNumber);
        EditorGUILayout.PropertyField(typeInput);
        EditorGUILayout.PropertyField(sprite);
        EditorGUILayout.PropertyField(canBeInspected);
        EditorGUILayout.PropertyField(itemInspectUI);

        if ((InputType)typeInput.enumValueIndex == InputType.Key)
        {
            EditorGUILayout.PropertyField(keyID);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
