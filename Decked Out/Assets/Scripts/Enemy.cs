using System.Collections;
using System.Collections.Generic;
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
                EnemyWaveManager.EnemyDied(gameObject);
                Destroy(gameObject);
            }
        }
    }

    public void DisplayHealth(){
        healthText.text = health.ToString();
    }


    public static Enemy Create(Vector3 position,string enemyName) {
        Transform pfEnemy = Resources.Load<Transform>(enemyName);
        Transform enemyTransform = Instantiate(pfEnemy, position, Quaternion.identity);
        Enemy enemy = enemyTransform.GetComponent<Enemy>();
        return enemy;
    }
}
