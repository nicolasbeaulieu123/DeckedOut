using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;

    public static GameAssets Instance
    {
        get
        {
            if (instance == null) instance = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            return instance;
        }
    }

    public GameObject pfDamagePopup;

    public AudioClip MagicianMusic;
    public AudioClip JokerMusic;
    public AudioClip SerpentMusic;
    public AudioClip SilencerMusic;
    public AudioClip JungleMapMusic;
    public AudioClip OceanMapMusic;
    public AudioClip LavaMapMusic;
    public AudioClip DungeonMapMusic;
    public AudioClip GreyMapMusic;

    public AudioClip DefeatSound;

    public AudioClip hitSound;
    public AudioClip Boss_Magician_Heal;
    public AudioClip Boss_Magician_Destroy;
    public AudioClip Boss_Magician_Destroy_Charge;
    public AudioClip Boss_Knight_Charge;
    public AudioClip Boss_Knight_Poof;
    public AudioClip Boss_Snake_Charge;
    public AudioClip Boss_Silencer_Charge;
    public AudioClip Card_Angel_Life;
    public AudioClip Card_Time_Rewind;
    public AudioMixer EffectsMixer;
}
