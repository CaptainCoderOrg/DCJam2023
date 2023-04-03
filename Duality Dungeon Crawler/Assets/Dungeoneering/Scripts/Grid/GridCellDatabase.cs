using UnityEngine;
using System.Collections.Generic;

namespace CaptainCoder.Dungeoneering
{
    public class GridCellDatabase : MonoBehaviour
    {
        [SerializeField]
        private List<Entry> _cellEntries;
        [SerializeField]
        private GameObject _defaultFloor;
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
            _cellEntryDict = null;
            _wallEntryDict = null;   
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
    }
}