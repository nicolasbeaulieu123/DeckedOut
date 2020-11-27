using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CPUI : MonoBehaviour
{
 	[SerializeField]
	public TextMeshProUGUI CPText;

	void Update () {
		CPText.text = PlayerStats.CP.ToString() + " CP";
	}
}
