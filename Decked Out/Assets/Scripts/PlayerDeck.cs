using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerDeck : MonoBehaviour, IPointerDownHandler
{
    private static bool loaded = false;
    public void Start()
    {
        if (!loaded)
            LoadPlayerDeckImages();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Card cardType = eventData.pointerCurrentRaycast.gameObject.GetComponent<Card>();
        if (cardType.PowerUpLevel < 5 && PlayerStats.CP >= cardType.CostPowerUp(cardType.PowerUpLevel))
        {
            PlayerStats.CP -= cardType.CostPowerUp(cardType.PowerUpLevel);
            ChangeCardLevelText(eventData.pointerEnter);
            ChangeCardPowerUpCostText(eventData.pointerEnter);
            cardType.PowerUpCard();
            foreach (GameObject card in Board.Instance.AllCardsOnBoard())
            {
                if (card != null && card.name == eventData.pointerEnter.name)
                    card.GetComponent<Card>().PowerUpCard();
            }
        }
    }
    private void ChangeCardLevelText(GameObject cardType)
    {
        int index = cardType.transform.GetSiblingIndex() + 1;
        GameObject textObject = GameObject.Find("DeckCardLevels").transform.Find("CardLvl (" + index + ")").gameObject;
        int nextLevel = cardType.GetComponent<Card>().PowerUpLevel + 1;
        string newLevel = nextLevel == 5 ? "MAX" : nextLevel.ToString();
        string text = textObject.GetComponent<TextMeshProUGUI>().text;
        textObject.GetComponent<TextMeshProUGUI>().text = text.Remove(text.Length - 1, 1) + newLevel;
    }

    private void ChangeCardPowerUpCostText(GameObject cardType)
    {
        int index = cardType.transform.GetSiblingIndex() + 1;
        GameObject textObject = GameObject.Find("DeckCardPowerUpCosts").transform.Find("CardLvlPrice (" + index + ")").gameObject;
        Card card = cardType.GetComponent<Card>();
        string nextCost = card.CostPowerUp(card.PowerUpLevel + 1).ToString();
        string text = textObject.GetComponent<TextMeshProUGUI>().text;
        textObject.GetComponent<TextMeshProUGUI>().text = text.Remove(text.Length - 3, 3) + nextCost;
    }
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
            created.AddComponent<PlayerDeck>();
        }
        loaded = true;
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
