using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerDeck
{
    public static void LoadPlayerDeckImages()
    {
        for (int i = 1; i <= 5; i++)
        {
            GameObject card = GameObject.Find("Deck").transform.Find("Card (" + i + ")").gameObject;
            float x = card.GetComponent<RectTransform>().position.x;
            float y = card.GetComponent<RectTransform>().position.y;
            GameObject.Destroy(card);
            GameObject created = GameObject.Instantiate(Deck()[i - 1], new Vector2(x, y), Quaternion.identity);
            created.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            created.transform.SetParent(GameObject.Find("Deck").transform);
        }
    }
    public static GameObject[] Deck()
    {
        GameObject[] CardsPrefabs = Resources.LoadAll<GameObject>("Cards");
        GameObject[] deck = new GameObject[5];
        for (int i = 1; i <= 5; i++)
        {
            foreach (GameObject card in CardsPrefabs)
            {
                if (card.name.Contains(PlayerPrefs.GetString("Card" + i)))
                    deck[i - 1] = card;
            }
        }
        return deck;
    }
}
