using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardsHandler : MonoBehaviour, IPointerDownHandler
{
    public GameObject[] CardsPrefabs;
    public GameObject CardMenu;
    public GameObject Deck;
    static GameObject lastCardClicked = null;

    private static List<GameObject> selectedCards = new List<GameObject>();

    private static bool loaded = false;

    void Start()
    {
        CardsPrefabs = Resources.LoadAll<GameObject>("Cards");

        selectedCards.RemoveAll(item => item == null);
        loadDeckFromPrefs();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Card"))
        {
            if (!selectedCards.Any(c => c.name.Contains(obj.name)))
                selectedCards.Add(obj);
        }
    }

    public static void ChangedScene()
    {
        loaded = false;
    }
    public void ChooseCard()
    {
        if (!isCardAlreadyInDeck(gameObject.name))
        {
            lastCardClicked = gameObject;
            toggleReplaceMenu();
            GameObject.Find("DeckMenu").transform.Find("Button CancelSelect").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("DeckMenu").transform.Find("Button CancelSelect").gameObject.GetComponent<Button>().onClick.AddListener(delegate { CancelChooseCard(); });
        }
        else
            lastCardClicked = null;
    }
    private void CancelChooseCard()
    {
        lastCardClicked = null;
        toggleReplaceMenu();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.tag == "Card" && lastCardClicked != null)
        {
            if (!selectedCards.Any(c => c.name.Contains(lastCardClicked.name)))
            {
                replaceCard(eventData);
                saveDeck();
                toggleReplaceMenu();
            }
        }
        else if (!CardMenu.activeInHierarchy && eventData.pointerCurrentRaycast.gameObject.tag == "AcquiredCard")
        {
            showCardInfo();
        }

        lastCardClicked = null;
        selectedCards.RemoveAll(item => item == null);
    }

    private void saveDeck()
    {
        int i = 1;
        foreach(GameObject card in selectedCards)
        {
            PlayerPrefs.SetString("Card" + i, card.name);
            i++;
        }
        PlayerPrefs.Save();
    }

    private void loadDeckFromPrefs()
    {
        if (!loaded)
        {
            for (int i = 1; i <= 5; i++)
            {
                GameObject oldCard = GameObject.Find("Card" + i);
                float x = oldCard.GetComponent<RectTransform>().position.x;
                float y = oldCard.GetComponent<RectTransform>().position.y;

                GameObject toCreate = null;
                foreach (GameObject card in CardsPrefabs)
                {
                    if (card.name.Contains(PlayerPrefs.GetString("Card" + i)))
                        toCreate = card;
                }

                Destroy(oldCard);
                GameObject createdCard = createCardInDeck(toCreate, x, y);
                selectedCards.Add(createdCard);
            }
            loaded = true;
        }
    }
    private GameObject createCardInDeck(GameObject toCreate, float x, float y)
    {
        GameObject createdCard = Instantiate(toCreate, new Vector2(x, y), Quaternion.identity);
        createdCard.transform.SetParent(Deck.transform);
        createdCard.name = toCreate.name;

        if (createdCard.GetComponent<Image>().sprite.name != "blank")
            createdCard.GetComponent<Image>().color = Color.HSVToRGB(0, 0, 100);
        createdCard.AddComponent<CardsHandler>();
        createdCard.GetComponent<CardsHandler>().CardMenu = GameObject.Find("Canvas").transform.Find("CardMenu").gameObject;
        createdCard.GetComponent<CardsHandler>().Deck = GameObject.FindGameObjectWithTag("Deck");
        createdCard.tag = "Card";
        return createdCard;
    }
    private void showCardInfo()
    {
        CardMenu.SetActive(true);
        Card card = gameObject.GetComponent<Card>();
        GameObject.Find("CardName").GetComponent<TextMeshProUGUI>().text = card.Name;
        Image cardImage = GameObject.Find("CardImage").GetComponent<Image>();
        GameObject.Find("CardDescription").GetComponent<TextMeshProUGUI>().text = card.CardDescription();
        GameObject.Find("ATK").GetComponent<TextMeshProUGUI>().text = card.CardAtk();
        GameObject.Find("TYPE").GetComponent<TextMeshProUGUI>().text = card.CardType();
        GameObject.Find("ATKSPEED").GetComponent<TextMeshProUGUI>().text = card.CardAtkSpeed();
        GameObject.Find("TARGET").GetComponent<TextMeshProUGUI>().text = card.CardTarget();
        GameObject.Find("ABILITY").GetComponent<TextMeshProUGUI>().text = card.AbilityDescription;
        GameObject.Find("ABILITYDMG").GetComponent<TextMeshProUGUI>().text = card.CardAbilityDmg();
        if (card.GetComponent<Image>().sprite.name == "blank")
        {
            cardImage.color = card.GetComponent<Image>().color;
            cardImage.sprite = card.GetComponent<Image>().sprite;
        }
        else
        {
            cardImage.sprite = card.GetComponent<Image>().sprite;
            cardImage.color = Color.HSVToRGB(0, 0, 100);
        }
        GameObject.Find("Canvas").transform.Find("CardMenu").transform.Find("Canvas").transform.Find("UseButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("Canvas").transform.Find("CardMenu").transform.Find("Canvas").transform.Find("UseButton").gameObject.GetComponent<Button>().onClick.AddListener(delegate { ChooseCard(); });
    }

    private bool isCardAlreadyInDeck(string CardName)
        => selectedCards.Any(c => c.name.Contains(CardName.Replace("(Clone)", "")));

    private void toggleReplaceMenu()
    {
        GameObject acquiredCardsList = GameObject.Find("DeckMenu").transform.Find("AcquiredCards").gameObject;
        GameObject acquiredCardsText = GameObject.Find("DeckMenu").transform.Find("Text AcquiredCards").gameObject;
        GameObject selectACardText = GameObject.Find("DeckMenu").transform.Find("Text SelectACard").gameObject;
        GameObject cancelSelectButton = GameObject.Find("DeckMenu").transform.Find("Button CancelSelect").gameObject;
        acquiredCardsList.SetActive(!acquiredCardsList.activeInHierarchy);
        acquiredCardsText.SetActive(!acquiredCardsText.activeInHierarchy);
        selectACardText.SetActive(!selectACardText.activeInHierarchy);
        cancelSelectButton.SetActive(!cancelSelectButton.activeInHierarchy);
    }

    private GameObject cardToCreate(string CardName)
    {
        GameObject toCreate = null;
        foreach (GameObject card in CardsPrefabs)
        {
            if (CardName.Contains(card.name))
                toCreate = card;
        }
        return toCreate;
    }

    private void replaceCard(PointerEventData eventData)
    {
        GameObject oldCard = GameObject.Find(eventData.pointerCurrentRaycast.gameObject.name);
        float x = oldCard.GetComponent<RectTransform>().position.x;
        float y = oldCard.GetComponent<RectTransform>().position.y;

        GameObject toCreate = cardToCreate(lastCardClicked.name);

        if (!selectedCards.Any(c => c.name.Contains(toCreate.name)))
        {
            int index = selectedCards.FindIndex(a => a.name == oldCard.name);
            Destroy(oldCard);
            GameObject createdCard = createCardInDeck(toCreate, x, y);
            selectedCards[index] = createdCard;
        }
    }
}
