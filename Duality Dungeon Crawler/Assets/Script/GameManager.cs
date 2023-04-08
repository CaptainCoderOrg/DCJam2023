using UnityEngine;
using System.Collections.Generic;
using System;
using CaptainCoder.Audio;

public class GameManager : MonoBehaviour
{
    
    public bool _resetOnStart = false;
    private static Queue<Action> OnReady = new();

    // This hack is insane, don't do this
    public static void EnqueueAction(string details, Action toEnqueue)
    {
        if (Instance == null)
        {
            // Debug.Log($"Action Enqueued: {details}");
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
    public MapLoaderController BodyMap;
    public MapLoaderController MindMap;
    public PlayerStats PlayerStats;
    public PlayerData Player;
    public AbilityManifest AbilityManifest;
    public RuneManifest Runes;
    public PlayerAbilityController AbilityController;
    public MusicTrackDatabase MusicTracks;
    public SoundEffects SoundEffects;
    public PainFlashController PainFlashController;

    public void Awake()
    {
        Instance = this;
        while (OnReady.Count > 0)
        {
            OnReady.Dequeue().Invoke();
        }
        PedestalController.Pedestals = new Dictionary<CaptainCoder.Core.Position, PedestalController>();
    }

    public void Start()
    {
        #if UNITY_EDITOR
            if (_resetOnStart) { ResetData(); }
        #else
            ResetData();
        #endif
        
        Player.NotifyObservers();
        foreach (PlayerStat stat in PlayerStats.Stats)
        {
            stat.OnStatIsZero += HandleOutOfHarmony;
        }        
    }

    private void ResetData()
    {
        PlayerMovementController.Instance.CurrentMap = EntranceMap;
        PlayerMovementController.Instance.Position = (2, 2);
        PlayerMovementController.Instance.Facing = Direction.East; 
        Player.ResetData();
    }


    public void InterruptAbilities() => OnInterrupt?.Invoke();

    private bool _isKnockedOut = false;
    public void HandleOutOfHarmony(Stat zeroStat)
    {
        if (_isKnockedOut) { return; }
        _isKnockedOut = true;
        DialogController.Instance.DisplayDialog(@"Your body is out of harmony... You can no longer continue. You feint.");
        Action WakeUp = () => {
            _isKnockedOut = false;
            PlayerMovementController.Instance.Position = (2,2);
            PlayerMovementController.Instance.Facing = Direction.North;
            PlayerMovementController.Instance.CurrentMap = EntranceMap;
        };
        DialogController.Instance.SetOptions(("Wake up", WakeUp.ThenCloseDialog()));
        DialogController.Instance.IsVisible = true;
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

    public void Hurt()
    {
        PainFlashController.ShowPain(Color.white, Color.red);
    }
}

public enum Location
{
    Entrance,
    Yang,
    Yin
}
