using System.Collections;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "YinYangAbility", menuName = "BodyMind/Ability/YinYang")]
public class YinYangAbility : AbilityDefinition
{
    public int YangCost = 0;
    public int YinCost = 0;
    public string EnergyName;
    public PlayerEffect RequiredFlag;
    public bool isBodyTemple;
 
    public override IEnumerator OnUse(PlayerData player, Action OnFinish)
    {
        MessageController.Display($"You close your eyes and concentrate your {EnergyName} energy.");
        if (player.Effects.HasFlag(RequiredFlag))
        {
            MessageController.Display($"The portal is reacting to your energy!");
        }
        while (player.Stats.Stat(Stat.Yin) > YinCost*10 && player.Stats.Stat(Stat.Yang) > YangCost*10)
        {
            player.Stats.Stat(DualStat.YinYang).Value -= YinCost;
            player.Stats.Stat(DualStat.YinYang).Value += YangCost;
            yield return new WaitForSeconds(0.05f);
        }
        MessageController.Display($"You cannot focus any further.");

        if (player.Effects.HasFlag(RequiredFlag))
        {
            if (isBodyTemple)
            {
                DialogController.Instance.DisplayDialog("The portal is reacting to your Yin energy! It swirls rapidly.");
                Action Enter = () =>
                {
                    MessageController.Display($"You are surrounded by a warm light...");
                    PlayerMovementController.Instance.CurrentMap = GameManager.Instance.BodyMap;
                    PlayerMovementController.Instance.Position = (5, 1);
                    PlayerMovementController.Instance.Facing = Direction.East;
                    GameManager.Instance.Player.Effects &= ~PlayerEffect.OnSolPortal;
                };
                DialogController.Instance.SetOptions(
                    ("Enter Portal", Enter.ThenCloseDialog()),
                    ("Leave", () => {DialogController.Instance.IsVisible = false;})
                );
                DialogController.Instance.IsVisible = true;
                
            }
            OnFinish();
        }
        OnFinish();

    }
    public override bool CanCast(PlayerData player, out string message)
    {
        message = string.Empty;
        return true;
    }
}
