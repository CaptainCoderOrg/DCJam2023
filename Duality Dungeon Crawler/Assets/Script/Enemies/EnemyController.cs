using UnityEngine;

using CaptainCoder.Core;
using System.Collections.Generic;
using System.Collections;
using CaptainCoder.Audio;

public class EnemyController : MonoBehaviour
{

    public List<MapEvent> OnDeathEvent;   
    public static List<EnemyController> CurrentEnemies = new ();
    public GameObject HideOnDeath;
    public GameObject ShowOnDeath;
    public MutablePosition ResetPosition { get; set; }
    public Direction ResetDirection { get; set; }
    public int FireDamage;
    public int IceDamage;
    public int MaxHealth;
    public int Health;
    public MutablePosition Position;
    public Direction Facing;
    public float BobbleAmount;
    public float BobbleSpeed;
    public float BaseY;
    public GameObject PivotObject;
    public float ActionSpeed = 2f;
    public float ActionOffset = 0f;
    public bool IsAlive = true;
    public bool IsActing = false;
    public Coroutine routine;
    [field: SerializeField]
    public virtual ProjectileController Projectile { get; set; }

    public void Awake()
    {
        ResetPosition = Position;
        ResetDirection = Facing;
        BaseY = transform.position.y;
    }

    public void StartCombat()
    {
        IsActing = true;
        CurrentEnemies.Add(this);
    }

    public void ResetEnemy()
    {
        StopCoroutine(routine);
        IsActing = false;        
        IsAlive = true;
        Position = ResetPosition;
        Facing = ResetDirection;
        Health = MaxHealth;
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

    public virtual void Hit(ProjectileController projectile)
    {
        if (!IsActing) { return; }
        if (projectile.IsFire)
        {
            Health -= FireDamage;
        }
        else
        {
            Health -= IceDamage;
        }

        if (Health <= 0)
        {
            IsActing = false;
            IsAlive = false;
            StartCoroutine(HandleDeath());
            if (IsCombatOver())
            {
                CurrentEnemies.Clear();
                MusicController.Instance.StopSecondTrack();
                foreach (MapEvent evt in OnDeathEvent)
                {
                    evt.OnEnter();    
                }

            }
        }        
    }

    public virtual bool IsCombatOver()
    {
        return Health <= 0;
    }

    public virtual IEnumerator HandleDeath()
    {
        ShowOnDeath?.SetActive(true);
        HideOnDeath?.SetActive(false);
        yield break;
    }

    public virtual void TakeAction()
    {
        if (!IsActing) { return; }
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
        if (diff.Row > 8 || diff.Col > 8) { return false; }
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
        if (rowSign != 0 && colSign != 0)
        {
            int r = Random.Range(0, 2);
            if (r == 0) { rowSign = 0; }
            else { colSign = 0; }
        }
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