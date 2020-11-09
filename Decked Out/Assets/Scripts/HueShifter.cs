using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HueShifter : MonoBehaviour
{
    public float Speed = 0.5f;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        rend.material.SetColor("_Color", Color.HSVToRGB(Mathf.PingPong(Time.time * Speed, 1), 1, 1));
    }
}
