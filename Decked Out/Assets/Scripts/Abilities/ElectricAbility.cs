using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricAbility : MonoBehaviour
{
    [SerializeField] GameObject pfElectricAnimation;
    [SerializeField] GameObject pfElectricBolt;
    const int FIRST_ENEMY_DAMAGE_PERCENTAGE = 70;
    const int SECOND_ENEMY_DAMAGE_PERCENTAGE = 30;

    const float pixelRatioLightningBolt = 40;
    const float scaleRatioLightningBolt = 0.4f;

    public void ApplyElectricAbility(Enemy enemy)
    {
        float abilityDamage = gameObject.GetComponent<Card>().actualAbility;

        if (enemy != null)
        {
            Enemy first = GetNextEnemy(enemy);
            Enemy second = GetNextEnemy(first);
            float damage;
            Vector3 position1 = new Vector3(enemy.transform.position.x, enemy.transform.position.y, 10);
            Transform parent1 = GameObject.Find("Animations").transform;
            Quaternion rotation1 = new Quaternion(0, 0, 0, 0);
            GameObject pe1 = Instantiate(pfElectricAnimation, position1, rotation1, parent1);
            pe1.transform.position = new Vector3(pe1.transform.position.x, pe1.transform.position.y, 500);
            Destroy(pe1, 1);
            if (first != null)
            {
                damage = abilityDamage * FIRST_ENEMY_DAMAGE_PERCENTAGE / 100;

                Vector3 position = new Vector3(first.transform.position.x, first.transform.position.y, 10);
                Transform parent = GameObject.Find("Animations").transform;
                Quaternion rotation = new Quaternion(0, 0 , 0, 0);
                GameObject pe = Instantiate(pfElectricAnimation, position, rotation, parent);
                pe.transform.position = new Vector3(pe.transform.position.x, pe.transform.position.y, 500);
                Destroy(pe, 1);

                first.Damage(damage, true);
                DamagePopup.Create(first.GetPosition(), damage, false, ColorUtility.ToHtmlStringRGBA(gameObject.GetComponent<Card>().AccentsColor));
            }
            if (second != null)
            {
                damage = abilityDamage * SECOND_ENEMY_DAMAGE_PERCENTAGE / 100;

                Vector3 position = new Vector3(second.transform.position.x, second.transform.position.y, 10);
                Transform parent = GameObject.Find("Animations").transform;
                Quaternion rotation = new Quaternion(0, 0, 0, 0);
                GameObject pe = Instantiate(pfElectricAnimation, position, rotation, parent);
                pe.transform.position = new Vector3(pe.transform.position.x, pe.transform.position.y, 500);
                Destroy(pe, 1);

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
