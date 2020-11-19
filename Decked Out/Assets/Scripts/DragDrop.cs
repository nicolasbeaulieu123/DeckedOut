using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public static DragDrop Instance { get; private set; }

    [SerializeField] public Canvas canvas;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Card card;
    private static GameObject lastParent;

    private GameObject ghostCard;
    private GameObject originalCard;
    private void Awake()
    {
        Instance = this;

        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<Card>().canMerge)
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<Card>().canMerge)
        {
            lastParent = eventData.pointerDrag.transform.parent.gameObject;
            originalCard = eventData.pointerDrag.gameObject;
            ghostCard = CreateGhostCard(originalCard);
            eventData.pointerDrag.GetComponent<Canvas>().overrideSorting = true;
            eventData.pointerDrag.GetComponent<Canvas>().sortingLayerName = "Top";
            canvasGroup.alpha = 0.8f;
            canvasGroup.blocksRaycasts = false;
            CardMergingManager.Instance.ShowAvailableCardsForMerging(eventData.pointerDrag.gameObject);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerDrag.GetComponent<Card>().canMerge)
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            eventData.pointerDrag.GetComponent<Canvas>().overrideSorting = false;
            if (eventData.pointerEnter != null)
            {
                if (!eventData.pointerEnter.name.Contains("(Clone)"))
                {
                    eventData.pointerDrag.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                }
            }
            else
                eventData.pointerDrag.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            CardMergingManager.Instance.ResetCardsOpacity();
            Destroy(ghostCard);
            ghostCard = null;
            originalCard = null;
        }
    }

    private GameObject CreateGhostCard(GameObject card)
    {
        int indexOfCard = GetCardIndexInDeck(card);
        GameObject created = Instantiate(PlayerDeck.Deck()[indexOfCard], UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
        created.transform.SetParent(card.transform.parent.transform);
        created.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        created.GetComponent<RectTransform>().localScale = new Vector3(0.4f, 0.4f, 0);
        created.GetComponent<CanvasGroup>().alpha = 0.6f;
        for (int i = created.transform.childCount; i > 0; i++)
        {
            Destroy(created.GetComponent<CardShoot>());
        }
        return created;
    }

    private int GetCardIndexInDeck(GameObject card)
    {
        for (int i = 0; i < PlayerDeck.Deck().Length; i++)
        {
            if (card.name.Replace("(Clone)", "") == PlayerDeck.Deck()[i].name)
                return i;
        }
        return -1;
    }

    public GameObject GetLastParent()
        => lastParent;
}
