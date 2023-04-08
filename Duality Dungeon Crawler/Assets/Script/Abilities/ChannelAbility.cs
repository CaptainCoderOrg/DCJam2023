using System.Collections;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ChannelAbility", menuName = "BodyMind/Ability/ChannelAbility")]
public class ChannelAbility : AbilityDefinition
{
    public int YinRequirement;
    public int YangRequirement;
    
    public int YinYangChange = 0;
    public int SunMoonChange = 0;
    public int BodyMindChange = 0;
    public string ChannelMessage = "You begin to channel your energy...";
    public static WaitForSeconds _delay = new (.05f);
 
    public override IEnumerator OnUse(PlayerData player, Action OnFinish)
    {
        MessageController.Display(ChannelMessage);
        while (true)
        {
            yield return _delay;
            player.Stats.Stat(DualStat.YinYang).Value += YinYangChange;
            player.Stats.Stat(DualStat.BodyMind).Value += BodyMindChange;
            player.Stats.Stat(DualStat.SunMoon).Value += SunMoonChange;
            if(player.Stats.Stat(Stat.Yin) == 0) { break; }
            if(player.Stats.Stat(Stat.Yang) == 0) { break; }
            if(player.Stats.Stat(Stat.Moon) == 0) { break; }
            if(player.Stats.Stat(Stat.Sun) == 0) { break; }
            if(player.Stats.Stat(Stat.Body) == 0) { break; }
            if(player.Stats.Stat(Stat.Mind) == 0) { break; }
        }
        OnFinish();

    }
    public override bool CanCast(PlayerData player, out string message)
    {
        message = string.Empty;
        return true;
    }
}
