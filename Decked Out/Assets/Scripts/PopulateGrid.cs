using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class PopulateGrid : MonoBehaviour
{
    public GameObject[] cards;
    private int numberToCreate;

    void Start()
    {
        cards = Resources.LoadAll<GameObject>("Cards");
        numberToCreate = cards.Length;
        Populate();
    }

    void Update()
    {
        
    }

    void Populate()
    {
        GameObject newObj;

        for (int i = 0; i < numberToCreate; i++)
        {
            newObj = (GameObject)Instantiate(cards[i], transform);
            newObj.AddComponent<CardsHandler>();
            newObj.GetComponent<CardsHandler>().CardMenu = GameObject.FindGameObjectWithTag("CardMenu");
            newObj.GetComponent<CardsHandler>().Deck = GameObject.FindGameObjectWithTag("Deck");
            newObj.tag = "AcquiredCard";
        }
        GameObject.FindGameObjectWithTag("CardMenu").SetActive(false);
    }
}
