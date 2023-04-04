using CaptainCoder.Core;
using UnityEngine;

[CreateAssetMenu(fileName = "StatChangeEvent", menuName = "BodyMind/Events/Stat Change")]
public class StatChangeEvent : MapEvent
{
    public override bool OnInteract()
    {
        GameManager.Instance.PlayerStats.Stat(DualStat.BodyMind).Value += 10;
        GameManager.Instance.PlayerStats.Stat(DualStat.SunMoon).Value -= 17;
        GameManager.Instance.PlayerStats.Stat(DualStat.YinYang).Value += 7;
        MessageController.WriteLine("You feel your body grow!");
        return true;
    }
}