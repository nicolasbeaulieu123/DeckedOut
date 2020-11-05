using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardShoot : MonoBehaviour
{
    private Vector3 projectileShootFromPosition;
    private float shootTimerMax;
    private float shootTimer;

    private void Awake()
    {
        projectileShootFromPosition = transform.Find("ProjectileShootFromPosition").position;
        shootTimerMax = transform.parent.GetComponent<Card>().actualAttackSpeed;
    }
    private void Update()
    {
        Card card = transform.parent.GetComponent<Card>();
        Enemy enemy = GetTargetEnemy(card.Target);

        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0f)
        {
            shootTimer = shootTimerMax;
            if (enemy != null)
            {
                Projectile.Create(projectileShootFromPosition, enemy, card);
            }
        }
    }

    private Enemy GetTargetEnemy(Target target)
    {
        Enemy enemy = null;
        switch (target)
        {
            default:
                break;
            case Target.First:
                enemy = Enemy.GetFirstOrLastEnemy(true);
                break;
            case Target.Last:
                enemy = Enemy.GetFirstOrLastEnemy(false);
                break;
            case Target.Random:
                enemy = Enemy.GetRandomEnemy();
                break;
            case Target.Strongest:
                enemy = Enemy.GetStrongestEnemy();
                break;
            case Target.NotInfected:
                enemy = Enemy.GetNotInfectedEnemy();
                break;
        }
        return enemy;
    }
}
