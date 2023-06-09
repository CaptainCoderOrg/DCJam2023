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
    public AudioClip TeleportSound;
    public PlayerEffect SetEffect;

    public override bool OnInteract()
    {
        SoundEffectController.PlaySFX(TeleportSound);
        var player = PlayerMovementController.Instance;
        player.CurrentMap = GameManager.Instance.GetMap(Target);
        player.Position = Position.Freeze();
        player.Facing = Facing;
        GameManager.Instance.Player.Effects &= ~SetEffect;
        if (Message != null && Message != string.Empty)
        {
            MessageController.Display(Message);
        }
        return true;
    }

    public override bool OnEnter()
    {
        GameManager.Instance.Player.Effects |= SetEffect;
        return false;
    }

    public override bool OnExit()
    {
        GameManager.Instance.Player.Effects &= ~SetEffect;
        return false;
    }

}