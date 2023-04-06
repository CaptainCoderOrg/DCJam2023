using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class InteractController : MonoBehaviour
{
    public void Awake()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        root.RegisterCallback<PointerDownEvent>(Interact);
    }

    private void Interact(PointerDownEvent evt)
    {
        // Debug.Log("Clicked");
    }

}