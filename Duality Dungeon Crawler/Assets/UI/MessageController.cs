using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MessageController : MonoBehaviour
{
    public static MessageController Instance { get; private set; }
    private VisualElement _root;
    private ScrollView _messageContainer;


    public void Awake()
    {
        Instance = this;
        _root = GetComponent<UIDocument>().rootVisualElement;
        _messageContainer = _root.Q<ScrollView>("MessageContainer");
        Debug.Assert(_messageContainer != null, "Could not locate message container.");
    }

    public static void Display(string message)
    {
        // Debug.Log(message);
        Label label = new (message);
        label.AddToClassList("message-label");
        Instance._messageContainer.Add(label);
        Instance.StartCoroutine(ScrollLater(label));
    }

    private static IEnumerator ScrollLater(Label label)
    {
        yield return new WaitForEndOfFrame();
        Instance._messageContainer.ScrollTo(label);
    }

}