using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostAbility : MonoBehaviour
{
    public static float totalSlowEffect = 0f;
    public Enemy lastEnemy;
    public const float MAX_SLOW_PERCENTAGE = 0.5f;

    public void ApplySlowEffect(Enemy enemy, float slowPercentage)
    {
        if (enemy != lastEnemy)
        {
            totalSlowEffect = 1 - (enemy.speed / enemy.baseSpeed);
            lastEnemy = enemy;
        }
        if (totalSlowEffect < MAX_SLOW_PERCENTAGE)
        {
            enemy.speed = enemy.speed * (1 - slowPercentage);
            totalSlowEffect += slowPercentage;
        }
    }
}