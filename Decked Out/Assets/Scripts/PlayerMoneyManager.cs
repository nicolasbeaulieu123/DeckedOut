using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMoneyManager : MonoBehaviour
{
    private int money;
    private TextMeshProUGUI moneyText;
    void Start()
    {
        money = PlayerPrefs.GetInt("Money", 0);
        moneyText = gameObject.GetComponent<TextMeshProUGUI>();
        moneyText.text = money.ToString();
    }

    void Update()
    {
        
    }
}
