using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MessageController : MonoBehaviour
{
    public static WaitForSeconds CharDelay = new (0.0025f);
    public static MessageController Instance { get; private set; }
    private VisualElement _root;
    private ScrollView _messageContainer;
    private static VisualElement _last;


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
        Label label = new ();
        _last = label;
        label.AddToClassList("message-label");
        Instance._messageContainer.Add(label);
        GameManager.Instance.StartCoroutine(PrintMessage(message, label));
    }

    private static IEnumerator PrintMessage(string message, Label label)
    {
        Queue<char> queue = new (message.ToCharArray());
        while (queue.Count > 0)
        {
            label.text += queue.Dequeue();
            yield return CharDelay;
            ScrollLater();
        }
        GameManager.Instance.StartCoroutine(FadeLabel(label));
    }

    private static IEnumerator FadeLabel(Label label)
    {
        yield return new WaitForSeconds(10f);
        Debug.Log($"Opacity: {label.style.opacity.value}");
        label.style.opacity = 1;
        while (label.style.opacity.value > 0)
        {
            label.style.opacity = label.style.opacity.value - 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
        label.parent.Remove(label);
    }

    private static void ScrollLater()
    {
        Instance._messageContainer.ScrollTo(_last);
    }

}