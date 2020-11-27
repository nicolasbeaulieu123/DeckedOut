using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateShopGrid : MonoBehaviour
{
    public GameObject CardShopContainer;
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
        for (int i = 0; i < numberToCreate; i++)
        {
            GameObject cardSlot = (GameObject)Instantiate(CardShopContainer, transform);
            GameObject card = Instantiate<GameObject>(cards[i], cardSlot.transform.Find("CardContainer").transform);
            RectTransform cardContainerRectTransform = cardSlot.transform.Find("CardContainer").GetComponent<RectTransform>();
            card.GetComponent<RectTransform>().sizeDelta = new Vector2(cardContainerRectTransform.rect.width, cardContainerRectTransform.rect.height);
            Destroy(card.GetComponent<CardSlot>());
        }
    }
}
