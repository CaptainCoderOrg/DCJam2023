using System.Collections;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ShadeAbility", menuName = "BodyMind/Ability/Shade")]
public class ShadeAbility : AbilityDefinition
{

    private static readonly WaitForSeconds s_Delay = new (0.05f);
    [field: SerializeField]
    public int MoonCost { get; private set; } = 20;


    public override IEnumerator OnUse(PlayerData player, Action OnFinish)
    {
        MessageController.Display("You begin to concentrate...");
        for (int steps = 0; steps < MoonCost; steps++)
        {   
            if (player.Stats.Stat(Stat.Moon) < (MoonCost - steps))
            {
                MessageController.Display("You don't have enough Moon energy.");
                OnFinish();
                yield break;
            }         
            yield return s_Delay;
            player.Stats.Stat(DualStat.SunMoon).Value++;
        }
        MessageController.Display("A protective shade surrounds you.");
        GameManager.Instance.Player.Effects |= PlayerEffect.Shade;
        GameManager.Instance.Player.Effects &= ~PlayerEffect.Light;
        OnFinish();
    }

    public override bool CanCast(PlayerData player, out string message)
    { 
        var stats = player.Stats;
        message = string.Empty;
        if (player.Effects.HasFlag(PlayerEffect.Shade))
        {
            message = "You are already surrounded by a protective shade.";
            return false;
        }
        if(stats.Stat(Stat.Moon) >= MoonCost)
        {
            return true;
        }
        message = "You do not have enough Moon energy.";
        return false;
    }

}
