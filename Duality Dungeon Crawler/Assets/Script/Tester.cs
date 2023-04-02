using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("Awaken my son... It is time... FOR TESTER!");
    }
    // Start is called before the first frame update
    void Start()
    {
        if(AbilityManifest.TryLookup("123", out AbilityDefinition def))
        {
            Debug.Log($"Found it! {def.Name}");
        }
        if(!AbilityManifest.TryLookup("not real", out def))
        {
            Debug.Log($"Did not find it!");
        }
    }
}
