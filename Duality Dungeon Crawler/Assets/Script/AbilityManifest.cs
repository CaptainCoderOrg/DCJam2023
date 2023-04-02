#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityManifest", menuName = "AbilityManifest")]
public class AbilityManifest : ScriptableObject, ISerializationCallbackReceiver
{
    public static AbilityManifest Instance { get; private set; }
    private Dictionary<string, AbilityDefinition> _lookup;
    [field: SerializeField]
    public List<AbilityDefinition> Abilities { get; private set; }
    public bool LoadAbilities = false;
    private Dictionary<string, AbilityDefinition> Lookup => _lookup ??= InitLookup();

    public void OnEnable()
    {
        Instance = this;
        Debug.Log("Ability Manifest Loaded");
    }
    

    private Dictionary<string, AbilityDefinition> InitLookup()
    {
        Dictionary<string, AbilityDefinition> lookup = new();
        foreach (AbilityDefinition definition in Abilities)
        {
            lookup[definition.RunePhrase] = definition;
        }
        return lookup;
    }

    
    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        // Ignore
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        _lookup = null;
    }

    /// <summary>
    /// Attempts to find a <paramref name="definition"/> based on a <paramref name="runePhrase"/>. If 
    /// a definition is found, returns true and <paramref name="definition"/> is set. Otherwise, returns 
    /// false and the value of <paramref name="definition"/> is undefined.
    /// </summary>
    public static bool TryLookup(string runePhrase, out AbilityDefinition definition) => Instance.Lookup.TryGetValue(runePhrase, out definition);


#if UNITY_EDITOR
    #region Load Manifest
    private void OnValidate()
    {
        if (LoadAbilities)
        {
            FindAllAbilities();
            LoadAbilities = false;
        }
    }

    private void FindAllAbilities()
    {
        Abilities.Clear();
        Debug.Log("Loading abilities...");
        var paths = AssetDatabase.FindAssets($"t:{nameof(AbilityDefinition)}").Select(AssetDatabase.GUIDToAssetPath);
        foreach (string path in paths)
        {
            Abilities.Add(AssetDatabase.LoadAssetAtPath<AbilityDefinition>(path));
        }
        Debug.Log($"Found {Abilities.Count()} abilities!");
    }
    #endregion
#endif
}
