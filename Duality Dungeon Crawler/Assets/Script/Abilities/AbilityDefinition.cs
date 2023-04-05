using System.Collections.Generic;
using System.Collections;
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

    public virtual IEnumerator OnUse(PlayerData player, System.Action OnFinish)
    {
        throw new System.NotImplementedException("Subclass must implement Ability Definition.");
    }

    /// <summary>
    /// Returns if the conditions are met. If they are not message is set to
    /// the reason why it could not be cast.
    /// </summary>
    public virtual bool CanCast(PlayerData player, out string message)
    {
        message = string.Empty;
        return true;
    }
    
}