using CaptainCoder.Core;
using UnityEngine;

[CreateAssetMenu(fileName = "BarrierEvent", menuName = "BodyMind/Events/Barrier")]
public class BarrierEvent : MapEvent
{
    [field: SerializeField]
    public string Message { get; private set; }

    public override bool OnEnter()
    {
        MessageController.Display("Ouch! The magic barrier burns you.");
        GameManager.Instance.Hurt();
        GameManager.Instance.Player.Stats.Stat(DualStat.BodyMind).Value -= 10;
        GameManager.Instance.Player.Stats.Stat(DualStat.SunMoon).Value += 15;
        GameManager.Instance.Player.Stats.Stat(DualStat.YinYang).Value += 5;
        SoundEffectController.PlaySFX(GameManager.Instance.SoundEffects.Hurt);        
        return true;
    }
}