using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float baseSpeed;
    public float health;
    public float armor;
    public float CPGainAmount;

    public bool Infected = false;
    public float infectedDamageCooldown;

    public TextMesh healthText;
    private Waypoints Wpoints;

    public int waypointIndex = 1;

    public static bool Rewinding = false;

    void Start()
    {
        Wpoints = GameObject.FindGameObjectWithTag("Waypoints").GetComponent<Waypoints>();
    }

    void Update()
    {
        DisplayHealth();
        int targetIndex;
        if (!Rewinding)
        {
            transform.position = Vector2.MoveTowards(transform.position, Wpoints.waypoints[waypointIndex].position, speed * Time.deltaTime);
            targetIndex = waypointIndex;
        }
        else
        {
            targetIndex = waypointIndex - 1 >= 0 ? waypointIndex - 1 : 0;
            transform.position = Vector2.MoveTowards(transform.position, Wpoints.waypoints[targetIndex].position, speed * Time.deltaTime);
        }

        if (Vector2.Distance(transform.position, Wpoints.waypoints[targetIndex].position) < 0.1f)
        {
            if (waypointIndex < Wpoints.waypoints.Length - 1)
            {
                if (!Rewinding)
                    waypointIndex++;
                else if (waypointIndex > 0)
                    waypointIndex--;
            }
            else
            {
                PlayerStats.Lives--;
                gameObject.GetComponent<Enemy>().health = 0;
                EnemyWaveManager.EnemyDied(gameObject.GetComponent<Enemy>());
                Destroy(gameObject);
            }
        }
    }

    public void DisplayHealth()
    {
        healthText.text = health.ToString();
    }

    public void Damage(float damageAmount)
    {
        health -= damageAmount;
        if (IsDead())
            EnemyWaveManager.EnemyDied(this, true);
    }

    public bool IsDead()
        => health <= 0;


    public static Enemy Create(Vector3 position, string enemyName)
    {
        Transform pfEnemy = Resources.Load<Transform>(enemyName);
        Transform enemyTransform = Instantiate(pfEnemy, position, Quaternion.identity);
        enemyTransform.SetParent(GameObject.Find("Enemies").transform);
        Enemy enemy = enemyTransform.GetComponent<Enemy>();
        return enemy;
    }
    public Vector3 GetPosition()
        => transform.position;
    public static Enemy GetFirstOrLastEnemy(bool first)
    {
        SortEnemyListByDistanceFromEnd();
        Enemy enemy = null;
        if (EnemyWaveManager.enemies.Count > 0)
        {
            if (first)
                enemy = EnemyWaveManager.enemies[0];
            else
                enemy =  EnemyWaveManager.enemies[EnemyWaveManager.enemies.Count - 1];
        }
        return enemy;
    }
    public static Enemy GetStrongestEnemy()
    {
        Enemy strongestEnemy = null;
        foreach (Enemy enemy in EnemyWaveManager.enemies)
        {
            if (strongestEnemy == null)
                strongestEnemy = enemy;
            else if (enemy.health > strongestEnemy.health)
                strongestEnemy = enemy;
        }
        return strongestEnemy;
    }
    public static Enemy GetRandomEnemy()
    {
        if (EnemyWaveManager.enemies.Count > 0)
            return EnemyWaveManager.enemies[UnityEngine.Random.Range(0, EnemyWaveManager.enemies.Count)];
        return null;
    }
    public static Enemy GetNotInfectedEnemy()
    {
        SortEnemyListByDistanceFromEnd();
        Enemy targetEnemy = null;
        if (EnemyWaveManager.enemies.Count > 0)
        {
            foreach (Enemy enemy in EnemyWaveManager.enemies)
            {
                if (!enemy.Infected)
                {
                    targetEnemy = enemy;
                    break;
                }
            }
            if (targetEnemy == null)
                targetEnemy = EnemyWaveManager.enemies[0];
        }
        return targetEnemy;
    }

    public float GetEnemyDistanceFromEnd()
    {
        float totalDistanceLeft = 0;
        bool nextWaypointIsEnemyNextWaypoint = true;
        for (int i = this.waypointIndex; i < Waypoints.Instance.waypoints.Length; i++)
        {
            if (nextWaypointIsEnemyNextWaypoint)
            {
                totalDistanceLeft += Math.Abs(Vector3.Distance(this.GetPosition(), Waypoints.Instance.waypoints[i].position));
                nextWaypointIsEnemyNextWaypoint = false;
            }
            else
                totalDistanceLeft += Math.Abs(Vector3.Distance(Waypoints.Instance.waypoints[i - 1].position, Waypoints.Instance.waypoints[i].position));
        }
        return totalDistanceLeft;
    }

    public static void SortEnemyListByDistanceFromEnd()
    {
        for (int i = 0; i < EnemyWaveManager.enemies.Count - 1; i++)
        {
            for (int j = 0; j < EnemyWaveManager.enemies.Count - 1; j++)
            {
                if (EnemyWaveManager.enemies[j].GetEnemyDistanceFromEnd() > EnemyWaveManager.enemies[j + 1].GetEnemyDistanceFromEnd())
                {
                    Enemy t = EnemyWaveManager.enemies[j + 1];
                    EnemyWaveManager.enemies[j + 1] = EnemyWaveManager.enemies[j];
                    EnemyWaveManager.enemies[j] = t;
                }
            }
        }
    }
}
