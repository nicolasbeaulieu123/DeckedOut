using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossJokerAbility : MonoBehaviour
{
    [SerializeField] private float BaseAbilityCoolDown;
    [SerializeField] private GameObject pfAbilityAnimation;
    [SerializeField] private GameObject pfAbilityCardAnimation;
    private EnemyWaveManager waveManager;
    private Enemy Boss;
    private float abilityCooldown;
    private bool animationPlayed = false;
    float changeCardsCooldown = 1f;
    private Vector3 spawnPosition = new Vector3(-299.95f, -260.5f);

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
                changeCardsCooldown -= Time.deltaTime;
                Boss.Stopped = true;
                if (!animationPlayed) 
                {
                    animationPlayed = true;
                    pe = Instantiate(pfAbilityAnimation);
                    pe.transform.position = new Vector3(Boss.GetPosition().x, Boss.GetPosition().y, 45);
                    pe.transform.SetParent(GameObject.Find("Animations").transform, true);
                    SoundManager.PlaySound(GameAssets.Instance.Boss_Knight_Charge);
                }
                if (changeCardsCooldown < 0)
                {
                    ActivateAbility();
                    changeCardsCooldown = 1f;
                    abilityCooldown = BaseAbilityCoolDown + pfAbilityAnimation.GetComponent<ParticleSystem>().main.duration;
                    animationPlayed = false;
                }
            }
            if (pe == null)
                Boss.Stopped = false;
        }
    }
    void ActivateAbility()
    {
        List<GameObject> cards = Board.Instance.AllCardsOnBoard();
        SoundManager.PlaySound(GameAssets.Instance.Boss_Knight_Poof);
        for (int i = 0; i < cards.Count; i++)
        {
            Card card = cards[i].GetComponent<Card>();
            int oldStartCount = card.starCount;
            Transform oldPosition = cards[i].transform;

            GameObject cardAnim = Instantiate(pfAbilityCardAnimation);
            cardAnim.transform.position = new Vector3(oldPosition.position.x, oldPosition.position.y, 45);
            cardAnim.transform.SetParent(GameObject.Find("Animations").transform, true);

            GameObject created = Instantiate(PlayerDeck.Deck()[Random.Range(0, 5)], oldPosition.parent, false);
            created.transform.localScale = new Vector3(1.4f, 1.4f, 0);
            created.GetComponent<Card>().starCount = oldStartCount;
            created.AddComponent<DragDrop>();
            created.GetComponent<DragDrop>().canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            Card cardType = GameObject.Find("Deck").transform.Find(created.name).GetComponent<Card>();
            for (int j = 1; j < cardType.PowerUpLevel; j++)
                created.GetComponent<Card>().PowerUpCard();
            StarCountUIManager.UpdateStarCountUI(created);
            created.tag = "CardOnBoard";
            Destroy(card.gameObject);
        }
    }
}
