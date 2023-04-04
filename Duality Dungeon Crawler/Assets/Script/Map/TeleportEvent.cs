using CaptainCoder.Core;
using UnityEngine;

[CreateAssetMenu(fileName = "TeleportEvent", menuName = "BodyMind/Events/Teleport")]
public class TeleportEvent : MapEvent
{
    [field: SerializeField]
    public Location Target { get; private set; }
    [field: SerializeField]
    public MutablePosition Position { get; private set; }
    [field: SerializeField]
    public Direction Facing { get; private set; }
    [field: SerializeField]
    public string Message { get; private set; }

    public override bool OnInteract()
    {
        var player = PlayerMovementController.Instance;
        player.CurrentMap = GameManager.Instance.GetMap(Target);
        player.Position = Position.Freeze();
        player.Facing = Facing;
        if (Message != null && Message != string.Empty)
        {
            Debug.Log(Message);
        }
        return true;
    }
}