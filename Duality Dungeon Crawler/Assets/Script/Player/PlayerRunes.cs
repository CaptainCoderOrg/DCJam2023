using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerRunes : ISerializationCallbackReceiver
{
    public event Action<RuneData> OnRuneAdded;
    public event Action<PlayerRunes> OnReady;
    [SerializeField]
    private List<RuneData> _runes;
    public int Count => _runesSet.Count;
    private HashSet<RuneData> _runesSet;

    [NaughtyAttributes.Button("Reset Data")]
    public void ResetData()
    {
        _runes.Clear();
        _runesSet.Clear();
    }


    public bool HasRune(RuneData ability) => _runesSet.Contains(ability);
    
    // Hacky gross oh god no!
    public bool HasRune(string ability, out RuneData rune)
    {
        foreach (RuneData r in _runesSet)
        {
            if (r.RuneIndex.ToString() == ability)
            {
                rune = r;
                return true;
            }
        }
        rune = default;
        return false;
    }

    public void AddRune(RuneData ability)
    {
        if (_runesSet.Add(ability))
        {
            _runes.Add(ability);
        }
        OnRuneAdded?.Invoke(ability);
    }


    public void OnAfterDeserialize()
    {
        _runesSet = _runes.ToHashSet();
    }

    public void OnBeforeSerialize()
    {
    }

    internal void NotifyObservers()
    {
        OnReady?.Invoke(this);
    }
}
