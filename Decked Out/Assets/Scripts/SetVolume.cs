using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public GameObject escapeMenuCanvas;
    public string MixerName;

    public static int volumesLoaded = 0;

    Slider slider;
    float volume;

    void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
        LoadPlayerPrefsVolume();
        volumesLoaded++;
        if (volumesLoaded == 2)
            escapeMenuCanvas.SetActive(false);
    }

    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat(MixerName, Mathf.Log10(sliderValue) * 20);
        volume = sliderValue;
        SavePlayerPrefs();
    }
    public void LoadPlayerPrefsVolume()
    {
        volume = PlayerPrefs.GetFloat(MixerName, 0.75f);
        slider.value = volume;
        SetLevel(volume);
    }
    void SavePlayerPrefs()
    {
        PlayerPrefs.SetFloat(MixerName, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
        => volume;
}
