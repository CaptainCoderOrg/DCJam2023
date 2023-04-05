using UnityEngine;
using System;

[CreateAssetMenu(fileName = "TooDarkEvent", menuName = "BodyMind/Events/Yang/Too Dark")]
public class TooDarkEvent : MapEvent
{
    private PlayerData Player => GameManager.Instance.Player;

    public override bool OnEnter()
    {
        if (Player.Effects.HasFlag(PlayerEffect.Light)) { return false; }
        MessageController.Display("There is an overwhelming darkness in this area.");
        Player.Stats.Stat(DualStat.SunMoon).Value -= 10;
        return false;
    }

}