using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricAbility : MonoBehaviour
{
    const int FIRST_ENEMY_DAMAGE_PERCENTAGE = 70;
    const int SECOND_ENEMY_DAMAGE_PERCENTAGE = 30;

    public void ApplyElectricAbility(Enemy enemy)
    {
        float abilityDamage = gameObject.GetComponent<Card>().actualAbility;

        if (enemy != null)
        {
            Enemy first = GetNextEnemy(enemy);
            Enemy second = GetNextEnemy(first);
            float damage;
            if (first != null)
            {
                damage = abilityDamage * FIRST_ENEMY_DAMAGE_PERCENTAGE / 100;
                // Creer l'animation d'électrocution
                first.Damage(damage, true);
                DamagePopup.Create(first.GetPosition(), damage, false, ColorUtility.ToHtmlStringRGBA(gameObject.GetComponent<Card>().AccentsColor));
            }
            if (second != null)
            {
                damage = abilityDamage * SECOND_ENEMY_DAMAGE_PERCENTAGE / 100;
                // Creer l'animation d'électrocution
                second.Damage(damage, true);
                DamagePopup.Create(second.GetPosition(), damage, false, ColorUtility.ToHtmlStringRGBA(gameObject.GetComponent<Card>().AccentsColor));
            }
        }
    }
    private Enemy GetNextEnemy(Enemy enemy)
    {
        Enemy target = null;
        if (enemy != null)
        {
            Enemy.SortEnemyListByDistanceFromEnd();
            int i = EnemyWaveManager.enemies.IndexOf(enemy);
            int indexNext = i + 1;
            if (EnemyWaveManager.enemies.Count > indexNext)
                target = EnemyWaveManager.enemies[indexNext];
        }
        return target;
    }
}
