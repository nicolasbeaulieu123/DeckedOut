using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private GameObject pfDamagePopup;
    public static DamagePopup Create(Vector3 position, float damageAmount, bool isCriticalHit, string AbilityDamageColor)
    {
        Transform damagePopupTransform = Instantiate(GameAssets.Instance.pfDamagePopup.transform, position, Quaternion.identity);
        damagePopupTransform.SetParent(GameObject.Find("DamagePopups").transform);
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount, isCriticalHit, AbilityDamageColor);

        return damagePopup;
    }

    private static int sortingOrder;

    private const float DISAPPEAR_TIMER_MAX = 0.5f;
    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private Vector3 moveVector;
    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }
    public void Setup(float damageAmount, bool isCriticalHit, string abilityDamageColor)
    {
        textMesh.SetText(damageAmount.ToString());
        if (!isCriticalHit)
        {
            textMesh.fontSize = 130;
            textColor = UtilsClass.GetColorFromString("FFCF00");
        }
        else
        {
            textMesh.fontSize = 140;
            textColor = UtilsClass.GetColorFromString("FF2B00");
        }
        if (abilityDamageColor != "000000")
            textColor = UtilsClass.GetColorFromString(abilityDamageColor);
        textMesh.color = textColor;
        disappearTimer = DISAPPEAR_TIMER_MAX;

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;

        moveVector = new Vector3(Random.Range(-10, 10), Random.Range(0, 15)) * 30f;
    }

    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 8f * Time.deltaTime;

        if (disappearTimer > DISAPPEAR_TIMER_MAX)
        {
            // First half of popup
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        disappearTimer -= Time.deltaTime;

        if (disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
                Destroy(gameObject);
        }
    }
}
