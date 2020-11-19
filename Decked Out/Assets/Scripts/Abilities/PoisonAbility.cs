using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoisonAbility : MonoBehaviour
{
    private float basePoisonDamageCooldown = 1f;
    private List<Enemy> infectedEnemies = new List<Enemy>();
    public GameObject pfPoisonCloud;
    public void ApplyPoisonEffect(Enemy enemy, float poisonDamageAmount)
    {
        if (!enemy.Infected)
        {
            enemy.Infected = true;
            enemy.infectedCount++;
            enemy.infectedDamageCooldown = basePoisonDamageCooldown;
            infectedEnemies.Add(enemy);
        }
        else
            enemy.infectedCount = enemy.infectedCount < 5 ? enemy.infectedCount + 0.2f : 5;
    }

    private void Update()
    {
        foreach (Enemy targetEnemy in EnemyWaveManager.enemies.ToList())
        {
            if (targetEnemy.Infected)
            {
                targetEnemy.infectedDamageCooldown -= Time.deltaTime;

                if (targetEnemy.infectedDamageCooldown < 0)
                {
                    targetEnemy.infectedDamageCooldown = basePoisonDamageCooldown;
                    targetEnemy.Damage(gameObject.GetComponent<Card>().actualAbility * targetEnemy.infectedCount, true);
                    if (targetEnemy.health <= 0)
                    {
                        GameObject pe = Instantiate(pfPoisonCloud);
                        pe.transform.position = new Vector3(targetEnemy.GetPosition().x, targetEnemy.GetPosition().y, 45);
                        pe.transform.SetParent(GameObject.Find("Animations").transform, true);
                    }
                    DamagePopup.Create(targetEnemy.GetPosition(), (int)(gameObject.GetComponent<Card>().actualAbility * targetEnemy.infectedCount), false, ColorUtility.ToHtmlStringRGBA(gameObject.GetComponent<Card>().AccentsColor));
                }
            }
        }
    }
}
