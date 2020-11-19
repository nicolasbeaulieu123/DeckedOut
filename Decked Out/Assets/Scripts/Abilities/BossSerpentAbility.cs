using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSerpentAbility : MonoBehaviour
{
    [SerializeField] private float BaseAbilityCoolDown;
    [SerializeField] private float BaseAbilitySpawnAmount;
    [SerializeField] private GameObject pfAbilityAnimation;
    private EnemyWaveManager waveManager;
    private Enemy Boss;
    private float abilityCooldown;
    bool abilityEnded = false;
    bool animationPlayed = false;
    private Vector3 spawnPosition = new Vector3(-299.95f, -260.5f);
    void Start()
    {
        abilityCooldown = 3f;
        waveManager = GameObject.Find("EnemyWaveManager").GetComponent<EnemyWaveManager>();
        Boss = gameObject.GetComponent<Enemy>();
    }

    void Update()
    {
        if (this != null)
        {
            abilityCooldown -= Time.deltaTime;

            if (abilityCooldown < 0)
            {
                if (!abilityEnded)
                    ActivateAbility();
            }
            if (abilityEnded)
            {
                abilityCooldown = BaseAbilityCoolDown;
                animationPlayed = false;
            }
            abilityEnded = false;
        }
    }

    private int spawnedCount = 0;
    private float BaseAbilitySpawnCooldown = 1f;
    private float abilitySpawnCooldown = 0f;
    void ActivateAbility()
    {
        abilitySpawnCooldown -= Time.deltaTime;

        if (abilitySpawnCooldown < 0)
        {
            Boss.Stopped = true;
            abilitySpawnCooldown = BaseAbilitySpawnCooldown;
            Enemy created = null;
            if (spawnedCount < 2)
            {
                created = Enemy.Create(spawnPosition, EnemyWaveManager.EnemyTypes.Square + "Enemy");
                created.health = (int)(Boss.health * 0.1f);
            }
            else
            {
                created = Enemy.Create(spawnPosition, EnemyWaveManager.EnemyTypes.Circle + "Enemy");
                created.health = (int)(Boss.health * 0.05f);
            }
            spawnedCount++;
            EnemyWaveManager.enemies.Add(created);
            if (!animationPlayed)
            {
                animationPlayed = true;
                GameObject pe = Instantiate(pfAbilityAnimation);
                pe.transform.position = new Vector3(Boss.GetPosition().x, Boss.GetPosition().y, 45);
                pe.transform.SetParent(GameObject.Find("Animations").transform, true);
            }
        }
        if (spawnedCount == BaseAbilitySpawnAmount)
        {
            abilityEnded = true;
            Boss.Stopped = false;
            spawnedCount = 0;
        }
    }
}
