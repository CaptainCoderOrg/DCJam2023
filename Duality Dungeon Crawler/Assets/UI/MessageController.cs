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
    }

    // public void DisplayDialog(string message)
    // {
    //     _dialogText.text = string.Empty;
    //     _messageQueue.Clear();
    //     message.ToList().ForEach(_messageQueue.Enqueue);
    //     StartDisplayMessage();
    // }

    // private void StartDisplayMessage()
    // {
    //     if (_messagePrinter != null)
    //     {
    //         StopCoroutine(_messagePrinter);
    //     }
    //     _messagePrinter = StartCoroutine(DisplayMessage());
    // }

    // private IEnumerator DisplayMessage()
    // {
    //     while (_messageQueue.Count > 0)
    //     {
    //         _dialogText.text += _messageQueue.Dequeue();
    //         yield return new WaitForSeconds(CharDelay);
    //     }
    // }

    private static void ScrollLater()
    {
        Instance._messageContainer.ScrollTo(_last);
    }

}