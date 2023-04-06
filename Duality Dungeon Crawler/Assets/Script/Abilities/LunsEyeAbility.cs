using System.Collections;
using UnityEngine;
using System;
using CaptainCoder.Core;

[CreateAssetMenu(fileName = "Luns Eye Ability", menuName = "BodyMind/Ability/Luns Eye Ability")]
public class LunsEyeAbility : AbilityDefinition
{
    public int MoonCost = 5;
    public override IEnumerator OnUse(PlayerData player, Action OnFinish)
    {
        MessageController.Display("You close your eyes and focus on the Lun's Shade...");
        yield return new WaitForSeconds(1f);
        MessageController.Display("The shade expands outward and you can feel the walls nearby.");
        player.Effects |= PlayerEffect.SunsEye;
        player.Stats.Stat(DualStat.SunMoon).Value += 5;
        OnFinish();
        yield break;
    }

    public override bool CanCast(PlayerData player, out string message)
    { 
        if (!player.Effects.HasFlag(PlayerEffect.Shade))
        {
            message = "You must first summon Lun's Shade.";
            return false;
        }
        if (player.Stats.Stat(Stat.Moon) < MoonCost)
        {
            message = "You do not have enough Moon energy.";
            return false;
        }
        message = string.Empty;
        return true;
    }

}
