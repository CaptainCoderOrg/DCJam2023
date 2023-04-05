using UnityEngine;
using UnityEngine.UIElements;
using CaptainCoder.Audio;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(UIDocument))]
public class OptionsButtonController : MonoBehaviour
{
    public OptionsMenuController OptionsMenu;
    private UIDocument _document;
    
    

    public void Awake()
    {
        _document = GetComponent<UIDocument>();
        Button optionsButton = _document.rootVisualElement.Q<Button>("OptionsButton");
        optionsButton.clicked += OptionsMenu.ToggleMenu;

    }

}