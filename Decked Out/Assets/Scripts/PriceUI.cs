using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PriceUI : MonoBehaviour
{
 	[SerializeField]
	public TextMeshProUGUI PriceText;

	void Update () {
		PriceText.text = PlaceCard.cardPrice.ToString() + " CP";
	}
}
