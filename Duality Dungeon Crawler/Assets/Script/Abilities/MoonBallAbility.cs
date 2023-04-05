using System.Collections;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Moon Ball Ability", menuName = "BodyMind/Ability/Moon Ball")]
public class MoonBallAbility : AbilityDefinition
{

    public override IEnumerator OnUse(PlayerData player, Action OnFinish)
    {
        MessageController.Display("You place your hands in front of you and focus...");
        yield return new WaitForSeconds(1f);
        MessageController.Display("The shade around you coalesces into a dense ball that falls to the ground.");
        player.Effects &= ~PlayerEffect.Shade;
        OnFinish();
        yield break;
    }

    public override bool CanCast(PlayerData player, out string message)
    { 
        if (!player.Effects.HasFlag(PlayerEffect.Shade))
        {
            message = "There is no Moon energy to harmonize.";
            return false;
        }
        message = string.Empty;
        return true;
    }

}
