using UnityEngine;
using System;

[CreateAssetMenu(fileName = "EnterYinTemple", menuName = "BodyMind/Events/Yin/Enter Temple")]
public class EnterYinTempleEvent : MapEvent
{
    private PlayerData Player => GameManager.Instance.Player;
    private DialogController Dialog => DialogController.Instance;

    public override bool OnEnter()
    {
        if (Player.Effects.HasFlag(PlayerEffect.Shade)) 
        {
            MessageController.Display("There is a blinding light in this area. The shade spell protects you.");
            return false; 
        }
        Dialog.DisplayDialog(@"There is a blinding light in this area that is overwhelming. You cannot proceed.");
        Dialog.SetOptions(("Leave the Light", MovePlayer.ThenCloseDialog()));
        Dialog.IsVisible = true;
        return false;
    }

    private Action MovePlayer => () => 
    {
        PlayerMovementController.Instance.Position = (4, 8);
    };

}