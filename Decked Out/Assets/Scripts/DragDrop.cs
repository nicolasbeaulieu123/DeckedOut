using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerUpHandler
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
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!eventData.pointerEnter.name.Contains("Slot") && !eventData.pointerEnter.name.Contains("(Clone)"))
        {
            eventData.pointerDrag.transform.SetParent(lastParent.transform);
            eventData.pointerDrag.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        lastParent = eventData.pointerDrag.transform.parent.gameObject;
        eventData.pointerDrag.transform.SetParent(GameObject.Find("Board").transform);
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
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
