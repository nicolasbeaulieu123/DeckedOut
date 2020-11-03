using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMergingManager : MonoBehaviour
{
    public static CardMergingManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void CardsMerged(GameObject parent)
    {
        GameObject card1 = parent.transform.GetChild(0).gameObject;
        GameObject card2 = parent.transform.GetChild(1).gameObject;
        GameObject newCard = GenerateNewCard(parent.transform);
        UpgradeCardStarCount(newCard, card1);
        StarCountUIManager.UpdateStarCountUI(newCard);
        Destroy(card1);
        Destroy(card2);
    }

    private GameObject GenerateNewCard(Transform parent)
    {
        Board board = GameObject.FindGameObjectWithTag("Board").GetComponent<Board>();
        GameObject created = Instantiate(PlayerDeck.Deck()[Random.Range(0, 5)], board.slots[Board.FindSlotIdFromName(parent.name) - 1].transform, false);
        created.transform.localScale = new Vector3(0.4f, 0.4f, 1);
        created.AddComponent<DragDrop>();
        created.GetComponent<DragDrop>().canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        return created;
    }
    private void UpgradeCardStarCount(GameObject newCard, GameObject oldCard)
    {
        newCard.GetComponent<Card>().starCount = oldCard.GetComponent<Card>().starCount + 1;
    }

    public void ShowAvailableCardsForMerging(GameObject selectedCardObject)
    {
        GameObject[] cardsOnBoard = Board.Instance.AllCardsOnBoard();

        foreach (GameObject cardObject in cardsOnBoard)
        {
            if (cardObject != null)
            {
                Card card = cardObject.GetComponent<Card>();
                Card selectedCard = selectedCardObject.GetComponent<Card>();
                if (!(card.name == selectedCard.name && card.starCount == selectedCard.starCount))
                    card.GetComponent<CanvasGroup>().alpha = 0.3f;
            }
        }
    }
    public void ResetCardsOpacity()
    {
        GameObject[] cardsOnBoard = Board.Instance.AllCardsOnBoard();
        foreach (GameObject card in cardsOnBoard)
            if (card != null)
                card.GetComponent<CanvasGroup>().alpha = 1;
    }
}
