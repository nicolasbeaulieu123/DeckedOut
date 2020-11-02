using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public bool[] isFull;
    public GameObject[] slots;
    public static Board Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public static int FindSlotIdFromName(string name)
    {
        string temp = string.Empty;
        for (int i = 0; i < name.Length; i++)
        {
            if (char.IsDigit(name[i]))
                temp += name[i];
        }
        if (temp.Length > 0)
            return int.Parse(temp);
        return -1;
    }

    public GameObject[] AllCardsOnBoard()
    {
        GameObject[] cards = new GameObject[15];
        int i = 0;
        foreach (GameObject slot in slots)
        {
            if (slot.transform.childCount == 1)
            {
                cards[i] = slot.transform.GetChild(0).gameObject;
            }
            i++;
        }
        return cards;
    }
}
