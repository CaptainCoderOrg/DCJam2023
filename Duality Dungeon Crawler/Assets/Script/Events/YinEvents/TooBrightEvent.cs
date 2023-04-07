using UnityEngine;
using System;

[CreateAssetMenu(fileName = "TooBrightEvent", menuName = "BodyMind/Events/Yin/Too Bright")]
public class TooBrightEvent : MapEvent
{
    private PlayerData Player => GameManager.Instance.Player;

    public override bool OnEnter()
    {
        if (Player.Effects.HasFlag(PlayerEffect.Shade)) { return false; }
        MessageController.Display("There is a blinding light in this area that is overwhelming.");
        Player.Stats.Stat(DualStat.SunMoon).Value += 10;        
        GameManager.Instance.PainFlashController.ShowPain(Color.black, Color.white);
        return false;
    }

}