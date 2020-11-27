using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscapeMenu : MonoBehaviour
{
    public bool Paused = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Paused)
            {
                Time.timeScale = 1f;
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
                Paused = false;
            }
            else
            {
                Time.timeScale = 0f;
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                Paused = true;
            }
        }
    }

    public void Leave()
    {
        Time.timeScale = 1f;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        Paused = false;
    }
}
