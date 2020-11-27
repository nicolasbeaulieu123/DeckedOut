using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicClass : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    static bool loaded = false;
    void Awake()
    {
        if (!loaded)
            DontDestroyOnLoad(gameObject);
        else if (!gameObject.name.Contains("Camera"))
        {
            Destroy(gameObject);
            Resources.FindObjectsOfTypeAll<MusicClass>()[0].gameObject.SetActive(true);
        }
        loaded = true;
    }

    public void PlayMusic()
    {
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
}
