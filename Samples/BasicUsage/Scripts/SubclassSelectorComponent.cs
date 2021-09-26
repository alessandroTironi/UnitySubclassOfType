using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TigerDev.SubclassOfType;

public class SubclassSelectorComponent : MonoBehaviour
{
    public SubclassOf<AComponent> componentSubclass = new SubclassOf<AComponent>();

    private void Start()
    {
        if (componentSubclass.SelectedType != null)
        {
            Debug.Log("Spawning a component of type " + componentSubclass.SelectedType?.ToString());
            gameObject.AddComponent(componentSubclass.SelectedType);
        }
    }
}
