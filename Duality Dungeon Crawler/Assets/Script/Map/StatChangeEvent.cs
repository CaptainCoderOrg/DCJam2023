using CaptainCoder.Core;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "StatChangeEvent", menuName = "BodyMind/Events/Stat Change")]
public class StatChangeEvent : MapEvent
{
    public override bool OnInteract()
    {
        var diag = DialogController.Instance;
        diag.DisplayDialog("Before you are three fountains.\nA red fountain.\nA blue fountain.\nA green fountain.\nWhich do you drink?");
        (string, Action) red = ("Red Fountain", CreateAction(DualStat.BodyMind, 10));
        (string, Action) blue = ("Blue Fountain", CreateAction(DualStat.SunMoon, -10));
        (string, Action) green = ("Green Fountain", CreateAction(DualStat.YinYang, 15));
        diag.SetOptions(red, blue, green);
        diag.IsVisible = true;
        return true;
    }

    private Action CreateAction(DualStat stat, int change)
    {
        return () =>
        {
            GameManager.Instance.Player.Runes.AddRune(GameManager.Instance.Runes.Balance);
            GameManager.Instance.PlayerStats.Stat(stat).Value += change;
            DialogController.Instance.IsVisible = false;
            MessageController.Display("You feel your body grow!");

        };
    }
}