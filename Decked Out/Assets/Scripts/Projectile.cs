using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public static void Create(Vector3 spawnPosition, Enemy enemy, Card card)
    {
        GameObject projectileGO = Instantiate(Resources.Load<GameObject>("Projectile"), spawnPosition, Quaternion.identity);
        projectileGO.GetComponent<SpriteRenderer>().color = card.AccentsColor;
        projectileGO.GetComponent<SpriteRenderer>().sortingOrder = 1;
        projectileGO.transform.SetParent(GameObject.Find("Board").transform);
        Transform projectileTransform = projectileGO.transform;
        projectileTransform.SetParent(GameObject.Find("Projectiles").transform);
        Projectile projectile = projectileTransform.GetComponent<Projectile>();
        projectile.Setup(enemy, card);
    }
    private Vector3 targetPosition;
    private Enemy enemy;
    private Card card;

    private void Setup(Enemy enemy, Card card)
    {
        this.enemy = enemy;
        this.card = card;
    }

    private void Update()
    {
        if (enemy != null)
        {
            Vector3 targetPosition = enemy.GetPosition();
            Vector3 moveDir = (targetPosition - transform.position).normalized;
            float moveSpeed = 800f;
            transform.position += moveDir * moveSpeed * Time.deltaTime;

            float angle = UtilsClass.GetAngleFromVectorFloat(moveDir) - 90;
            transform.eulerAngles = new Vector3(0, 0, angle);

            float destroySelfDistance = 8f;
            if (Vector3.Distance(transform.position, targetPosition) < destroySelfDistance)
            {
                if (card != null)
                {
                    card.TryActivateAbility(enemy);
                    bool isCrit = Random.Range(0, 101) <= 10;
                    int damageAmount = isCrit ? card.actualAttack * Card.CritDamageBoost / 100 : card.actualAttack;
                    damageAmount = card.name == "Mechanical(Clone)" && enemy.name.Contains("Boss") ? damageAmount * 2 : damageAmount;
                    enemy.Damage(damageAmount);
                    DamagePopup.Create(enemy.GetPosition(), damageAmount, isCrit, "000000");
                    Destroy(gameObject);
                }
                else
                    Destroy(gameObject);
            }
        }
        else
            Destroy(gameObject);
    }
}