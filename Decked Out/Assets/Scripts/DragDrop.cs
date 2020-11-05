using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public static DragDrop Instance { get; private set; }

    [SerializeField] public Canvas canvas;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Card card;
    private static GameObject lastParent;
    private void Awake()
    {
        Instance = this;

        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        lastParent = eventData.pointerEnter.transform.parent.gameObject;
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        eventData.pointerDrag.GetComponent<Canvas>().overrideSorting = true;
        eventData.pointerDrag.GetComponent<Canvas>().sortingLayerName = "Top";
        canvasGroup.alpha = 0.8f;
        canvasGroup.blocksRaycasts = false;
        CardMergingManager.Instance.ShowAvailableCardsForMerging(eventData.pointerDrag.gameObject);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        eventData.pointerDrag.GetComponent<Canvas>().overrideSorting = false;
        if (!eventData.pointerEnter.name.Contains("Slot") && !eventData.pointerEnter.name.Contains("(Clone)"))
        {
            eventData.pointerDrag.gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
        CardMergingManager.Instance.ResetCardsOpacity();
    }

    public void SetCard(Card card)
    {
        this.card = card;
    }

    public Card GetCard()
        => card;
    public GameObject GetLastParent()
        => lastParent;
}
