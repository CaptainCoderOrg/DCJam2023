using System.Collections;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Fireball Ability", menuName = "BodyMind/Ability/Fireball")]
public class FireballAbility : AbilityDefinition
{
    public int SunCost = 10;
    public int YinCost = 15;
    public ProjectileController Projectile;

    public override IEnumerator OnUse(PlayerData player, Action OnFinish)
    {
        player.Stats.Stat(DualStat.SunMoon).Value -= SunCost;
        player.Stats.Stat(DualStat.YinYang).Value -= YinCost;
        MessageController.Display("You focus your energies into a Fireball.");
        ProjectileController projectile = Instantiate<ProjectileController>(Projectile);
        projectile.Direction = PlayerMovementController.Instance.Facing;
        projectile.IsFire = true;
        GameObject map = PlayerMovementController.Instance.CurrentMap.gameObject;
        projectile.transform.SetParent(map.transform);
        projectile.transform.localPosition = PlayerMovementController.Instance.gameObject.transform.localPosition;
        OnFinish();
        yield break;
    }
    public override bool CanCast(PlayerData player, out string message)
    {
        if(!player.Effects.HasFlag(PlayerEffect.Light))
        {
            message = "You must first summon Sol's Light.";
            return false;
        }
        if(player.Stats.Stat(Stat.Sun) < SunCost)
        {
            message = "You do not have enough Sun energy.";
            return false;
        }
        if(player.Stats.Stat(Stat.Yin) < YinCost)
        {
            message = "You do not have enough Yin energy.";
            return false;
        }
        message = string.Empty;
        return true;
    }
}
