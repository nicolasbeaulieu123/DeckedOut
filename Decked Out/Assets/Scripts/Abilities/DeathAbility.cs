using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAbility : MonoBehaviour
{
    public GameObject pfEnemyInstantDeathAnimation;
    public void TryInstantDeath(Enemy enemy, float instantDeathChance)
    {
        if (Random.Range(0, 101) <= instantDeathChance && !enemy.name.Contains("Boss") && !enemy.name.Contains("MiniBoss"))
        {
            DamagePopup.Create(enemy.GetPosition(), enemy.health, false, ColorUtility.ToHtmlStringRGBA(gameObject.GetComponent<Card>().AccentsColor));
            GameObject pe = Instantiate(pfEnemyInstantDeathAnimation, enemy.GetPosition(), enemy.transform.rotation);
            pe.transform.SetParent(GameObject.Find("Animations").transform);
            enemy.Damage(enemy.health);
        }
    }
}
