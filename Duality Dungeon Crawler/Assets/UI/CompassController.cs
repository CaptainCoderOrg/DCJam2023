using System;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class CompassController : MonoBehaviour
{
    private VisualElement _root;
    private Label _compassLabel;


    public void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _compassLabel = _root.Q<Label>("CompassLabel");
        Debug.Assert(_compassLabel != null, "Compass label was null");
    }

    public void Start()
    {
        PlayerMovementController.Instance.OnDirectionChange += UpdateLabel;
    }

    private void UpdateLabel(Direction newDirection)
    {
        _compassLabel.text = newDirection.ToString();
    }

}