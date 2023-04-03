using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CaptainCoder.Core;
using Cinemachine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMovementController : MonoBehaviour
{
    private PlayerControls _controls;
    [field: SerializeField]
    public float GridCellSize { get; private set; } = 5;
    [field: SerializeField]
    public List<CameraPosition> Cameras { get; private set; }
    [SerializeField]
    private MutablePosition _position;
    public Position Position
    {
        get => _position.Freeze();
        set
        {
            _position = new MutablePosition { Row = value.Row, Col = value.Col };
            PositionCamera();
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
                Position += Facing.MovePosition();
                break;
            case InputType.Backward:
                Position -= Facing.MovePosition();
                break;
            case InputType.Left:
                Position += Facing.RotateCounterClockwise().MovePosition();
                break;
            case InputType.Right:
                Position += Facing.RotateClockwise().MovePosition();
                break;
            default:
                throw new System.Exception($"Could not handle input type {type}.");
        }
        return true;
    }

    private void PositionCamera()
    {
        Vector3 newPosition = new(_position.Row * GridCellSize, 0, _position.Col * GridCellSize);
        transform.position = newPosition;
        foreach (CameraPosition camera in Cameras)
        {
            camera.Camera.gameObject.SetActive(camera.Facing == Facing);
        }
    }

    public void OnValidate()
    {
        Position = _position;
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