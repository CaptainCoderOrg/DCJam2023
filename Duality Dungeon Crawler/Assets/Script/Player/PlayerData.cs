using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "BodyMind/Player Data")]
public class PlayerData : ScriptableObject
{
    [NaughtyAttributes.OnValueChanged("NotifyObservers")]
    public PlayerStats Stats;
    [NaughtyAttributes.OnValueChanged("NotifyObservers")]
    public PlayerAbilities Abilities;
    [NaughtyAttributes.OnValueChanged("NotifyObservers")]
    public PlayerRunes Runes;
    [NaughtyAttributes.OnValueChanged("NotifyObservers")]
    public PlayerEffect Effects;

    [Header("Drag a Player Data here to copy values.")]
    public PlayerData SetValues;

    public void NotifyObservers()
    {
        Stats?.NotifyObservers();
        Abilities?.NotifyObservers();
        Runes?.NotifyObservers();
    }

    [NaughtyAttributes.Button("Reset Data")]
    public void ResetData()
    {
        Abilities.ResetData();
        Runes.ResetData();
        Stats.ResetData();
    }

    

    public void OnValidate()
    {
        if (SetValues != null)
        {
            Stats = SetValues.Stats;
            Abilities = SetValues.Abilities;
            Runes = SetValues.Runes;
            SetValues = null;            
        }
    }
}
