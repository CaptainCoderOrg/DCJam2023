using UnityEngine;
using System.Collections.Generic;
using System;
using CaptainCoder.Audio;

public class GameManager : MonoBehaviour
{
    private static Queue<Action> OnReady = new();

    // This hack is insane, don't do this
    public static void EnqueueAction(string details, Action toEnqueue)
    {
        if (Instance == null)
        {
            Debug.Log($"Action Enqueued: {details}");
            OnReady.Enqueue(toEnqueue);
        }
        else
        {
            toEnqueue.Invoke();
        }
    }
    public static GameManager Instance { get; private set; }
    public event Action OnInterrupt;
    public MapLoaderController EntranceMap;
    public MapLoaderController YangMap;
    public MapLoaderController YinMap;
    public PlayerStats PlayerStats;
    public PlayerData Player;
    public AbilityManifest AbilityManifest;
    public RuneManifest Runes;
    public PlayerAbilityController AbilityController;
    public MusicTrackDatabase MusicTracks;

    public void Awake()
    {
        Instance = this;
        while (OnReady.Count > 0)
        {
            OnReady.Dequeue().Invoke();
        }
    }

    public void Start()
    {
        Player.NotifyObservers();
    }

    public void InterruptAbilities() => OnInterrupt?.Invoke();

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
