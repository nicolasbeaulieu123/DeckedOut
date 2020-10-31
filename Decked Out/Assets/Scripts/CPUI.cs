using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CPUI : MonoBehaviour
{
 	[SerializeField]
	public Text CPText;

	void Update () {
		CPText.text = PlayerStats.CP.ToString() + " CP";
	}
}
