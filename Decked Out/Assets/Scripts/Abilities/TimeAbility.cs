using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeAbility : MonoBehaviour
{
    const float BASE_REWIND_TIMER = 5f;
    const float BASE_REWINDABILITY_TIMER = 30f;
    static float rewindAbilityTimer;
    static float rewindTimer;
    static bool isRewinding = false;
    static int TimeCardsCount;

    static GameObject timeSound = null;
    static bool playedSound = false;

    private void Start()
    {
        TimeCardsCount++;
        rewindTimer = BASE_REWIND_TIMER;
        rewindAbilityTimer = BASE_REWINDABILITY_TIMER;
    }
    private void Update()
    {
        rewindAbilityTimer -= Time.deltaTime / TimeCardsCount;
        if (isRewinding)
        {
            if (!playedSound)
            {
                playedSound = true;
                timeSound = SoundManager.PlaySound(GameAssets.Instance.Card_Time_Rewind, 10, true);
            }
            rewindTimer -= Time.deltaTime / TimeCardsCount;
            if (rewindTimer < 0)
            {
                //timeSound.GetComponent<AudioSource>().loop = false;
                //timeSound.GetComponent<AudioSource>().Stop();
                DestroyImmediate(timeSound);
                playedSound = false;
                timeSound = null;
                isRewinding = false;
                Enemy.Rewinding = false;
                rewindTimer = BASE_REWIND_TIMER;
                rewindAbilityTimer = BASE_REWINDABILITY_TIMER;
            }
        }
    }
    public void TryRewindEnemies()
    {
        if (rewindAbilityTimer < 0)
        {
            if (Random.Range(0, 101) <= gameObject.GetComponent<Card>().actualAbility)
            {
                isRewinding = true;
                Enemy.Rewinding = true;
            }
        }
    }
}
