using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CaptainCoder.Core;
using Cinemachine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMovementController : MonoBehaviour
{
    public static event Action<Direction> OnDirectionChange;
    public static event Action<Position> OnPositionChange;
    private PlayerControls _controls;
    [field: SerializeField]
    public static float GridCellSize { get; private set; } = 5;
    [field: SerializeField]
    public List<CameraPosition> Cameras { get; private set; }
    [SerializeField]
    private MapLoaderController _currentMap;
    public MapLoaderController CurrentMap
    {
        get => _currentMap;
        set
        {
            _currentMap = value;
            transform.SetParent(_currentMap.gameObject.transform);
            PositionCamera();
        }
    }
    [SerializeField]
    private MutablePosition _position;
    public Position Position
    {
        get => _position.Freeze();
        set
        {
            _position = new MutablePosition { Row = value.Row, Col = value.Col };
            PositionCamera();
            OnPositionChange?.Invoke(_position);
        }
    }
    [SerializeField]
    private Direction _facing = Direction.North;
    public Direction Facing
    {
        get => _facing;
        set
        {
            _facing = value;
            PositionCamera();
            OnDirectionChange?.Invoke(_facing);
        }
    }

    public void Awake()
    {
        if (CurrentMap == null)
        {
            CurrentMap = GetComponentInParent<MapLoaderController>();
        }
    }


    public void OnEnable()
    {
        _controls ??= new PlayerControls();
        _controls.Enable();
        _controls.PlayerMovement.Step.started += HandleMoveInput;
        _controls.PlayerMovement.Rotate.started += HandleRotateInput;
    }

    public void OnDisable()
    {
        _controls ??= new PlayerControls();
        _controls.Disable();
        _controls.PlayerMovement.Step.started -= HandleMoveInput;
        _controls.PlayerMovement.Rotate.started -= HandleRotateInput;
    }

    private void HandleRotateInput(CallbackContext ctx)
    {
        float raw = ctx.ReadValue<float>();
        int direction = raw == 0 ? 0 : (int)Mathf.Sign(raw);
        InputType type = direction switch
        {
            1 => InputType.RotateClockwise,
            -1 => InputType.RotateCounterClockwise,
            _ => throw new System.NotImplementedException($"Could not rotate {direction}"),
        };
        TryMove(type);
    }

    private void HandleMoveInput(CallbackContext ctx)
    {
        Vector2 raw = ctx.ReadValue<Vector2>();
        int rowChange = raw.y == 0 ? 0 : (int)Mathf.Sign(raw.y);
        int colChange = raw.x == 0 ? 0 : (int)Mathf.Sign(raw.x);
        InputType type = (rowChange, colChange) switch
        {
            (1, _) => InputType.Forward,
            (-1, _) => InputType.Backward,
            (_, 1) => InputType.Right,
            (_, -1) => InputType.Left,
            _ => throw new System.NotImplementedException($"Could not handle input {(rowChange, colChange)}."),
        };
        TryMove(type);
    }

    private bool TryMove(InputType type)
    {
        switch (type)
        {
            case InputType.RotateClockwise:
                Facing = Facing.RotateClockwise();
                break;
            case InputType.RotateCounterClockwise:
                Facing = Facing.RotateCounterClockwise();
                break;
            case InputType.Forward:
            case InputType.Backward:
            case InputType.Left:
            case InputType.Right:
                if (!MoveWall(type).IsPassable)
                {
                    IllegalMove();
                }
                else
                {
                    Position += MovePosition(type);
                }
                break;
            default:
                throw new System.Exception($"Could not handle input type {type}.");
        }
        return true;
    }
    private void IllegalMove()
    {
        // TODO: Play bump sound / animation
    }
    private Position MovePosition(InputType type)
    {
        return type switch
        {
            InputType.Forward => Facing.MovePosition(),
            InputType.Backward => Facing.RotateClockwise().RotateClockwise().MovePosition(),
            InputType.Left => Facing.RotateCounterClockwise().MovePosition(),
            InputType.Right => Facing.RotateClockwise().MovePosition(),
            _ => throw new NotImplementedException($"Invalid movement {type}"),
        };
    }
    private IWall MoveWall(InputType type)
    {
        return type switch
        {
            InputType.Forward => CurrentMap.MapData.Grid.WallAt(Position, Facing),
            InputType.Backward => CurrentMap.MapData.Grid.WallAt(Position, Facing.RotateClockwise().RotateClockwise()),
            InputType.Left => CurrentMap.MapData.Grid.WallAt(Position, Facing.RotateCounterClockwise()),
            InputType.Right => CurrentMap.MapData.Grid.WallAt(Position, Facing.RotateClockwise()),
            _ => throw new NotImplementedException($"Invalid movement {type}"),
        };
    }

    private void PositionCamera()
    {
        Vector3 newPosition = new(_position.Row * GridCellSize, 0, _position.Col * GridCellSize);
        transform.localPosition = newPosition;
        foreach (CameraPosition camera in Cameras)
        {
            camera.Camera.gameObject.SetActive(camera.Facing == Facing);
        }
    }

    public void OnValidate()
    {
        Position = _position;
        CurrentMap = _currentMap;
    }

}

[System.Serializable]
public class CameraPosition
{
    public CinemachineVirtualCamera Camera;
    public Direction Facing;
}

public enum InputType
{
    Forward,
    Backward,
    Left,
    Right,
    RotateClockwise,
    RotateCounterClockwise,

}