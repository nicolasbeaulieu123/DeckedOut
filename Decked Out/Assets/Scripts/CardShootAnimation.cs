using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardShootAnimation : MonoBehaviour
{
    private bool cardShot = false;
    private const float ANIMATION_DURATION = 0.1f;
    private float animationTimer;
    private bool animationComplete = true;

    private GameObject star;
    private Vector3 originalScale;

    private void Awake()
    {
        star = gameObject;
        originalScale = star.transform.localScale;
        if (gameObject.transform.parent.GetComponent<Card>().starCount == 7)
            originalScale = new Vector3(originalScale.x * 2, originalScale.y * 2, originalScale.z);
    }

    void Update()
    {
        if (cardShot)
        {
            star.GetComponent<Transform>().localScale = new Vector3(originalScale.x * 1.3f, originalScale.y * 1.3f, originalScale.z);
            animationComplete = false;
        }
        cardShot = false;
        if (!animationComplete)
            animationTimer -= Time.deltaTime;
        if (animationTimer <= 0)
        {
            animationTimer = ANIMATION_DURATION;
            animationComplete = true;
            star.GetComponent<Transform>().localScale = originalScale;
        }
    }

    public void CardShot()
    {
        cardShot = true;
    }
}
