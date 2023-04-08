using System.Collections;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "FinalPortal", menuName = "BodyMind/Ability/Final Portal")]
public class FinalPortalAbility : AbilityDefinition
{
    public int Change = 0;
    public int Check = 0;
    public WhereTo Destination;
    
    public enum WhereTo
    {
        Sol, Lun, Death
    }


    
    public static WaitForSeconds _delay = new (.05f);
 
    public override IEnumerator OnUse(PlayerData player, Action OnFinish)
    {
        MessageController.Display("You begin to channel your energy...");
        while (CheckStats(player))
        {
            if (player.Stats.Stat(DualStat.SunMoon).Value != Check) 
            { 
                player.Stats.Stat(DualStat.SunMoon).Value += Change;
            }
            if (player.Stats.Stat(DualStat.YinYang).Value != Check) 
            { 
                player.Stats.Stat(DualStat.YinYang).Value += Change;
            }
            if (player.Stats.Stat(DualStat.BodyMind).Value != Check) 
            { 
                player.Stats.Stat(DualStat.BodyMind).Value += Check;
            }
            yield return _delay;
        }
        DialogChain.Dialog("A portal opens before you and you enter.")
        .OnFinish(Teleport)
        .Display();
        OnFinish();

    }

    public void Teleport()
    {
        if (Destination== WhereTo.Sol)
        {
            PlayerMovementController.Instance.CurrentMap = GameManager.Instance.SolFinal;
            PlayerMovementController.Instance.Position = (3, 3);
            PlayerMovementController.Instance.Facing = Direction.North;
        }
        if (Destination== WhereTo.Lun)
        {
            PlayerMovementController.Instance.CurrentMap = GameManager.Instance.LunFinal;
            PlayerMovementController.Instance.Position = (3, 3);
            PlayerMovementController.Instance.Facing = Direction.North;
        }
    }

    public bool CheckStats(PlayerData player)
    {
        if (player.Stats.Stat(DualStat.SunMoon).Value != Check) { return true; }
        if (player.Stats.Stat(DualStat.YinYang).Value != Check) { return true; }
        if (player.Stats.Stat(DualStat.BodyMind).Value != Check) { return true; }
        return false;
    }

    public override bool CanCast(PlayerData player, out string message)
    {
        message = string.Empty;
        return true;
    }
}
