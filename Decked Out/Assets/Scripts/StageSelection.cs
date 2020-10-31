using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelection : MonoBehaviour
{
    public void PlayStage(string StageName){
        SceneManager.LoadScene("Game");
    }
}
