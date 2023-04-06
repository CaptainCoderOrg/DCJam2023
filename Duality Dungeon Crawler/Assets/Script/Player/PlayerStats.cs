using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStat", menuName = "BodyMind/Player Stat")]
public class PlayerStats : ScriptableObject
{
    [NaughtyAttributes.OnValueChanged("NotifyObservers")]
    public List<PlayerStat> Stats;

    public PlayerStat Stat(DualStat stat)
    {
        foreach(PlayerStat playerStat in Stats)
        {
            if (stat == playerStat.Stat) { return playerStat; }
        }
        throw new System.ArgumentException($"Could not find stat {stat}.");
    }

    public int Stat(Stat stat)
    {
        foreach(PlayerStat playerStat in Stats)
        {
            // if (stat == playerStat.Stat) { return playerStat; }
            if (stat == playerStat.Parts.Left) { return playerStat.PartValue(stat); }
            if (stat == playerStat.Parts.Right) { return playerStat.PartValue(stat); }
        }
        throw new System.ArgumentException($"Could not find stat {stat}.");
    }

    [NaughtyAttributes.Button("Reset Data")]
    public void ResetData()
    {
        int i = 0;
        foreach (PlayerStat playerStat in Stats)
        {
            playerStat.Value = i++ % 2 == 0 ? -90 : 90;
        }    
    }


    internal void NotifyObservers()
    {
        foreach (PlayerStat stat in Stats)
        {
            stat.NotifyObservers();
        }
    }
}
