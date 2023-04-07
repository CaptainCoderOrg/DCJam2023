using CaptainCoder.Core;
using UnityEngine;
using System.Collections.Generic;

public class PedestalController : MonoBehaviour
{
    public static Dictionary<Position, PedestalController> Pedestals = new();

    public bool IsLit;
    public GameObject Center;
    public Direction Direction;
    public PositionController PositionController { get; set; }
    public GameObject FireEffect;

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

}