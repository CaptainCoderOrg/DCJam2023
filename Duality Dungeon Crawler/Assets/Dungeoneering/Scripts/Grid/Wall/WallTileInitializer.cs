using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CaptainCoder.Dungeoneering

{
    public class WallTileInitializer : MonoBehaviour
    {
        private bool _isSet = false;
        [SerializeField]
        private GameObject _northSouth;
        [SerializeField]
        private GameObject _eastWest;
        // [SerializeField]
        // private Material _material;
        
        public void Initialize(bool isNorthSouth)
        {
            Debug.Assert(_isSet == false, "Wall was previously set!");
            if (isNorthSouth)
            {
                _northSouth.SetActive(true);
                DestroyImmediate(_eastWest);
            }
            else // isEastWest
            {
                _eastWest.SetActive(true);
                DestroyImmediate(_northSouth);
            }
            _isSet = true;
        }

        // public void OnValidate()
        // {
        //     WallRenderer[] renderers = GetComponentsInChildren<WallRenderer>();
        //     foreach (WallRenderer renderer in renderers)
        //     {
        //         renderer.Material = _material;
        //     }
        // }
    }
}