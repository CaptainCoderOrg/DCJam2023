using System.Collections;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "LightAbility", menuName = "BodyMind/Ability/Light")]
public class LightAbility : AbilityDefinition
{

    private static readonly WaitForSeconds s_Delay = new (0.05f);
    [field: SerializeField]
    public int SunCost { get; private set; } = 20;


    public override IEnumerator OnUse(PlayerData player, Action OnFinish)
    {
        MessageController.Display("You begin to concentrate...");
        for (int steps = 0; steps < SunCost; steps++)
        {   
            if (player.Stats.Stat(Stat.Sun) < (SunCost - steps))
            {
                MessageController.Display("You don't have enough Sun energy.");
                OnFinish();
                yield break;
            }         
            yield return s_Delay;
            player.Stats.Stat(DualStat.SunMoon).Value--;
        }
        MessageController.Display("A ball of light is hovering with you.");
        GameManager.Instance.Player.Effects |= PlayerEffect.Light;
        GameManager.Instance.Player.Effects &= ~PlayerEffect.Shade;
        OnFinish();
    }

    public override bool CanCast(PlayerData player, out string message)
    { 
        var stats = player.Stats;
        message = string.Empty;
        if (player.Effects.HasFlag(PlayerEffect.Light))
        {
            message = "A ball of light is already following you.";
            return false;
        }
        if(stats.Stat(Stat.Sun) >= SunCost)
        {
            return true;
        }
        message = "You do not have enough Sun energy.";
        return false;
    }

}
