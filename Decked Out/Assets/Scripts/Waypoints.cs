using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public Transform[] waypoints;
    public static Waypoints Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        
    }
}
