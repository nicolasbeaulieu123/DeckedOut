using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstTimeOpenGame : MonoBehaviour
{
    [SerializeField] GameObject camera;
    private void Start()
    {
        SceneManager.MoveGameObjectToScene(camera, SceneManager.GetSceneByName("Main Menu"));
        if (!PlayerPrefs.HasKey("FirstSave") || !PlayerPrefs.HasKey("Card1") || !PlayerPrefs.HasKey("Card2") || !PlayerPrefs.HasKey("Card3") || !PlayerPrefs.HasKey("Card4") || !PlayerPrefs.HasKey("Card5"))
        {
            PlayerPrefs.SetString("FirstSave", "True");
            PlayerPrefs.SetString("Card1", "Electric");
            PlayerPrefs.SetString("Card2", "Mechanical");
            PlayerPrefs.SetString("Card3", "Sacrifice");
            PlayerPrefs.SetString("Card4", "Frost");
            PlayerPrefs.SetString("Card5", "Wind");
            PlayerPrefs.SetInt("Wind", 1);
            PlayerPrefs.SetInt("Electric", 1);
            PlayerPrefs.SetInt("Frost", 1);
            PlayerPrefs.SetInt("Mechanical", 1);
            PlayerPrefs.SetInt("Sacrifice", 1);
            PlayerPrefs.Save();
        }
    }
}
