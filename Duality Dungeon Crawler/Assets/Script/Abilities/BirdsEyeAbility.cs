using System.Collections;
using UnityEngine;
using System;
using CaptainCoder.Core;

[CreateAssetMenu(fileName = "Suns Eye Ability", menuName = "BodyMind/Ability/Birds Eye Ability")]
public class BirdsEyeAbility : AbilityDefinition
{
    public int SunCost = 5;
    public override IEnumerator OnUse(PlayerData player, Action OnFinish)
    {
        MessageController.Display("You close your eyes and focus on the Sol's Light...");
        yield return new WaitForSeconds(1f);
        MessageController.Display("The ball of light expands outward and you can feel the walls nearby.");
        player.Effects |= PlayerEffect.SunsEye;
        player.Stats.Stat(DualStat.SunMoon).Value -= 5;
        OnFinish();
        yield break;
    }

    public override bool CanCast(PlayerData player, out string message)
    { 
        if (!player.Effects.HasFlag(PlayerEffect.Light))
        {
            message = "You must first summon Sol's Light.";
            return false;
        }
        
        if (player.Stats.Stat(Stat.Sun) < SunCost)
        {
            message = "You do not have enough Moon energy.";
            return false;
        }
        message = string.Empty;
        return true;
    }

}
