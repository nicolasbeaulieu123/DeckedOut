using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSilencerAbility : MonoBehaviour
{
    [SerializeField] private float BaseAbilityCoolDown;
    [SerializeField] private GameObject pfAbilityAnimation;
    [SerializeField] private GameObject pfAbilityCardAnimation;
    private EnemyWaveManager waveManager;
    private Enemy Boss;
    private float abilityCooldown;
    private bool animationPlayed = false;
    private float animationCooldown = 2f;
    float silenceCardsCooldown = 1f;
    private Vector3 spawnPosition = new Vector3(-299.95f, -260.5f);

    List<GameObject> animations;
    List<GameObject> notSilencedCards;

    void Start()
    {
        abilityCooldown = 3f;
        waveManager = GameObject.Find("EnemyWaveManager").GetComponent<EnemyWaveManager>();
        Boss = gameObject.GetComponent<Enemy>();
        animations = new List<GameObject>();
    }
    GameObject pe;
    void Update()
    {
        if (this != null)
        {
            abilityCooldown -= Time.deltaTime;
            notSilencedCards = Board.Instance.AllCardsOnBoard().FindAll(card => card.GetComponent<Card>().canShoot);
            if (abilityCooldown < 0 && notSilencedCards.Count > 0)
            {
                silenceCardsCooldown -= Time.deltaTime;
                Boss.Stopped = true;
                if (!animationPlayed)
                {
                    animationPlayed = true;
                    animationCooldown -= Time.deltaTime;
                    pe = Instantiate(pfAbilityAnimation);
                    pe.transform.position = new Vector3(Boss.GetPosition().x, Boss.GetPosition().y, 0);
                    pe.transform.SetParent(GameObject.Find("Animations").transform, true);
                    SoundManager.PlaySound(GameAssets.Instance.Boss_Silencer_Charge);
                }
                if (silenceCardsCooldown < 0)
                {
                    ActivateAbility();
                    silenceCardsCooldown = 1f;
                    abilityCooldown = BaseAbilityCoolDown + pfAbilityAnimation.GetComponent<ParticleSystem>().main.duration;
                    animationPlayed = false;
                }
            }
            if (animationCooldown != 2f)
                animationCooldown -= Time.deltaTime;
            if (animationCooldown < 0)
            {
                animationCooldown = 2f;
                Destroy(pe);
                Boss.Stopped = false;
            }
        }
    }
    void ActivateAbility()
    {
        int silencedCards = 0;

        do
        {
            int index = Random.Range(0, notSilencedCards.Count);
            if (notSilencedCards.Count == 0)
                silencedCards = 2;
            else
            {
                notSilencedCards[index].GetComponent<Card>().canShoot = false;
                notSilencedCards[index].GetComponent<Card>().canMerge = false;
                GameObject peCard = Instantiate(pfAbilityCardAnimation);
                peCard.transform.position = new Vector3(notSilencedCards[index].transform.position.x, notSilencedCards[index].transform.position.y, 45);
                peCard.transform.SetParent(GameObject.Find("Animations").transform, true);
                animations.Add(peCard);
                notSilencedCards.RemoveAt(index);
                silencedCards++;
            }
        } while (silencedCards != 2);
    }

    public void IsDead()
    {
        foreach (GameObject card in Board.Instance.AllCardsOnBoard())
        {
            card.GetComponent<Card>().canMerge = true;
            card.GetComponent<Card>().canShoot = true;
        }
        for (int i = 0; i < animations.Count; i++)
        {
            Destroy(animations[i]);
        }
        animations.Clear();
    }
}
