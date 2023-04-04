using CaptainCoder.Core;
using UnityEngine;

[CreateAssetMenu(fileName = "MessageEvent", menuName = "BodyMind/Events/Message")]
public class MessageEvent : MapEvent
{
    [field: SerializeField]
    public string Message { get; private set; }

    public override bool OnEnter()
    {
        MessageController.Display(Message);
        return false;
    }
}