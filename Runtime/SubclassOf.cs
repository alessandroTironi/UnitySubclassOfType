using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TigerDev.SubclassOfType
{
    [Serializable]
    public class SubclassOf<T>
    {
        public Type SelectedType
        {
            get
            {
                // Always better to make an additional check before returning the type,
                // because Unity serialization system does not match well with generic types.
                // As a result of that, changing the T parameter of a SubclassOf property
                // without renaming it would prevent the property to be correctly refreshed
                // and this would return the type selected before the change. 

                Type selectedType = Type.GetType(selectedTypeQualifiedName);
                if (selectedType == null || !selectedType.IsSubclassOf(typeof(T)) && selectedType != typeof(T))
                {
                    // fallback on the null type
                    parentTypeQualifiedName = typeof(T).AssemblyQualifiedName;
                    selectedTypeQualifiedName = "Null";
                    selectedTypeName = "None";
                    return null;
                }

                return selectedType; 
            }

            set
            {
                
                if (value != null)
                {
                    if (!value.IsSubclassOf(typeof(T)))
                    {
                        // Log error and set to null if the type is not compatible.
                        Debug.LogError(string.Format("Type {0} is not child of {1}",
                            value.ToString(), typeof(T).ToString()));
                        selectedTypeQualifiedName = "Null";
                        selectedTypeName = "None";
                    }
                    else
                    {
                        selectedTypeQualifiedName = value.AssemblyQualifiedName;
                        selectedTypeName = value.ToString();
                    }
                }
                else
                {
                    selectedTypeQualifiedName = "Null";
                    selectedTypeName = "None";
                }
            }
        }

        /// <summary>
        /// Short name of the currently selected type.
        /// </summary>
        [SerializeField]
        [HideInInspector]
        protected internal string selectedTypeName = "None";

        public string SelectedTypeName { get { return selectedTypeName; } }

        /// <summary>
        /// Assembly Qualified name of the currently selected type.
        /// </summary>
        [SerializeField]
        [HideInInspector]
        protected internal string selectedTypeQualifiedName = "Null";
        public string SelectedTypeAssemblyQualifiedName { get { return selectedTypeQualifiedName; } }

        /// <summary>
        /// Assembly Qualified name of the T type.
        /// </summary>
        [SerializeField]
        [HideInInspector]
        protected internal string parentTypeQualifiedName;
        public string ParentTypeAssemblyQualifiedName { get { return parentTypeQualifiedName; } }

        public override string ToString()
        {
            return string.Format("Type {0} (parent: {1})", selectedTypeName, parentTypeQualifiedName);
        }

        public SubclassOf()
        {
            parentTypeQualifiedName = typeof(T).AssemblyQualifiedName;
            selectedTypeQualifiedName = "Null";
            if (SelectedType != null)
            {
                selectedTypeName = SelectedType.ToString();
            }
            else
            {
                selectedTypeName = "None";
            }
        }

        /// <summary>
        /// Explicit type selection constructor.
        /// </summary>
        /// <param name="selectedType"></param>
        public SubclassOf(Type selectedType)
        {
            parentTypeQualifiedName = typeof(T).AssemblyQualifiedName;

            if (selectedType == null)
            {
                selectedTypeQualifiedName = "Null";
                selectedTypeName = "None";
            }
            else
            {
                if (selectedType.IsSubclassOf(typeof(T)))
                {
                    selectedTypeQualifiedName = selectedType.AssemblyQualifiedName;
                    selectedTypeName = selectedType.ToString();
                }
                else
                {
                    Debug.LogError("The provided type is not subclass of " + typeof(T).ToString());
                }
            }    
        }

        public override bool Equals(object obj)
        {
            SubclassOf<T> other = (SubclassOf<T>)obj;
            return other != null && SelectedType == other.SelectedType;
        }

        public override int GetHashCode()
        {
            string uniqueStringID = string.Format("SubclassOf<{0}>: {1}",
                parentTypeQualifiedName, selectedTypeQualifiedName);
            return uniqueStringID.GetHashCode();
        }
    }

}