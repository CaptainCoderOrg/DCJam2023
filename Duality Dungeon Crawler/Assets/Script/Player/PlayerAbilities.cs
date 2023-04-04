using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerAbilities : ISerializationCallbackReceiver
{
    [SerializeField]
    private List<AbilityDefinition> _knownAbilities;
    private HashSet<AbilityDefinition> _abilities;

    public bool HasAbility(AbilityDefinition ability) => _abilities.Contains(ability);

    public void AddAbility(AbilityDefinition ability)
    {
        if (_abilities.Add(ability))
        {
            _knownAbilities.Add(ability);
        }
    }


    public void OnAfterDeserialize()
    {
        _abilities = _knownAbilities.ToHashSet();
    }

    public void OnBeforeSerialize()
    {
    }

    internal void NotifyObservers()
    {
        
    }
}
