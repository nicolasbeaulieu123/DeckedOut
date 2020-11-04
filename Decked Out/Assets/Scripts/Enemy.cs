using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    public float speed;
    public float health;
    public float armor;
    public float CPGainAmount;
    public TextMesh healthText;
    private Waypoints Wpoints;

    private int waypointIndex;

    void Start(){
        Wpoints = GameObject.FindGameObjectWithTag("Waypoints").GetComponent<Waypoints>();
    }

    void Update(){
        DisplayHealth();
        transform.position = Vector2.MoveTowards(transform.position, Wpoints.waypoints[waypointIndex].position, speed * Time.deltaTime);

        if(Vector2.Distance(transform.position, Wpoints.waypoints[waypointIndex].position) < 0.1f){
            if(waypointIndex < Wpoints.waypoints.Length - 1){
                waypointIndex++;
            } else{
                PlayerStats.Lives--;
                gameObject.GetComponent<Enemy>().health = 0;
                EnemyWaveManager.EnemyDied(gameObject.GetComponent<Enemy>());
                Destroy(gameObject);
            }
        }
    }

    public void DisplayHealth(){
        healthText.text = health.ToString();
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        if (IsDead())
            EnemyWaveManager.EnemyDied(this, true);
    }

    public bool IsDead()
        => health <= 0;


    public static Enemy Create(Vector3 position,string enemyName) {
        Transform pfEnemy = Resources.Load<Transform>(enemyName);
        Transform enemyTransform = Instantiate(pfEnemy, position, Quaternion.identity);
        Enemy enemy = enemyTransform.GetComponent<Enemy>();
        return enemy;
    }
    public Vector3 GetPosition()
        => transform.position;
    public static Enemy GetFirstEnemy()
    {
        Enemy firstEnemy = null;
        if (EnemyWaveManager.enemies.Count > 0)
        {
            float smallestDistance = float.MaxValue;
            foreach (Enemy enemy in EnemyWaveManager.enemies)
            {
                float totalDistanceLeft = 0;
                for (int i = enemy.waypointIndex; i < Waypoints.Instance.waypoints.Length; i++)
                {
                    totalDistanceLeft += Math.Abs(Vector3.Distance(enemy.GetPosition(), Waypoints.Instance.waypoints[i].position));
                }
                if (totalDistanceLeft < smallestDistance)
                {
                    smallestDistance = totalDistanceLeft;
                    firstEnemy = enemy;
                }
            }
        }
        return firstEnemy;
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
    public static Enemy GetLastEnemy()
    {
        if (EnemyWaveManager.enemies.Count > 0)
            return EnemyWaveManager.enemies[EnemyWaveManager.enemies.Count - 1];
        return null;
    }
    public static Enemy GetRandomEnemy()
    {
        if (EnemyWaveManager.enemies.Count > 0)
            return EnemyWaveManager.enemies[UnityEngine.Random.Range(0, EnemyWaveManager.enemies.Count)];
        return null;
    }
}
