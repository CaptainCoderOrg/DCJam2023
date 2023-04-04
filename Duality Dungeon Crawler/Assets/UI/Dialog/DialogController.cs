using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

[RequireComponent(typeof(UIDocument))]
public class DialogController : MonoBehaviour
{
    public static DialogController Instance { get; private set; }
    private VisualElement _root;
    private VisualElement _buttonContainer;
    private VisualElement _textContainer; // This is where you should put new labels
    private Label _dialogText; // This is the starting label
    private void Awake()
    {
        Instance = this;
        _root = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("DialogBox");
        Debug.Assert(_root != null, "Could not find dialog box.");
        _buttonContainer = _root.Q<VisualElement>("ButtonContainer");
        Debug.Assert(_buttonContainer != null, "Could not find button container.");
        _dialogText = _root.Q<Label>("DialogText");
        Debug.Assert(_dialogText != null, "Could not find dialog text.");
    }

    public bool IsVisible
    {
        get => _root.style.display.value == DisplayStyle.Flex;
        set
        {
            PlayerMovementController.Instance.ControlsEnabled = !value;
            _root.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }

    public void DisplayDialog(string message)
    {
        _dialogText.text = message;
    }

    public void SetOptions(params (string label, System.Action actions)[] actions)
    {
        var children = _buttonContainer.Children().ToArray();
        foreach (VisualElement child in children)
        {
            _buttonContainer.Remove(child);
        }
        foreach ((string label, System.Action action) in actions)
        {
            Button b = new (action) { text = label };
            _buttonContainer.Add(b);            
        }
    }
}

