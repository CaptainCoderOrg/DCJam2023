using System.Collections;
using UnityEngine;
using System;
using CaptainCoder.Core;

[CreateAssetMenu(fileName = "Float Ability", menuName = "BodyMind/Ability/Float Ability")]
public class FloatAbility : AbilityDefinition
{
    public const int FloatDuration = 20;
    public int FloatRemaining = 0;
    private bool _isRegistered = false;

    private void OnEnable()
    {
        FloatRemaining = 0;
        _isRegistered = false;
    }
    public override IEnumerator OnUse(PlayerData player, Action OnFinish)
    {
        MessageController.Display("You reach toward the ball of light and focus...");
        yield return new WaitForSeconds(1f);
        MessageController.Display("The air beneath your feet begins to warm and swirl, raising you into the air.");
        PlayerFloatController.Instance.OffsetY = 1.5f;
        PlayerFloatController.Instance.BobSpeed = 3f;        
        player.Effects &= ~PlayerEffect.Light;
        player.Effects |= PlayerEffect.Float;
        FloatRemaining = FloatDuration;
        if(!_isRegistered)
        {
            _isRegistered = true;
            PlayerMovementController.Instance.OnPositionChange += CheckFloat;
        }
        OnFinish();
        yield break;
    }

    private void CheckFloat(Position position)
    {
        PlayerData player = GameManager.Instance.Player;
        if (FloatRemaining == 1)
        {
            MessageController.Display("You return to the ground.");
            player.Effects &= ~PlayerEffect.Float;
        }
        else if (FloatRemaining == 5)
        {
            MessageController.Display("The air beneath you slows...");
            PlayerFloatController.Instance.OffsetY = .75f;
            PlayerFloatController.Instance.BobSpeed = 1f;        
        }
        FloatRemaining--;
    }

    public override bool CanCast(PlayerData player, out string message)
    { 
        if (!player.Effects.HasFlag(PlayerEffect.Light))
        {
            message = "You must first summon a Sol's Light.";
            return false;
        }
        message = string.Empty;
        return true;
    }

}
