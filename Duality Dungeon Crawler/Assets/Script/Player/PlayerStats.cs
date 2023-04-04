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

    internal void NotifyObservers()
    {
        foreach (PlayerStat stat in Stats)
        {
            stat.NotifyObservers();
        }
    }
}