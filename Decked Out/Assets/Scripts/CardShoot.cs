using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardShoot : MonoBehaviour
{
    private Vector3 projectileShootFromPosition;
    private float shootTimerMax;
    private float shootTimer;
    private Card card;

    private void Awake()
    {
        card = transform.parent.GetComponent<Card>();
        projectileShootFromPosition = transform.Find("ProjectileShootFromPosition").position;
        shootTimerMax = card.actualAttackSpeed;
        shootTimer = shootTimerMax / card.starCount * transform.GetSiblingIndex() + shootTimerMax / card.starCount;
        if (card.starCount == 7)
            shootTimer = shootTimerMax / card.starCount;
    }
    private void Update()
    {
        shootTimerMax = card.actualAttackSpeed;
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            Enemy enemy = GetTargetEnemy(card.Target);
            if (enemy != null)
            {
                Projectile.Create(projectileShootFromPosition, enemy, card);
                gameObject.GetComponent<CardShootAnimation>().CardShot();
                shootTimer = shootTimerMax;
                if (card.starCount == 7)
                    shootTimer = shootTimerMax / card.starCount;
            }
            else
                shootTimer = shootTimerMax / card.starCount * transform.GetSiblingIndex() + shootTimerMax / card.starCount;
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
