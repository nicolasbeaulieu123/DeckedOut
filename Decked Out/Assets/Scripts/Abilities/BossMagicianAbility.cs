using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMagicianAbility : MonoBehaviour
{
    [SerializeField] private float BaseAbilityCoolDown;
    [SerializeField] private GameObject pfAbilityAnimationHeal;
    [SerializeField] private GameObject pfAbilityAnimationDestroy;
    [SerializeField] private GameObject pfAbilityCardAnimation;
    private EnemyWaveManager waveManager;
    private Enemy Boss;
    private float abilityCooldown;
    private bool animationPlayed = false;
    float abilityOnCardCooldown = 0.5f;
    private Vector3 spawnPosition = new Vector3(-299.95f, -260.5f);
    int animationCycle = 0;
    void Start()
    {
        abilityCooldown = 3f;
        waveManager = GameObject.Find("EnemyWaveManager").GetComponent<EnemyWaveManager>();
        Boss = gameObject.GetComponent<Enemy>();
    }
    GameObject pe;
    void Update()
    {
        if (this != null)
        {
            abilityCooldown -= Time.deltaTime;
            if (abilityCooldown < 0)
            {
                abilityOnCardCooldown -= Time.deltaTime;
                Boss.Stopped = true;
                if (animationCycle == 0)
                    DestroyAbilityAnim();
                if (abilityOnCardCooldown < 0)
                {
                    ActivateAbility();
                    abilityOnCardCooldown = 0.5f;
                    abilityCooldown = BaseAbilityCoolDown + pfAbilityAnimationHeal.GetComponent<ParticleSystem>().main.duration;
                    animationPlayed = false;
                }
            }
            if (pe == null)
                Boss.Stopped = false;
        }
    }
    void ActivateAbility()
    {
        if (animationCycle == 0)
            DestroyAbility();
        else
            HealAbility();
        animationCycle = animationCycle == 1 ? 0 : animationCycle + 1;
    }

    void HealAbility()
    {
        if (!animationPlayed)
        {
            animationPlayed = true;
            pe = Instantiate(pfAbilityAnimationHeal);
            pe.transform.position = new Vector3(Boss.GetPosition().x, Boss.GetPosition().y, 0);
            pe.transform.SetParent(GameObject.Find("Animations").transform, true);
            Boss.health = (int)(Boss.health + Boss.health * 0.5f);
        }
    }
    void DestroyAbilityAnim()
    {
        if (!animationPlayed)
        {
            animationPlayed = true;
            pe = Instantiate(pfAbilityAnimationDestroy);
            pe.transform.position = new Vector3(Boss.GetPosition().x, Boss.GetPosition().y, 0);
            pe.transform.SetParent(GameObject.Find("Animations").transform, true);
        }
    }
    void DestroyAbility()
    {
        List<GameObject> cards = Board.Instance.AllCardsOnBoard();
        if (abilityOnCardCooldown < 0 && cards.Count > 0)
        {
            GameObject card = cards[Random.Range(0, cards.Count)];
            GameObject destroyCardAnim = Instantiate(pfAbilityCardAnimation);
            destroyCardAnim.transform.position = new Vector3(card.transform.position.x, card.transform.position.y, 0);
            destroyCardAnim.transform.SetParent(GameObject.Find("Animations").transform, true);
            Destroy(card);
        }
    }
}
