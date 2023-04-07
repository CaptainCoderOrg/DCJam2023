using UnityEngine;
using UnityEngine.UIElements;
using CaptainCoder.Audio;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(UIDocument))]
public class SpellBookController : MonoBehaviour
{

    private PlayerControls _controls;
    private UIDocument _document;

    public void Awake()
    {
        _document = GetComponent<UIDocument>();
        _document.enabled = true;
    }

    public void ToggleSpellBook()
    {
        DisplayStyle current = _document.rootVisualElement.style.display.value;
        _document.rootVisualElement.style.display = current == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None;
    }

    private void ToggleSpellBook(CallbackContext ctx) => ToggleSpellBook();

}