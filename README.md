# Unreal-like SubclassOf<T> type implementation in Unity
  
This is a Unity implementation of the Unreal Engine TSubclassOf<T> type, which allows the engine user to select any class defined as child of a type T from a friendly drop-down menu. Documentation on Unreal's TSubclassOf [here](https://docs.unrealengine.com/4.27/en-US/ProgrammingAndScripting/ProgrammingWithCPP/UnrealArchitecture/TSubclassOf/).
  
## Usage
Just expose a public SubclassOf property and initialize it with the default constructor. Unity will render it with a drop-down selector of all AComponent's subclasses.
  
```csharp
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
```
  
![In-engine result](doc/UnitySubclassOfExample.png?raw=true "In-Engine result")
  
## Known issues 
* At the moment, the initialization with the default constructor (e.g. ```public SubclassOf<AComponent> subclass = new SubclassOf<AComponent>();```) is mandatory. I am currently looking for a way to make it automatic.
* The dropdown list gets rebuilded multiple times per frame. It is not game time code, but it would be nice to optimize that work anyway.
