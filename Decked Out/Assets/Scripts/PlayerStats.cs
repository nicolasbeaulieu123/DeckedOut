using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

	public static int CP;
	public int startCP = 100;

	public static int Lives;
	public int startLives = 3;

	public static int WaveNumber;

	void Start ()
	{
		CP = startCP;
		Lives = startLives;

		WaveNumber = 1;
	}

}
