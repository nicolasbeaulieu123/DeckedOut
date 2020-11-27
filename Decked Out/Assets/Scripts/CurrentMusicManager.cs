using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrentMusicManager : MonoBehaviour
{
    public static CurrentMusicManager instance;
    public AudioSource audio;
    string bossName;
    bool MapMusicPlaying = false;
    void Start()
    {
        if (instance == null)
            instance = this;
    }

    void Update()
    {
        if (bossName != "")
        {
            switch (bossName)
            {
                case "Magician":
                    audio.clip = GameAssets.Instance.MagicianMusic;
                    break;
                case "Joker":
                    audio.clip = GameAssets.Instance.JokerMusic;
                    break;
                case "Silencer":
                    audio.clip = GameAssets.Instance.SilencerMusic;
                    break;
                case "Serpent":
                    audio.clip = GameAssets.Instance.SerpentMusic;
                    break;
            }
            audio.outputAudioMixerGroup.audioMixer.SetFloat("BackgroundVolume", PlayerPrefs.GetFloat("BackgroundVolume", 0.75f));
            audio.Play();
            MapMusicPlaying = false;
            bossName = "";
        }
        if (EnemyWaveManager.Instance.WaveNumber() % 10 == 1 && !MapMusicPlaying)
        {
            MapMusicPlaying = true;
            switch (SceneManager.GetActiveScene().name)
            {
                case "Game":
                    audio.clip = GameAssets.Instance.GreyMapMusic;
                    break;
            }
            audio.outputAudioMixerGroup.audioMixer.SetFloat("BackgroundVolume", PlayerPrefs.GetFloat("BackgroundVolume", 0.75f));
            audio.Play();
        }
    }

    public void SetBossName(string name)
    {
        bossName = name;
    }
}

