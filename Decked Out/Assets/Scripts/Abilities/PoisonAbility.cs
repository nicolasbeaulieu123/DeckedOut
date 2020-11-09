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
            enemy.infectedDamageCooldown = basePoisonDamageCooldown;
            infectedEnemies.Add(enemy);
        }
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
                    targetEnemy.Damage(gameObject.GetComponent<Card>().actualAbility, true);
                    if (targetEnemy.health <= 0)
                    {
                        GameObject pe = Instantiate(pfPoisonCloud);
                        pe.transform.position = new Vector3(targetEnemy.GetPosition().x, targetEnemy.GetPosition().y, 45);
                        pe.transform.SetParent(GameObject.Find("Animations").transform, true);
                    }
                    DamagePopup.Create(targetEnemy.GetPosition(), gameObject.GetComponent<Card>().actualAbility, false, ColorUtility.ToHtmlStringRGBA(gameObject.GetComponent<Card>().AccentsColor));
                }
            }
        }
    }
}
