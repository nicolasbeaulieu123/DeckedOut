using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelAbility : MonoBehaviour
{
    public GameObject pfAngelReviveAnimation;

    public void TryExtraLifeAbility()
    {
        float reveiveChance = gameObject.GetComponent<Card>().actualAbility;
        if (Random.Range(0, 1000) < reveiveChance * 10 && PlayerStats.Lives < 5)
        {
            GameObject pe = Instantiate(pfAngelReviveAnimation, gameObject.transform.position, gameObject.transform.rotation);
            pe.transform.SetParent(GameObject.Find("Animations").transform);
            PlayerStats.Lives++;
        }
    }
}
