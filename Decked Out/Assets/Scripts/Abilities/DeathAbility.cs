using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAbility : MonoBehaviour
{
    public GameObject pfEnemyInstantDeathAnimation;
    public void TryInstantDeath(Enemy enemy, float instantDeathChance)
    {
        if (Random.Range(0, 101) <= instantDeathChance)
        {
            GameObject pe = Instantiate(pfEnemyInstantDeathAnimation);
            pe.transform.SetParent(GameObject.Find("Animations").transform, false);
            pe.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, 0);
            float damage = enemy.name.Contains("Boss") ? enemy.health * 0.1f : enemy.health;
            DamagePopup.Create(enemy.GetPosition(), (int)damage, false, ColorUtility.ToHtmlStringRGBA(gameObject.GetComponent<Card>().AccentsColor));
            enemy.Damage(damage, true);
        }
    }
}
