using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Ability")]
public class AbilityDefinition : ScriptableObject
{
    [field: SerializeField]
    public string Name { get; private set; }
    [field: SerializeField]
    public string Description { get; private set; }
    [field: SerializeField]
    public string RunePhrase { get; private set; }
    [field: SerializeField]
    public List<StatModifier> Modifiers { get; private set; } 
    
}

[System.Serializable]
public struct StatModifier
{
    public Stat Stat;
    public float Modifier;
    public int UseAmount;
}