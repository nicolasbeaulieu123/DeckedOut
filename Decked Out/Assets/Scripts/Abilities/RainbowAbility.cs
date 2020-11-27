using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowAbility : MonoBehaviour
{
    public void CopyTargetCard(bool merged, Card targetCard)
    {
        if (merged && targetCard != null)
        {
            CreateCopy(targetCard);
        }
    }

    private void CreateCopy(Card targetCard)
    {
        Board board = GameObject.FindGameObjectWithTag("Board").GetComponent<Board>();
        bool cardPlaced = false;
        do
        {
            int cardDeckIndex = Random.Range(0, 5);
            if (targetCard.name.Contains(PlayerDeck.Deck()[cardDeckIndex].name))
            {
                cardPlaced = true;
                GameObject created = Instantiate(PlayerDeck.Deck()[cardDeckIndex], board.slots[Board.FindSlotIdFromName(gameObject.transform.parent.name) - 1].transform, false);
                created.transform.localScale = new Vector3(1.4f, 1.4f, 0);
                created.AddComponent<DragDrop>();
                created.GetComponent<DragDrop>().canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
                Card cardType = GameObject.Find("Deck").transform.Find(created.name).GetComponent<Card>();
                for (int i = 1; i < cardType.PowerUpLevel; i++)
                    created.GetComponent<Card>().PowerUpCard();
                created.GetComponent<Card>().starCount = targetCard.starCount;
                StarCountUIManager.UpdateStarCountUI(created);
                created.tag = "CardOnBoard";
            }
        } while (!cardPlaced);
    }
}
