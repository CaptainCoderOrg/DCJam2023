using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CaptainCoder.Core;

[CreateAssetMenu(fileName = "Balance", menuName = "BodyMind/Ability/Balance")]
public class BalanceAbility : AbilityDefinition
{
    private static readonly WaitForSeconds s_Delay = new(0.05f);
    private static readonly WaitForSeconds s_StartDelay = new(2f);

    public override IEnumerator OnUse(PlayerData player, Action OnFinish)
    {
        MessageController.Display("You take a deep breath and focus inward.");
        yield return s_StartDelay;
        MessageController.Display("You energies begin to balance...");
        while (true)
        {
            yield return s_Delay;
            bool stop = true;
            foreach (PlayerStat stat in player.Stats.Stats)
            {
                if (stat.Value != 0)
                {
                    stop = false;
                    stat.Value -= (int)Mathf.Sign(stat.Value);
                }
            }
            if (stop) { break; }
        }
        MessageController.Display("You feel balanced.");
        OnFinish();
    }
    public override bool CanCast(PlayerStats stats, out string message)
    {
        message = string.Empty;
        foreach (PlayerStat stat in stats.Stats)
        {
            if (stat.Value != 0) { return true; }
        }
        message = "You feel balanced.";
        return false;
    }
}
