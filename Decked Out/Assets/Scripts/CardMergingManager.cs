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

    public void CardsMerged(GameObject selfParent, GameObject targetParent)
    {
        GameObject card1;
        GameObject card2;
        if (selfParent == targetParent)
        {
            card1 = selfParent.transform.GetChild(1).gameObject;
            card2 = targetParent.transform.GetChild(0).gameObject;
        }
        else
        {
            card1 = selfParent.transform.GetChild(0).gameObject;
            card2 = targetParent.transform.GetChild(0).gameObject;
        }
        if (card1.GetComponent<Card>().name.Contains("Rainbow") && !card2.GetComponent<Card>().name.Contains("Rainbow"))
            DestroyImmediate(card1.GetComponent<DragDrop>().GetLastParent().transform.GetChild(1).gameObject);
        else
            DestroyImmediate(card1.GetComponent<DragDrop>().GetLastParent().transform.GetChild(0).gameObject);
        GameObject newCard = GenerateNewCard(targetParent.transform);
        UpgradeCardStarCount(newCard, card1);

        bool canDeleteCard1 = true;
        bool canDeleteCard2 = true;

        if (card1.GetComponent<Card>().Type == Type.Merge)
        {
            if (card1.GetComponent<Card>().Name == "Sacrifice")
                card1.GetComponent<Card>().TryActivateAbility(null, true);
            else if (card1.GetComponent<Card>().Name == "Summoner")
                card1.GetComponent<Card>().TryActivateAbility(null, true, newCard);
            else if (card1.GetComponent<Card>().Name == "Rainbow" && !(card1.GetComponent<Card>().Name == "Rainbow" && card2.GetComponent<Card>().Name == "Rainbow"))
            {
                card1.GetComponent<Card>().TryActivateAbility(null, true, card2);
                card1.transform.SetParent(card1.GetComponent<DragDrop>().GetLastParent().transform);
                Destroy(newCard);
                canDeleteCard2 = false;
            }
            else if (card1.GetComponent<Card>().Name == "Boost")
            {
                card2.GetComponent<Card>().starCount++;
                StarCountUIManager.UpdateStarCountUI(card2);
                Destroy(newCard);
                canDeleteCard2 = false;
            }
        }
        if (card2.GetComponent<Card>().Type == Type.Merge)
        {
            if (card1.GetComponent<Card>().Name == "Sacrifice")
                card1.GetComponent<Card>().TryActivateAbility(null, true);
        }
        
        if (canDeleteCard1)
            Destroy(card1);
        if (canDeleteCard2)
            Destroy(card2);
        if (newCard != null)
            StarCountUIManager.UpdateStarCountUI(newCard);
        CardMergingManager.Instance.ResetCardsOpacity();
    }

    private GameObject GenerateNewCard(Transform parent)
    {
        Board board = GameObject.FindGameObjectWithTag("Board").GetComponent<Board>();
        GameObject created = Instantiate(PlayerDeck.Deck()[Random.Range(0, 5)], board.slots[Board.FindSlotIdFromName(parent.name) - 1].transform, false);
        created.transform.localScale = new Vector3(1.4f, 1.4f, 0);
        created.AddComponent<DragDrop>();
        created.GetComponent<DragDrop>().canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        created.tag = "CardOnBoard";
        return created;
    }
    private void UpgradeCardStarCount(GameObject newCard, GameObject oldCard)
    {
        newCard.GetComponent<Card>().starCount = oldCard.GetComponent<Card>().starCount + 1;
    }

    public void ShowAvailableCardsForMerging(GameObject selectedCardObject)
    {
        List<GameObject> cardsOnBoard = Board.Instance.AllCardsOnBoard();

        foreach (GameObject cardObject in cardsOnBoard)
        {
            if (cardObject != null)
            {
                Card card = cardObject.GetComponent<Card>();
                Card selectedCard = selectedCardObject.GetComponent<Card>();
                if (!(card.name == selectedCard.name && card.starCount == selectedCard.starCount) && !(selectedCard.name.Contains("Rainbow") && card.starCount == selectedCard.starCount) && !(selectedCard.name.Contains("Boost") && card.starCount == selectedCard.starCount))
                {
                    for (int i = 0; i < cardObject.transform.childCount; i++)
                    {
                        Color tempColor = cardObject.transform.GetChild(i).GetComponent<SpriteRenderer>().color;
                        tempColor.a = 0.1f;
                        cardObject.transform.GetChild(i).GetComponent<SpriteRenderer>().color = tempColor;
                    }
                    card.GetComponent<CanvasGroup>().alpha = 0.3f;
                }
            }
        }
    }
    public void ResetCardsOpacity()
    {
        List<GameObject> cardsOnBoard = Board.Instance.AllCardsOnBoard();
        foreach (GameObject card in cardsOnBoard)
            if (card != null)
            {
                for (int i = 0; i < card.transform.childCount; i++)
                {
                    Color tempColor = card.transform.GetChild(i).GetComponent<SpriteRenderer>().color;
                    tempColor.a = 100;
                    card.transform.GetChild(i).GetComponent<SpriteRenderer>().color = tempColor;
                }
                card.GetComponent<CanvasGroup>().alpha = 1;
            }
    }
}
