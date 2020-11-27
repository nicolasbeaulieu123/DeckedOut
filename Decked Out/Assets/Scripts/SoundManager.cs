using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public static GameObject PlaySound(AudioClip sound, int timeToDestroy = 10, bool loop = false)
    {
        GameObject soundGameobject = new GameObject("Sound");
        AudioSource audioSource = soundGameobject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = GameAssets.Instance.EffectsMixer.FindMatchingGroups("Master")[0];
        audioSource.loop = loop;
        soundGameobject.transform.SetParent(GameObject.Find("SoundEffects").transform);
        if (!loop)
        {
            audioSource.PlayOneShot(sound);
            GameObject.Destroy(soundGameobject, timeToDestroy);
        }
        else
        {
            audioSource.clip = sound;
            audioSource.Play();
        }
        return soundGameobject;
    }
}
