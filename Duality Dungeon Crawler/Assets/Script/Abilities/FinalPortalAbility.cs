using System.Collections;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "FinalPortal", menuName = "BodyMind/Ability/Final Portal")]
public class FinalPortalAbility : AbilityDefinition
{
    public int SunMoonChange = 0;
    public int YingYangChange = 0;
    public int BodyMindChange = 0;
    public int SunMoonCheck = 0;
    public int YinYangCheck = 0;
    public int BodyMindCheck = 0;
    public WhereTo Destination;

    public enum WhereTo
    {
        Sol, Lun, Death
    }



    public static WaitForSeconds _delay = new(.05f);

    public override IEnumerator OnUse(PlayerData player, Action OnFinish)
    {
        
        Teleport();
        OnFinish();
        yield break;

    }

    public void Teleport()
    {
        if (Destination == WhereTo.Sol)
        {
            PlayerMovementController.Instance.CurrentMap = GameManager.Instance.SolFinal;
            PlayerMovementController.Instance.Position = (3, 3);
            PlayerMovementController.Instance.Facing = Direction.North;
        }
        if (Destination == WhereTo.Lun)
        {
            PlayerMovementController.Instance.CurrentMap = GameManager.Instance.LunFinal;
            PlayerMovementController.Instance.Position = (3, 3);
            PlayerMovementController.Instance.Facing = Direction.North;
        }
    }

    public bool CheckStats(PlayerData player)
    {
        if (player.Stats.Stat(DualStat.SunMoon).Value != SunMoonCheck) { return true; }
        if (player.Stats.Stat(DualStat.YinYang).Value != YinYangCheck) { return true; }
        if (player.Stats.Stat(DualStat.BodyMind).Value != BodyMindChange) { return true; }
        return false;
    }

    public override bool CanCast(PlayerData player, out string message)
    {
        if (PlayerMovementController.Instance.CurrentMap == GameManager.Instance.LunFinal ||
            PlayerMovementController.Instance.CurrentMap == GameManager.Instance.SolFinal)
        {
            message = "The energies of this place prevent you from casting that spell.";
            return false;
        }
        message = string.Empty;
        return true;
    }
}
