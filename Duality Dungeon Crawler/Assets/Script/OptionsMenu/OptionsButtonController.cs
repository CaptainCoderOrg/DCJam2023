using UnityEngine;
using UnityEngine.UIElements;
using CaptainCoder.Audio;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(UIDocument))]
public class OptionsButtonController : MonoBehaviour
{
    public OptionsMenuController OptionsMenu;
    public SpellBookController SpellBook;
    private UIDocument _document;

    public void Awake()
    {
        _document = GetComponent<UIDocument>();
        RegisterButton("OptionsButton", "OptionsLabel", OptionsMenu.ToggleMenu);
        RegisterButton("SpellsButton", "SpellsLabel", SpellBook.ToggleSpellBook);
    }

    public void RegisterButton(string buttonName, string labelName, System.Action action)
    {
        Button button = _document.rootVisualElement.Q<Button>(buttonName);
        button.clicked += action;
        Label label = _document.rootVisualElement.Q<Label>(labelName);
        label.style.display = DisplayStyle.None;
        button.RegisterCallback<PointerEnterEvent>((_) => label.style.display = DisplayStyle.Flex);
        button.RegisterCallback<PointerLeaveEvent>((_) => label.style.display = DisplayStyle.None);
    }
}