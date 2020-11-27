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
    void Populate()
    {
        GameObject newObj;

        for (int i = 0; i < numberToCreate; i++)
        {
            if (PlayerPrefs.GetInt(cards[i].name, 0) != 0)
            {
                newObj = (GameObject)Instantiate(cards[i], transform);
                newObj.transform.localScale = new Vector3(1.4f, 1.4f, 0);
                Destroy(newObj.GetComponent<CardSlot>());
                newObj.AddComponent<CardsHandler>();
                newObj.GetComponent<CardsHandler>().CardMenu = GameObject.FindGameObjectWithTag("CardMenu");
                newObj.GetComponent<CardsHandler>().Deck = GameObject.FindGameObjectWithTag("Deck");
                newObj.tag = "AcquiredCard";
            }
        }
        GameObject.FindGameObjectWithTag("CardMenu").SetActive(false);
    }
}
