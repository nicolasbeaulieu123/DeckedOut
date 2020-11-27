using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumonnerAbility : MonoBehaviour
{
    private static Board board;

    private void Start()
    {
        board = Board.Instance;
    }
    public void SummonNewCard(bool merged, Card oldCard)
    {
        if (!board.IsBoardFull() && merged)
        {
            CreateNewCard(oldCard);
        }
    }

    private void CreateNewCard(Card oldCard)
    {
        bool cardPlaced = false;
        do
        {
            int index = Random.Range(0, board.slots.Length);
            if (!board.isFull[index])
            {
                board.isFull[index] = true;
                cardPlaced = true;
                GameObject created = Instantiate(PlayerDeck.Deck()[Random.Range(1, 2)], board.slots[index].transform, false);
                created.transform.localScale = new Vector3(1.4f, 1.4f, 0);
                created.AddComponent<DragDrop>();
                created.GetComponent<DragDrop>().canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
                Card cardType = GameObject.Find("Deck").transform.Find(created.name).GetComponent<Card>();
                for (int i = 1; i < cardType.PowerUpLevel; i++)
                    created.GetComponent<Card>().PowerUpCard();
                created.GetComponent<Card>().starCount = Random.Range(1, oldCard.starCount - 1);
                StarCountUIManager.UpdateStarCountUI(created);
                created.tag = "CardOnBoard";
            }
        } while (!cardPlaced);
    }
}
