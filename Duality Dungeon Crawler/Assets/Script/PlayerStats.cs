using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStat", menuName = "Player Stat")]
public class PlayerStats : ScriptableObject
{
    public List<PlayerStat> Stats;

    public PlayerStat Stat(DualStat stat)
    {
        foreach(PlayerStat playerStat in Stats)
        {
            if (stat == playerStat.Stat) { return playerStat; }
        }
        throw new System.ArgumentException($"Could not find stat {stat}.");
    }

}
