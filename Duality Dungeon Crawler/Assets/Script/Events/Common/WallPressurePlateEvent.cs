using UnityEngine;
using System;
using CaptainCoder.Core;

[CreateAssetMenu(fileName = "WallPressurePlate", menuName = "BodyMind/Events/Common/Wall Pressure Plate")]
public class WallPressurePlate : MapEvent
{
    public MutablePosition Position;
    public Direction Facing;
    public string GameObjectName;
    public string OpenMessage = "Click! The gate opens.";
    public string CloseMessage = "Click! The gate closes.";
    private PlayerData Player => GameManager.Instance.Player;
    private DialogController Dialog => DialogController.Instance;
    private GameObject _gate;
    private bool _isHeld = false;
    public AudioClip ClickSound;
    private GameObject Wall
    {
        get
        {
            _gate ??= GameObject.Find(GameObjectName);
            Debug.Assert(_gate != null, $"Pressure Plate Broken. Could not find \"{GameObjectName}\".");
            return _gate;
        }
    }

    public override bool OnEnter()
    {
        if (_isHeld)
        {
            MessageController.Display("A dense Moon ball sits on this pressure plate.");
        }
        else
        {
            Wall.SetActive(false);
            MessageController.Display(OpenMessage);
            SoundEffectController.PlaySFX(ClickSound);
            IWall wall = PlayerMovementController.Instance.CurrentMap.MapData.Grid.WallAt(Position.Freeze(), Facing);
            wall.IsPassable = true;
            GameManager.Instance.AbilityController.OnAbilityFinished += CheckForMoonBall;
        }
        return false;   
    }

    public override bool OnExit()
    {
        if (!_isHeld)
        {
            Wall.SetActive(true);
            MessageController.Display(CloseMessage);
            SoundEffectController.PlaySFX(ClickSound);
            IWall wall = PlayerMovementController.Instance.CurrentMap.MapData.Grid.WallAt(Position.Freeze(), Facing);
            wall.IsPassable = false;
        }
        GameManager.Instance.AbilityController.OnAbilityFinished -= CheckForMoonBall;
        return false;
    }

    private void CheckForMoonBall(AbilityDefinition ability)
    {
        if (ability is MoonBallAbility)
        {
            MessageController.Display($"The ball holds the pressure plate in place.");
            _isHeld = true;
        }
    } 

    private void OnEnable()
    {
        _isHeld = false;
        _gate = null;
    }
  
}