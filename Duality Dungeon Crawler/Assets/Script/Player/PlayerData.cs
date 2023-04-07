using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "BodyMind/Player Data")]
public class PlayerData : ScriptableObject
{
    public event System.Action<PlayerEffect> OnEffectChange;
    public void OnEnable()
    {
        if (OnEffectChange == null) { return; }
        foreach (Delegate d in OnEffectChange.GetInvocationList())
        {
            OnEffectChange -= (Action<PlayerEffect>)d;
        }
    }

    [NaughtyAttributes.OnValueChanged("NotifyObservers")]
    public PlayerStats Stats;
    [NaughtyAttributes.OnValueChanged("NotifyObservers")]
    public PlayerAbilities Abilities;
    [NaughtyAttributes.OnValueChanged("NotifyObservers")]
    public PlayerRunes Runes;
    [NaughtyAttributes.OnValueChanged("NotifyObservers")]
    [SerializeField]
    private PlayerEffect _effects;
    public PlayerEffect Effects { 
        get => _effects; 
        set
        {
            _effects = value;
            OnEffectChange?.Invoke(_effects);
        }
    }

    [Header("Drag a Player Data here to copy values.")]
    public PlayerData SetValues;

    public void NotifyObservers()
    {
        OnEffectChange?.Invoke(_effects);
        Stats?.NotifyObservers();
        Abilities?.NotifyObservers();
        Runes?.NotifyObservers();
    }

    [NaughtyAttributes.Button("Reset Data")]
    public void ResetData()
    {
        Effects = 0;
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
