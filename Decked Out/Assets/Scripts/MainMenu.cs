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
        SceneManager.LoadScene("Main Menu");
        CardsHandler.ChangedScene();
    }
    public void Deck()
    {
        SceneManager.LoadScene("Deck");
    }
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
