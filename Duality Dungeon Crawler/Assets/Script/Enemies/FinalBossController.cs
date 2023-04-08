using UnityEngine;

using CaptainCoder.Core;
using System.Collections.Generic;
using System.Collections;
using CaptainCoder.Audio;

public class FinalBossController : EnemyController
{
    private static readonly Direction[] All = {Direction.North, Direction.East, Direction.South, Direction.West};
    
    private int counter = 0;

    public override void ResetEnemy()
    {
        base.ResetEnemy();
        counter = 0;
        SolFinalEvent._isInCombat = false;
    }
    
    public override void TakeAction()
    {
        if (!IsActing) { return; }

        // Teleport
        if (counter++ % 2 == 0)
        {
            Position.Row = Random.Range(1, 7);
            Position.Col = Random.Range(1, 7);
        }
        else
        {
            foreach (Direction d in All)
            {
                ProjectileController projectile = Instantiate(Projectile);
                projectile.Direction = d;
                projectile.IsFire = true;
                GameObject map = PlayerMovementController.Instance.CurrentMap.gameObject;
                projectile.transform.SetParent(map.transform);
                projectile.transform.position = transform.position;
            }
        }
        
    }

}