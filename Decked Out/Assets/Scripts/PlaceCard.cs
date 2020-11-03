using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceCard : MonoBehaviour
{
    private Board board;

    public static int cardPrice = 10;

    public void Start()
    {
        board = GameObject.FindGameObjectWithTag("Board").GetComponent<Board>();
        PlayerDeck.LoadPlayerDeckImages();
    }

    public void OnTriggerButton()
    {
        if (PlayerStats.CP >= cardPrice)
        {
            if (!isBoardFull())
            {
                bool cardPlaced = false;
                do
                {
                    int index = Random.Range(0, board.slots.Length);
                    if (!board.isFull[index])
                    {
                        board.isFull[index] = true;
                        cardPlaced = true;
                        buyCard();
                        GameObject created = Instantiate(PlayerDeck.Deck()[Random.Range(0, 5)], board.slots[index].transform, false);
                        created.transform.localScale = new Vector3(0.4f, 0.4f, 1);
                        created.AddComponent<DragDrop>();
                        created.GetComponent<DragDrop>().canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
                        Card cardType = GameObject.Find("Deck").transform.Find(created.name).GetComponent<Card>();
                        for (int i = 1; i < cardType.PowerUpLevel; i++)
                            created.GetComponent<Card>().PowerUpCard();
                        StarCountUIManager.UpdateStarCountUI(created);
                    }
                } while (!cardPlaced);
            }
        }
    }

    void buyCard()
    {
        PlayerStats.CP -= cardPrice;
        cardPrice += 10;
    }

    bool isBoardFull()
    {
        for (int i = 0; i < board.slots.Length; i++)
        {
            if (!board.isFull[i])
                return false;
        }
        return true;
    }
}
