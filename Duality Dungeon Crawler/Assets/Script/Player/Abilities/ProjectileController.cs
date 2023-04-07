using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileController : MonoBehaviour
{
    [SerializeField]
    private Direction _direction;
    public Direction Direction 
    { 
        get => _direction;
        set
        {
            _direction = value;
            _rigidBody.velocity = DirectionToVelocity(_direction) * Velocity;
            // TODO: Snap to grid.
        }
    }
    [field: SerializeField]
    public float Velocity { get; private set; }
    private Rigidbody _rigidBody;
    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        _rigidBody.velocity = DirectionToVelocity(Direction) * Velocity;
    }

    private static Vector3 DirectionToVelocity(Direction direction)
    {
        (int row, int col) = direction.MovePosition();
        return new Vector3(row, 0, col);
    }
}
