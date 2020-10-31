using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesUI : MonoBehaviour {
	[SerializeField]
	public Text livesText;

	// Update is called once per frame
	void Update () {
		livesText.text = PlayerStats.Lives.ToString() + " LIVES";
	}
}
