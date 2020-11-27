using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItemHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI priceText;

    Card card;
    private int cardPrice;
    void Start()
    {
        card = gameObject.transform.Find("CardContainer").GetChild(0).GetComponent<Card>();
        cardPrice = card.CardPrice;
        priceText.text = cardPrice.ToString();
    }

    void Update()
    {
        
    }
}
