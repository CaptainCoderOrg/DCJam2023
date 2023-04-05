using System.Collections.Generic;
using CaptainCoder.Core;
using CaptainCoder.Dungeoneering;
using UnityEngine;

[CreateAssetMenu(fileName = "MapData", menuName = "BodyMind/MapData")]
public class MapData : ScriptableObject
{
    [field: SerializeField]
    public TextAsset TextData { get; private set; }

    private DungeonGrid _grid;
    public DungeonGrid Grid
    {
        get => _grid ??= DungeonGrid.Load(TextData.text.Split("\n"));
    }

    public bool TryGetEventsAt(CaptainCoder.Core.Position position, out IEventEntry evt)
    {
        if(CellEventsDict.TryGetValue(Grid.TileAt(position).Symbol, out evt)) { return true; }
        return PositionEventsDict.TryGetValue(position, out evt);
    }

    [SerializeField]
    private List<Entry> _cellEntries;
    private Dictionary<char, GameObject> _cellEntryDict;
    private Dictionary<char, GameObject> CellEntryDict
    {
        get
        {
            if (_cellEntryDict == null)
            {
                _cellEntryDict = new Dictionary<char, GameObject>();
                foreach (Entry e in _cellEntries)
                {
                    foreach (char ch in e.Key)
                    {
                        Debug.Assert(!_cellEntryDict.ContainsKey(ch), $"Entry List contains duplicate entry for character '{ch}'");
                        _cellEntryDict[ch] = e.Value;
                    }
                }
            }
            return _cellEntryDict;
        }
    }


    [SerializeField]
    private List<WallEntry> _wallEntries;
    private Dictionary<char, WallTileInitializer> _wallEntryDict;
    private Dictionary<char, WallTileInitializer> WallEntryDict
    {
        get
        {
            if (_wallEntryDict == null)
            {
                _wallEntryDict = new Dictionary<char, WallTileInitializer>();
                foreach (WallEntry e in _wallEntries)
                {
                    foreach (char ch in e.Key)
                    {
                        Debug.Assert(!_wallEntryDict.ContainsKey(ch), $"Entry List contains duplicate entry for character '{ch}'");
                        _wallEntryDict[ch] = e.Value;
                    }
                }
            }
            return _wallEntryDict;
        }
    }

    [SerializeField]
    private List<EventEntry> _cellEvents;
    private Dictionary<char, IEventEntry> _cellEventsDict;
    private Dictionary<char, IEventEntry> CellEventsDict
    {
        get
        {
            if (_cellEventsDict == null)
            {
                _cellEventsDict = new Dictionary<char, IEventEntry>();
                foreach (EventEntry e in _cellEvents)
                {
                    foreach (char ch in e.Key)
                    {
                        Debug.Assert(!_cellEventsDict.ContainsKey(ch), $"Event Entry List contains duplicate entry for character '{ch}'");
                        _cellEventsDict[ch] = e;
                    }
                }
            }
            return _cellEventsDict;
        }
    }

    [SerializeField]
    private List<PositionEventEntry> _positionEvent;
    private Dictionary<CaptainCoder.Core.Position, IEventEntry> _positionEvenDict;
    private Dictionary<CaptainCoder.Core.Position, IEventEntry> PositionEventsDict
    {
        get
        {
            if (_positionEvenDict == null)
            {
                _positionEvenDict = new Dictionary<CaptainCoder.Core.Position, IEventEntry>();
                foreach (PositionEventEntry entry in _positionEvent)
                {
                    Debug.Assert(!_positionEvenDict.ContainsKey(entry.Position.Freeze()), $"Event Entry List contains duplicate entry '{entry.Position.Freeze()}'");
                    _positionEvenDict[entry.Position.Freeze()] = entry;
                }
            }
            return _positionEvenDict;
        }
    }

    // [field: SerializeField]
    // public Material WallMaterial { get; private set; }

    public GameObject InstantiateTile(char ch, Transform parent = null) => Instantiate(CellEntryDict[ch], parent);
    public GameObject InstantiateWall(char ch, bool isNorthSouth, Transform parent = null)
    {
        WallTileInitializer wall = Instantiate(WallEntryDict[ch], parent);
        wall.Initialize(isNorthSouth);
        return wall.gameObject;
    }



    public void OnValidate()
    {
        _grid = null;
        _cellEntryDict = null;
        _wallEntryDict = null;
        _cellEventsDict = null;
    }

    [System.Serializable]
    public class Entry
    {
        [field: SerializeField]
        public string Key { get; private set; }
        [field: SerializeField]
        public GameObject Value { get; private set; }
    }

    [System.Serializable]
    public class WallEntry
    {
        [field: SerializeField]
        public string Key { get; private set; }
        [field: SerializeField]
        public WallTileInitializer Value { get; private set; }
    }

    public interface IEventEntry
    {
        public List<MapEvent> EventHandlers { get; }
        public List<GameObject> Prefabs { get; }
        public string Name { get; }
    }

    [System.Serializable]
    public class EventEntry : IEventEntry
    {
        [field: SerializeField]
        public string Name { get; private set; }
        [field: SerializeField]
        public string Key { get; private set; }
        [field: SerializeField]
        public List<MapEvent> EventHandlers { get; private set; }
        [field: SerializeField]
        public List<GameObject> Prefabs { get; private set; }
    }

    [System.Serializable]
    public class PositionEventEntry : IEventEntry
    {
        [field: SerializeField]
        public string Name { get; private set; }
        [field: SerializeField]
        public MutablePosition Position { get; private set; }
        [field: SerializeField]
        public List<MapEvent> EventHandlers { get; private set; }
        [field: SerializeField]
        public List<GameObject> Prefabs { get; private set; }
    }

}