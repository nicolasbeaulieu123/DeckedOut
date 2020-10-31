using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveCounter : MonoBehaviour
{
 	[SerializeField]
	public Text WaveCounterText;

	void Update () {
		WaveCounterText.text = PlayerStats.WaveNumber.ToString();
	}
}
