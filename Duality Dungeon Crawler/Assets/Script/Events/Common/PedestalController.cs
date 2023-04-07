using CaptainCoder.Core;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PedestalController : MonoBehaviour
{
    public static Dictionary<Position, PedestalController> Pedestals = new();
    
    public bool IsLit;
    public bool IsFiring;
    public GameObject Center;
    public Direction Direction;
    public PositionController PositionController { get; set; }
    public GameObject FireEffect;
    private Coroutine routine;
    public ProjectileController EnergyBall;
    public Transform EnergyBallSpawnPoint;

    public void Awake()
    {
        PositionController = GetComponent<PositionController>();

    }

    public void Start()
    {
        Pedestals[PositionController.Position] = this;
    }

    public void Rotate()
    {
        
        Direction = Direction.RotateClockwise();
        Center.transform.rotation = DirectionToQuaternion();
        MessageController.Display($"The pedestal is now facing {Direction}");
    }

    private Quaternion DirectionToQuaternion()
    {
        return Direction switch
        {
            Direction.North or Direction.South => Quaternion.Euler(0, 90, 0),
            _ => Quaternion.Euler(0, 0, 0),
        };
    }

    public void OnTriggerEnter(Collider other)
    {
        ProjectileController projectile = other.attachedRigidbody?.GetComponent<ProjectileController>();
        if (projectile != null && projectile.IsFire)
        {
            IsLit = true;
            FireEffect.SetActive(true);
        }
    }

    public void Update()
    {
        if (IsLit && IsFiring && routine == null)
        {
            routine = StartCoroutine(Fire());
        }
    }

    public IEnumerator Fire()
    {
        ProjectileController ball = Instantiate(EnergyBall);
        ball.transform.SetParent(PlayerMovementController.Instance.CurrentMap.transform);
        Vector3 p = transform.position;
        // p.y = 2.5f;
        // p.x += 
        ball.transform.position = p;
        ball.Direction = Direction;
        yield return new WaitForSeconds(1f);
        routine = null;
    }

}