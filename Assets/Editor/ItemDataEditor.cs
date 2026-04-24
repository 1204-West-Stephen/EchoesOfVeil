using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemData))]
public class ItemDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty prefab = serializedObject.FindProperty("prefab");
        SerializedProperty itemName = serializedObject.FindProperty("itemName");
        SerializedProperty puzzleNumber = serializedObject.FindProperty("puzzleNumber");
        SerializedProperty typeInput = serializedObject.FindProperty("typeInput");
        SerializedProperty sprite = serializedObject.FindProperty("sprite");
        SerializedProperty canBeInspected = serializedObject.FindProperty("canBeInspected");
        SerializedProperty itemInspectUI = serializedObject.FindProperty("itemInspectUI");
        SerializedProperty keyID = serializedObject.FindProperty("keyID");
        SerializedProperty tabletNumber = serializedObject.FindProperty("tabletNumber");
        SerializedProperty stoneValue = serializedObject.FindProperty("stoneValue");
        SerializedProperty scale = serializedObject.FindProperty("scale");
        SerializedProperty rotation = serializedObject.FindProperty("rotation");

        EditorGUILayout.PropertyField(itemName);
        EditorGUILayout.PropertyField(puzzleNumber);
        EditorGUILayout.PropertyField(typeInput);
        EditorGUILayout.PropertyField(sprite);
        EditorGUILayout.PropertyField(canBeInspected);
        EditorGUILayout.PropertyField(itemInspectUI);
        EditorGUILayout.PropertyField(scale);
        EditorGUILayout.PropertyField(rotation);
        EditorGUILayout.PropertyField(prefab);

        if ((InputType)typeInput.enumValueIndex == InputType.Key)
        {
            EditorGUILayout.PropertyField(keyID);
        }

        if ((InputType)typeInput.enumValueIndex == InputType.Tablet)
        {
            EditorGUILayout.PropertyField(tabletNumber);
        }

        if ((InputType)typeInput.enumValueIndex == InputType.NumberStone)
        {
            EditorGUILayout.PropertyField(stoneValue);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
