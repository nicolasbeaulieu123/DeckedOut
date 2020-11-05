using System;
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

    public static List<Enemy> enemies = new List<Enemy>();

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
    private int tempFEnemyHp;
    private int fEnemyHpScale = 100;
    private int fEnemyHpScaleMod = 125;
    private const int FAST_ENEMYHP_SCALE_MOD_MOD = 50;

    // For MiniBoss enemies (Triangle)
    private int mbEnemyHp;

    // CP Gain amount for normal and fast enemies
    private static int EnemyCpGainAmount = 10;
    private static int EnemyCpGainAmountMod = 10;
    private const int MAX_ENEMY_CP_GAIN_AMOUNT = 50;

    // CP Gain amount for miniboss
    private static int MiniBossCpGainAmount = 50;
    private static int MiniBossCpGainAmountMod = 50;
    private const int MAX_MINIBOSS_CP_GAIN_AMOUNT = 250;

    // CP Gain amount for boss
    private static int BossCpGainAmount = 100;
    private static int BossCpGainAmountMod = 100;
    private const int MAX_BOSS_CP_GAIN_AMOUNT = 500;

    [SerializeField] private Vector3 spawnPosition = new Vector3(-299.95f, -260.5f);
    private void Start()
    {
        state = State.WaitingToSpawnNextWave;
        nextWaveSpawnTimer = 3f;
    }

    public static void EnemyDied(Enemy enemy, bool killedByPlayer = false)
    {
        if (killedByPlayer)
            switch (enemy.name)
            {
                case "SquareEnemy(Clone)":
                    PlayerStats.CP += EnemyCpGainAmount;
                    break;
                case "CircleEnemy(Clone)":
                    PlayerStats.CP += EnemyCpGainAmount;
                    break;
                case "MiniBossEnemy(Clone)":
                    PlayerStats.CP += MiniBossCpGainAmount;
                    break;
                case "BossEnemy(Clone)":
                    PlayerStats.CP += BossCpGainAmount;
                    break;
            }
        enemies.Remove(enemy);
        Destroy(enemy.gameObject);
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
            created.GetComponent<Enemy>().health = (int)(64.176 * Math.Pow(waveNumber, 2.8152) + (0.2755 * Math.Pow(waveNumber, 4) - 13.32 * Math.Pow(waveNumber, 3) - 10.387 * Math.Pow(waveNumber, 2) + 1921.6 * waveNumber - 2528.7));
            bossRemaining--;
            remainingEnemySpawnAmount--;
        }
        else if (miniBossRemaining > 0)
        {
            Enemy created = Enemy.Create(spawnPosition, EnemyTypes.MiniBoss + "Enemy");
            enemies.Add(created);
            created.GetComponent<Enemy>().health = (int)(28 * Math.Pow(waveNumber, 2.9096) + (-0.0001 * Math.Pow(waveNumber, 5) + 0.2588 * Math.Pow(waveNumber, 4) - 10.514 * Math.Pow(waveNumber, 3) + 50.258 * Math.Pow(waveNumber, 2) + 541.13 * waveNumber - 85.326));
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
            Enemy created = Enemy.Create(spawnPosition, EnemyTypes.Circle + "Enemy");
            if (nextFastEnemiesAmount == fastEnemiesRemaining)
                fEnemyHp = (int)(0.6949 * Math.Pow(waveNumber, 3) + 15.645 * Math.Pow(waveNumber, 2) - 12.023 * waveNumber + 441.27);
            else
                fEnemyHp += fEnemyHpScale;
            created.GetComponent<Enemy>().health = fEnemyHp;
            enemies.Add(created);
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
        if (waveNumber % 5 == 1 && waveNumber % 10 != 0 && waveNumber > 5)
        {
            fEnemyHpScale += fEnemyHpScaleMod;
            fEnemyHpScaleMod += FAST_ENEMYHP_SCALE_MOD_MOD;
        }
        nEnemyHpScale = waveNumber > 1 ? nEnemyHpScale + nEnemyHpScaleMod : nEnemyHpScale;
        //OnWaveNumberChanged?.Invoke(this, EventArgs.Empty);
    }

}
