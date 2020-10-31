using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    //public event EventHandler OnWaveNumberChanged;

    private enum State
    {
        WaitingToSpawnNextWave,
        SpawningWave,
    }
    private enum EnemyTypes
    {
        Circle,
        Square,
        MiniBoss,
        Boss
    }

    private static List<Enemy> enemies = new List<Enemy>();

    private State state;
    private int waveNumber = 0;
    private float nextWaveSpawnTimer;
    private float nextEnemySpawnTimer;
    private int remainingEnemySpawnAmount;

    // Remaining amount of enemies to spawn
    private int fastEnemiesRemaining;
    private int normalEnemiesRemaining;
    private int miniBossRemaining;
    private int bossRemaining;

    // Amount of fast enemies to spawn
    private int nextFastEnemiesAmount = 1;
    private const int MAX_FAST_ENEMIES_AMOUNT = 6;

    // For normal enemies (Square)
    private int nEnemyHp = 100;
    private int nEnemyHpScale = 10;
    private int nEnemyHpScaleMod = 10;
    private int nEnemyHpScaleModPer10Rounds = 10;

    // For fast enemies (Circle)
    private int fEnemyHp;
    private int fEnemyHpScale = 100;
    private int fEnemyHpScaleMod = 125;
    private const int FAST_ENEMYHP_SCALE_MOD_MOD = 50;

    // CP Gain amount for normal and fast enemies
    private int EnemyCpGainAmount = 10;
    private int EnemyCpGainAmountMod = 10;
    private const int MAX_ENEMY_CP_GAIN_AMOUNT = 50;

    // CP Gain amount for miniboss
    private int MiniBossCpGainAmount = 50;
    private int MiniBossCpGainAmountMod = 50;
    private const int MAX_MINIBOSS_CP_GAIN_AMOUNT = 250;

    // CP Gain amount for boss
    private int BossCpGainAmount = 100;
    private int BossCpGainAmountMod = 100;
    private const int MAX_BOSS_CP_GAIN_AMOUNT = 500;

    [SerializeField] private Vector3 spawnPosition = new Vector3(-229, -362);
    private void Start()
    {
        state = State.WaitingToSpawnNextWave;
        nextWaveSpawnTimer = 3f;
    }

    public static void EnemyDied(GameObject enemy)
    {
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i].health <= 0)
                enemies.RemoveAt(i);
        }
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToSpawnNextWave:
                nextWaveSpawnTimer -= Time.deltaTime;
                if (nextWaveSpawnTimer < 0f)
                {
                    SpawnWave();
                }
                break;
            case State.SpawningWave:
                if (remainingEnemySpawnAmount > 0)
                {
                    nextEnemySpawnTimer -= Time.deltaTime;
                    if (nextEnemySpawnTimer < 0f)
                    {
                        nextEnemySpawnTimer = 1f;

                        SpawnCorrectEnemy();
                        if (enemies.Count == 0)
                        {
                            PlayerStats.WaveNumber++;
                            state = State.WaitingToSpawnNextWave;
                            nextWaveSpawnTimer = 5f;
                        }
                    }
                }
                if (enemies.Count == 0 && remainingEnemySpawnAmount <= 0)
                {
                    PlayerStats.WaveNumber++;
                    state = State.WaitingToSpawnNextWave;
                    nextWaveSpawnTimer = 3f;
                }
                break;
        }
    }

    private void SpawnCorrectEnemy()
    {
        if (bossRemaining > 0)
        {
            Enemy created = Enemy.Create(spawnPosition, EnemyTypes.Boss + "Enemy");
            enemies.Add(created);
            bossRemaining--;
            remainingEnemySpawnAmount--;
        }
        else if (miniBossRemaining > 0)
        {
            enemies.Add(Enemy.Create(spawnPosition, EnemyTypes.MiniBoss + "Enemy"));
            miniBossRemaining--;
            remainingEnemySpawnAmount--;
        }
        else if (normalEnemiesRemaining > 0)
        {
            nEnemyHp = nEnemyHp + nEnemyHpScale;
            Enemy created = Enemy.Create(spawnPosition, EnemyTypes.Square + "Enemy");
            created.GetComponent<Enemy>().health = nEnemyHp;
            enemies.Add(created);
            normalEnemiesRemaining--;
            remainingEnemySpawnAmount--;
        }
        else if (fastEnemiesRemaining > 0)
        {
            enemies.Add(Enemy.Create(spawnPosition, EnemyTypes.Circle + "Enemy"));
            fastEnemiesRemaining--;
            remainingEnemySpawnAmount--;
        }
    }

    private void SpawnWave()
    {
        remainingEnemySpawnAmount = 10;
        state = State.SpawningWave;
        waveNumber++;
        if (waveNumber % 10 == 0)
        {
            bossRemaining = 1;
            remainingEnemySpawnAmount = 1;
            BossCpGainAmount = waveNumber > 10 && BossCpGainAmount < MAX_BOSS_CP_GAIN_AMOUNT ? BossCpGainAmount + BossCpGainAmountMod : BossCpGainAmount;
        }
        else if (waveNumber % 5 == 0 && waveNumber % 10 != 0)
        {
            miniBossRemaining = 1;
            fastEnemiesRemaining = nextFastEnemiesAmount;
            nextFastEnemiesAmount = nextFastEnemiesAmount < MAX_FAST_ENEMIES_AMOUNT ? nextFastEnemiesAmount++ : nextFastEnemiesAmount;
            normalEnemiesRemaining = remainingEnemySpawnAmount - miniBossRemaining - fastEnemiesRemaining;
            MiniBossCpGainAmount = waveNumber > 5 && MiniBossCpGainAmount < MAX_MINIBOSS_CP_GAIN_AMOUNT ? MiniBossCpGainAmount + MiniBossCpGainAmountMod : MiniBossCpGainAmount;
        }
        else
        {
            normalEnemiesRemaining = 10;
        }

        if (waveNumber > 10 && waveNumber % 10 == 1)
        {
            nEnemyHpScaleMod += nEnemyHpScaleModPer10Rounds;
            EnemyCpGainAmount = EnemyCpGainAmount < MAX_ENEMY_CP_GAIN_AMOUNT ? EnemyCpGainAmount + EnemyCpGainAmountMod : EnemyCpGainAmount;
        }
        nEnemyHpScale = waveNumber > 1 ? nEnemyHpScale + nEnemyHpScaleMod : nEnemyHpScale;
        //OnWaveNumberChanged?.Invoke(this, EventArgs.Empty);
    }

}
