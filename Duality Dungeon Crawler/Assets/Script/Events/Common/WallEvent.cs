using CaptainCoder.Core;
using UnityEngine;

[CreateAssetMenu(fileName = "WallEvent", menuName = "BodyMind/Events/Wall Event")]
public class WallEvent : MapEvent
{
    [field: SerializeField]
    public Direction Facing { get; private set; }
    [field: SerializeField]
    public string Message { get; private set; }

    public override bool OnEnter()
    {
        MessageController.Display(Message);
        return false;
    }
}