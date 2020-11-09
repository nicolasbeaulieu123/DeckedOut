using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SacrificeAbility : MonoBehaviour
{
    public void AddCPFromMerge(bool merged)
    {
        if (merged)
            PlayerStats.CP += (int)(gameObject.GetComponent<Card>().starCount * gameObject.GetComponent<Card>().actualAbility);
    }
}
