using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTimeOpenGame : MonoBehaviour
{
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("FirstSave") || !PlayerPrefs.HasKey("Card1") || !PlayerPrefs.HasKey("Card2") || !PlayerPrefs.HasKey("Card3") || !PlayerPrefs.HasKey("Card4") || !PlayerPrefs.HasKey("Card5"))
        {
            PlayerPrefs.SetString("FirstSave", "True");
            PlayerPrefs.SetString("Card1", "Fire");
            PlayerPrefs.SetString("Card2", "Mechanical");
            PlayerPrefs.SetString("Card3", "Poison");
            PlayerPrefs.SetString("Card4", "Frost");
            PlayerPrefs.SetString("Card5", "Wind");
            PlayerPrefs.Save();
        }
    }
}
