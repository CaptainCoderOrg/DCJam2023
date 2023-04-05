using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CaptainCoder.Core;

[CreateAssetMenu(fileName = "LightAbility", menuName = "BodyMind/Ability/Light")]
public class LightAbility : AbilityDefinition
{

    private static readonly WaitForSeconds s_Delay = new (0.05f);
    // private static readonly WaitForSeconds s_StartDelay = new (2f);
    private Coroutine _coroutine;
    private bool _isCasting = false;
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
        OnFinish();
    }

    public override bool CanCast(PlayerStats stats, out string message)
    { 
        message = string.Empty;
        if(GameManager.Instance.PlayerStats.Stat(Stat.Sun) >= SunCost)
        {
            return true;
        }
        message = "You do not have enough Sun energy.";
        return false;
    }

}
