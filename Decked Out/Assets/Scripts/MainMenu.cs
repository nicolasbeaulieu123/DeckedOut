using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("StageSelection");
    }
    public void mainmenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
        CardsHandler.ChangedScene();
    }
    public void Deck()
    {
        SceneManager.LoadScene("Deck");
    }
    public void Shop()
    {
        SceneManager.LoadScene("Shop");
    }
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
