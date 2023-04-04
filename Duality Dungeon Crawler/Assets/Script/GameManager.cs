using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public MapLoaderController EntranceMap;
    public MapLoaderController YangMap;
    public MapLoaderController YinMap;
    public PlayerStats PlayerStats;
    public AbilityManifest AbilityManifest;

    public void Awake()
    {
        Instance = this;
    }

    internal MapLoaderController GetMap(Location target)
    {
        return target switch
        {
            Location.Entrance => EntranceMap,
            Location.Yang => YangMap,
            Location.Yin => YinMap,
            _ => throw new NotImplementedException($"Could not load map {target}"),
        };
    }
}

public enum Location
{
    Entrance,
    Yang,
    Yin
}
