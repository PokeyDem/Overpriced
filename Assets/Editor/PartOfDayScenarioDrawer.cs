using UnityEngine;
using UnityEditor;
using static DayManager;
using System;

[CustomPropertyDrawer(typeof(InnerList<NPCGroupSpawn>))]
public class PartOfDayScenarioDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        int index = GetElementIndex(property);
        Array enumValues = Enum.GetValues(typeof(PartOfDay));
        string displayName="";
        if (Enum.IsDefined(typeof(PartOfDay), index))
        {
            displayName = $"{(PartOfDay)index} Scenario";
        }
        else
        {
            displayName = $"Element {index}";
        }


        EditorGUI.PropertyField(position, property, new GUIContent(displayName), true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    private int GetElementIndex(SerializedProperty property)
    {
        string path = property.propertyPath;
        int startIndex = path.LastIndexOf("[") + 1;
        int endIndex = path.LastIndexOf("]");

        if (startIndex >= 0 && endIndex > startIndex)
        {
            string indexStr = path.Substring(startIndex, endIndex - startIndex);
            if (int.TryParse(indexStr, out int index))
            {
                return index;
            }
        }
        return -1;
    }
}