using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAbility : MonoBehaviour
{
    private Card card;
    private static float actualAbility = 10;
    private static float attackSpeed;
    void Awake()
    {
        card = gameObject.GetComponent<Card>();
        attackSpeed = card.BaseAttackSpeed * (1 - (actualAbility / 100));
        card.actualAttackSpeed = attackSpeed;
    }

    private void Update()
    {
        if (card.actualAbility > actualAbility)
        {
            actualAbility = card.actualAbility;
            attackSpeed = card.BaseAttackSpeed * (1 - (actualAbility / 100));
        }
        if (card.actualAttackSpeed != attackSpeed)
            card.actualAttackSpeed = attackSpeed;
    }
}
