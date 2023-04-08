using UnityEngine;
using System;
using CaptainCoder.Core;

[CreateAssetMenu(fileName = "OpenDoorEvent", menuName = "BodyMind/Events/Common/OpenDoorEvent")]
public class OpenDoorEvent : MapEvent
{
    public MutablePosition Position;
    public Direction Facing;
    public string GameObjectName;
    private GameObject _gate;
    private GameObject Wall
    {
        get
        {
            _gate ??= GameObject.Find(GameObjectName);
            Debug.Assert(_gate != null, $"Open Door Event Broken. Could not find \"{GameObjectName}\".");
            return _gate;
        }
    }

    public override bool OnEnter()
    {

        Wall?.SetActive(false);
        IWall wall = PlayerMovementController.Instance.CurrentMap.MapData.Grid.WallAt(Position.Freeze(), Facing);
        wall.IsPassable = true;
        return false;
    }

    private void OnEnable()
    {
        _gate = null;
    }


}