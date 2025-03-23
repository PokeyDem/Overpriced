using UnityEngine;
using UnityEditor;
using NUnit.Framework;

[CustomPropertyDrawer(typeof(DailyNPCSpawns))]
public class DayScenarioDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        string indexStr = GetElementIndex(property);
        string displayName = $"DayScenario {indexStr}";

        EditorGUI.PropertyField(position, property, new GUIContent(displayName), true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    private string GetElementIndex(SerializedProperty property)
    {
        string path = property.propertyPath;
        int startIndex = path.LastIndexOf("[") + 1;
        int endIndex = path.LastIndexOf("]");

        if (startIndex >= 0 && endIndex > startIndex)
        {
            return path.Substring(startIndex, endIndex - startIndex);
        }
        return "?";
    }
}