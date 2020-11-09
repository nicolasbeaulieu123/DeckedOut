using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StarCountUIManager
{
    private static float[][,] jagged_starPosition = 
    {
        new float[,] { { 0, 0 } },
        new float[,] { { 0, 33.5f }, { 0, -33.5f } },
        new float[,] { { 0, 33.5f }, { 0, 0 }, { 0, -33.5f } },
        new float[,] { { -19, 33.5f }, { 19, 33.5f }, { -19, -33.5f }, { 19, -33.5f } },
        new float[,] { { -19, 33.5f }, { 19, 33.5f }, { -19, -33.5f }, { 19, -33.5f }, { 0, 0 } },
        new float[,] { { -19, 33.5f }, { 19, 33.5f }, { -19, -33.5f }, { 19, -33.5f }, { -19, 0 }, { 19, 0 } },
        new float[,] { { 0, 0 } },
    };
    private static readonly float[] scale = { 5, 5 };
    public static void UpdateStarCountUI(GameObject go)
    {
        Card card = go.GetComponent<Card>();
        GameObject starPrefab = Resources.Load<GameObject>("Star");
        if (card.starCount != 7)
        {
            for (int i = 0; i < card.starCount; i++)
            {
                GameObject created = GameObject.Instantiate(starPrefab, go.transform, false);
                float x = jagged_starPosition[card.starCount - 1][i, 0];
                float y = jagged_starPosition[card.starCount - 1][i, 1];
                created.transform.localPosition = new Vector3(x, y, 0);
                created.transform.localScale = new Vector3(scale[0], scale[1], 0);
                created.GetComponent<SpriteRenderer>().color = card.AccentsColor;
                created.AddComponent<CardShoot>();
                created.GetComponent<SpriteRenderer>().sortingLayerName = "Top";
                if (go.GetComponent<Card>().Name == "Rainbow")
                {
                    created.AddComponent<HueShifter>();
                    created.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
        }
        else
        {
            GameObject created = GameObject.Instantiate(starPrefab, go.transform, false);
            created.transform.localPosition = new Vector3(0, 0, 0);
            created.transform.localScale = new Vector3(scale[0] * 2, scale[1] * 2, 0);
            created.GetComponent<SpriteRenderer>().color = card.AccentsColor;
            created.AddComponent<CardShoot>();
            created.GetComponent<SpriteRenderer>().sortingLayerName = "Top";
            if (go.GetComponent<Card>().Name == "Rainbow")
            {
                created.AddComponent<HueShifter>();
                created.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }
}
