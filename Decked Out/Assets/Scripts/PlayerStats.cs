using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

	public static int CP;
	public int startCP = 10000;

	public static int Lives;
	public int startLives = 3;

	public static int WaveNumber;

	void Start ()
	{
		CP = 10000;
		Lives = startLives;

		WaveNumber = 1;
	}

}
