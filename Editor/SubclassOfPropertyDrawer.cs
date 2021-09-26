using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using UnityEngine.UIElements;

namespace TigerDev.SubclassOfType
{
    [CustomPropertyDrawer(typeof(SubclassOf<>))]
    public class SubclassOfPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Find useful Type objects
            Type selectedType = 
                Type.GetType(property.FindPropertyRelative("selectedTypeQualifiedName").stringValue);
            Type parentType = fieldInfo.FieldType.GetGenericArguments()[0];
            if (selectedType == null || selectedType != parentType && !selectedType.IsSubclassOf(parentType))
            {
                property.FindPropertyRelative("parentTypeQualifiedName").stringValue =
                    parentType.AssemblyQualifiedName;
                selectedType = null;
            }

            // Compute list of all the subtypes of the required parent.
            List<Type> allTypes = GetAllSubclassesOf(parentType);

            // Build string options array.
            string[] stringOptions = new string[allTypes.Count];
            stringOptions[0] = "None";
            int selectedIndex = 0;
            for (int i = 1; i < allTypes.Count; ++i)
            {
                if (allTypes[i] == selectedType)
                {
                    selectedIndex = i;
                }

                stringOptions[i] = allTypes[i].ToString();
            }

            // Build dropdown (Popup) menu
            selectedIndex = EditorGUI.Popup(position, property.displayName, selectedIndex, stringOptions);
            Type newSelectedType = allTypes[selectedIndex];

            // Update string values. The actual Type object can be retrieved with propertyName.SelectedType.
            property.FindPropertyRelative("selectedTypeQualifiedName").stringValue =
                newSelectedType != null? newSelectedType.AssemblyQualifiedName : "Null";
            property.FindPropertyRelative("selectedTypeName").stringValue = newSelectedType?.ToString();

            EditorGUI.EndProperty();
        }

        private List<Type> GetAllSubclassesOf(Type parentType)
        {
            List<Type> subclasses = new List<Type>();
            subclasses.Add(null);
            subclasses.Add(parentType);
            foreach (System.Reflection.Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(parentType))
                        subclasses.Add(type);
                }
            }

            return subclasses;
        }
    }
}
