using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class InteractController : MonoBehaviour
{
    Button _interactButton;
    public void Awake()
    {
        _interactButton = GetComponent<UIDocument>().rootVisualElement.Q<Button>("InteractButton");
        Debug.Assert(_interactButton != null, "Interact button was null.");
        _interactButton.clicked += Interact;
    }

    private void Interact()
    {
        PlayerMovementController.Instance.Interact();
    }

}