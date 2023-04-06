using System.Collections;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Balance", menuName = "BodyMind/Ability/Balance")]
public class BalanceAbility : AbilityDefinition
{
    private static readonly WaitForSeconds s_Delay = new(0.05f);
    private static readonly WaitForSeconds s_StartDelay = new(1f);

    public override IEnumerator OnUse(PlayerData player, Action OnFinish)
    {
        MessageController.Display("You take a deep breath and focus.");
        yield return s_StartDelay;
        GameManager.Instance.Player.Effects = 0;
        MessageController.Display("Your energies begin to harmonize...");
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
        MessageController.Display("You feel in harmony.");
        OnFinish();
    }
    public override bool CanCast(PlayerData player, out string message)
    {
        if(PlayerMovementController.Instance.CurrentMap != GameManager.Instance.EntranceMap)
        {
            message = "It is too dangerous to harmonize here.";
            return false;
        }
        var stats = player.Stats;
        message = string.Empty;
        foreach (PlayerStat stat in stats.Stats)
        {
            if (stat.Value != 0) { return true; }
        }
        message = "You feel in harmony.";
        player.Effects = 0;
        return false;
    }
}
