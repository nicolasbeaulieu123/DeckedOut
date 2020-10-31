using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PriceUI : MonoBehaviour
{
 	[SerializeField]
	public Text PriceText;

	void Update () {
		PriceText.text = PlaceCard.cardPrice.ToString() + " CP";
	}
}
