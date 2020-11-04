using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            Transform parent = DragDrop.Instance.GetLastParent().transform;
            Vector3 position = Vector3.zero;
            bool cardsMerged = false;

            if (GetTargetCard(gameObject) != null)
            {
                Card self = eventData.pointerDrag.GetComponent<Card>();
                Card target = GetTargetCard(gameObject);
                if (self.name == target.name && self.starCount == target.starCount && self.starCount < 7 && self.tag == target.tag)
                {
                    Board.Instance.isFull[Board.FindSlotIdFromName(parent.name) - 1] = false;
                    parent = gameObject.transform.parent.transform;
                    position = new Vector2(0, 0);
                    cardsMerged = true;
                }
            }
            eventData.pointerDrag.transform.SetParent(parent);
            eventData.pointerDrag.gameObject.GetComponent<RectTransform>().anchoredPosition = position;
            if (cardsMerged)
                CardMergingManager.Instance.CardsMerged(parent.gameObject);
        }
    }

    private Card GetTargetCard(GameObject go)
        => go.GetComponent<Card>();
}
