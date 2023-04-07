using CaptainCoder.Core;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RecepticalController : MonoBehaviour
{
    public string GameObjectName;
    private GameObject _gate;
    private bool _isHeld = false;
    public AudioClip ClickSound;
    private bool _isTriggered = false;
    public MutablePosition Position;
    public Direction Facing;
    public GameObject Particles;
    private GameObject Wall
    {
        get
        {
            _gate ??= GameObject.Find(GameObjectName);
            Debug.Assert(_gate != null, $"Receptical Broken. Could not find \"{GameObjectName}\".");
            return _gate;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (_isTriggered) { return; }
        ProjectileController projectile = other.attachedRigidbody?.GetComponent<ProjectileController>();
        if (projectile != null && projectile.IsEnergy)
        {
            _isTriggered = true;
            Particles.SetActive(true);
            Wall.SetActive(false);
            MessageController.Display("The gate opens!");
            SoundEffectController.PlaySFX(ClickSound);
            IWall wall = PlayerMovementController.Instance.CurrentMap.MapData.Grid.WallAt(Position.Freeze(), Facing);
            wall.IsPassable = true;
        }
    }

    
}