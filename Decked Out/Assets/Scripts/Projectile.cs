using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public static void Create(Vector3 spawnPosition, Enemy enemy, Color color, int damageAmount)
    {
        GameObject projectileGO = Instantiate(Resources.Load("Projectile") as GameObject, spawnPosition, Quaternion.identity);
        projectileGO.GetComponent<SpriteRenderer>().color = color;
        projectileGO.GetComponent<SpriteRenderer>().sortingOrder = 1;
        projectileGO.transform.SetParent(GameObject.Find("Board").transform);
        Transform projectileTransform = projectileGO.transform;
        Projectile projectile = projectileTransform.GetComponent<Projectile>();
        projectile.Setup(enemy, damageAmount);
    }
    private Vector3 targetPosition;
    private Enemy enemy;
    private int damageAmount;

    private void Setup(Enemy enemy, int damageAmount)
    {
        this.enemy = enemy;
        this.damageAmount = damageAmount;
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

            float destroySelfDistance = 1f;
            if (Vector3.Distance(transform.position, targetPosition) < destroySelfDistance)
            {
                enemy.Damage(damageAmount);
                DamagePopup.Create(enemy.GetPosition(), damageAmount, false);
                Destroy(gameObject);
            }
        }
        else
            Destroy(gameObject);
    }
}