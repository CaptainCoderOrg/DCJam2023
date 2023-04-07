using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileController : MonoBehaviour
{
    private static int _playerLayer = 0;
    private static int _wallLayer = 0;
    private static int WallLayer => _wallLayer == 0 ? (_wallLayer = LayerMask.NameToLayer("Walls")) : _wallLayer;
    private static int PlayerLayer => _playerLayer == 0 ? (_playerLayer = LayerMask.NameToLayer("Player")) : _playerLayer;
    [field: SerializeField]
    public bool IsFire { get; set; } = false;
    [field: SerializeField]
    public bool IsEnergy { get; set; } = false;
    public AudioSource ExplodeSound;
    public GameObject Projectile;
    public GameObject ExplodeObject;
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

    void OnTriggerEnter(Collider collider)
    {  
        if (collider.gameObject.layer == WallLayer)
        {
            Explode();
        }
        if (collider.gameObject.layer == PlayerLayer && IsEnergy)
        {
            Explode();
            MessageController.Display("Ouch!");
            GameManager.Instance.Hurt();
            GameManager.Instance.Player.Stats.Stat(DualStat.BodyMind).Value -= 10;
            GameManager.Instance.Player.Stats.Stat(DualStat.SunMoon).Value += 15;
            GameManager.Instance.Player.Stats.Stat(DualStat.YinYang).Value += 5;
            SoundEffectController.PlaySFX(GameManager.Instance.SoundEffects.Hurt);    
        }
    }

    public void Explode()
    {
        Velocity = 0;
        ExplodeSound?.Play();
        Projectile.SetActive(false);
        ExplodeObject.transform.rotation = DirectionToQuaternion(Direction);
        ExplodeObject.SetActive(true);
        StartCoroutine(RemoveFromScene(1));
    }

    private Quaternion DirectionToQuaternion(Direction d)
    {
        return d switch
        {
            Direction.North or Direction.South => Quaternion.Euler(90, 90, 0),
            Direction.East or Direction.West => Quaternion.Euler(90, 0, 0),
        };
    }

    private IEnumerator RemoveFromScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
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
