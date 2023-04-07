using UnityEngine;

using CaptainCoder.Core;
using System.Collections.Generic;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public MutablePosition Position;
    public Direction Facing;
    public float BobbleAmount;
    public float BobbleSpeed;
    public float BaseY;
    public GameObject PivotObject;
    public float ActionSpeed = 2f;
    public bool IsAlive = true;
    public Coroutine routine;
    [field: SerializeField]
    public virtual ProjectileController Projectile { get; set; }

    public void Awake()
    {
        BaseY = transform.position.y;
    }

    public void OnEnable()
    {
        routine = StartCoroutine(RunActions());
    }

    public void OnDisable()
    {
        if (routine != null)
        {
            StopCoroutine(routine);
        }
    }

    public void Update()
    {
        float y =  (Mathf.Sin(Time.time * BobbleSpeed)* BobbleAmount) + BaseY;
        transform.localPosition = new Vector3(Position.Row * PlayerMovementController.GridCellSize, y, Position.Col * PlayerMovementController.GridCellSize);
        PivotObject.transform.rotation = Facing.ToQuaternion();

    }

    private IEnumerator RunActions()
    {
        while (IsAlive)
        {
            yield return new WaitForSeconds(ActionSpeed);
            TakeAction();
        }
    }

    public virtual void TakeAction()
    {
        // If not facing player, turn to face player.
        Facing = RotateDirection();
        // If not on player position, move toward player
        Position p = Position.Freeze();
        if (p != PlayerMovementController.Instance.Position)
        {
            Position.Row += Facing.MovePosition().Row;
            Position.Col += Facing.MovePosition().Col;
        }
        
        if (ShouldFire())
        {
            ProjectileController projectile = Instantiate(Projectile);
            projectile.Direction = Facing;
            projectile.IsFire = true;
            GameObject map = PlayerMovementController.Instance.CurrentMap.gameObject;
            projectile.transform.SetParent(map.transform);
            projectile.transform.position = transform.position;
        }
    }

    private bool ShouldFire()
    {
        Position diff = Position.Freeze() - PlayerMovementController.Instance.Position;
        if (diff.Row > 5 || diff.Col > 5) { return false; }
        return IsLinedUp() && RotateDirection() == Facing;
    }

    private bool IsLinedUp()
    {
        Position diff = Position.Freeze() - PlayerMovementController.Instance.Position;
        int rowSign = diff.Row == 0 ? 0 : (int)Mathf.Sign(diff.Row);
        int colSign = diff.Col == 0 ? 0 : (int)Mathf.Sign(diff.Col);
        return (rowSign, colSign) switch
        {
            (1, 0) => true,
            (-1, 0) => true,
            (0, 1) => true,
            (0, -1) => true,
            _ => false,
        };
    }

    private Direction RotateDirection() 
    {
        Position diff = Position.Freeze() - PlayerMovementController.Instance.Position;
        int rowSign = diff.Row == 0 ? 0 : (int)Mathf.Sign(diff.Row);
        int colSign = diff.Col == 0 ? 0 : (int)Mathf.Sign(diff.Col);
        return (rowSign, colSign) switch
        {
            (0, 0) => PlayerMovementController.Instance.Facing.RotateClockwise().RotateClockwise(),
            (1, _) => Direction.North,
            (-1, _) => Direction.South,
            (_, 1) => Direction.West,
            (_, -1) => Direction.East,
        };
    }

}