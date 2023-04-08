using System.Collections;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Iceball Ability", menuName = "BodyMind/Ability/Iceball")]
public class IceballAbility : AbilityDefinition
{
    public int MoonCost = 15;
    public int YangCost = 10;
    public ProjectileController Projectile;

    public override IEnumerator OnUse(PlayerData player, Action OnFinish)
    {
        player.Stats.Stat(DualStat.SunMoon).Value += MoonCost;
        player.Stats.Stat(DualStat.YinYang).Value += YangCost;
        MessageController.Display("You focus your energies into an Iceball.");
        ProjectileController projectile = Instantiate<ProjectileController>(Projectile);
        projectile.Direction = PlayerMovementController.Instance.Facing;
        GameObject map = PlayerMovementController.Instance.CurrentMap.gameObject;
        projectile.transform.SetParent(map.transform);
        projectile.transform.localPosition = PlayerMovementController.Instance.gameObject.transform.localPosition;
        OnFinish();
        yield break;
    }
    public override bool CanCast(PlayerData player, out string message)
    {
        if(!player.Effects.HasFlag(PlayerEffect.Shade))
        {
            message = "You must first summon Lun's Shade.";
            return false;
        }
        if(player.Stats.Stat(Stat.Moon) <= MoonCost)
        {
            message = "You do not have enough Moon energy.";
            return false;
        }
        if(player.Stats.Stat(Stat.Yang) <= YangCost)
        {
            message = "You do not have enough Yang energy.";
            return false;
        }
        message = string.Empty;
        return true;
    }
}
