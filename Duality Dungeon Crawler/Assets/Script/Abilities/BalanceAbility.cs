using System.Collections;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Balance", menuName = "BodyMind/Ability/Balance")]
public class BalanceAbility : AbilityDefinition
{
    private static readonly WaitForSeconds s_Delay = new(0.05f);
    private static readonly WaitForSeconds s_StartDelay = new(2f);

    public override IEnumerator OnUse(PlayerData player, Action OnFinish)
    {
        MessageController.Display("You take a deep breath and focus inward.");
        yield return s_StartDelay;
        GameManager.Instance.Player.Effects = 0;
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
    public override bool CanCast(PlayerData player, out string message)
    {
        var stats = player.Stats;
        message = string.Empty;
        foreach (PlayerStat stat in stats.Stats)
        {
            if (stat.Value != 0) { return true; }
        }
        message = "You feel balanced.";
        return false;
    }
}
