using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.GetComponent<Card>().canMerge)
        {
            Transform parent = DragDrop.Instance.GetLastParent().transform;
            Vector3 position = Vector3.zero;
            bool cardsMerged = false;

            Card self = eventData.pointerDrag.GetComponent<Card>();
            bool selfIsRainbow = self.name.Contains("Rainbow");
            bool selfIsBoost = self.name.Contains("Boost");
            bool targetIsRainbow = false;
            if (GetTargetCard(gameObject) != null)
            {
                Card target = GetTargetCard(gameObject);
                targetIsRainbow = target.name.Contains("Rainbow");
                if ((self.name == target.name || selfIsRainbow || selfIsBoost) && self.starCount == target.starCount && self.tag == target.tag)
                {
                    if (((!selfIsRainbow && self.starCount < 7) || selfIsRainbow || selfIsBoost) || (selfIsRainbow && targetIsRainbow && self.starCount < 7))
                    {
                        if (!selfIsRainbow || (selfIsRainbow && targetIsRainbow) || selfIsBoost)
                            Board.Instance.isFull[Board.FindSlotIdFromName(parent.name) - 1] = false;
                        parent = gameObject.transform.parent.transform;
                        position = new Vector2(0, 0);
                        cardsMerged = true;
                    }
                }
            }
            if (!selfIsRainbow || selfIsRainbow && targetIsRainbow)
                eventData.pointerDrag.transform.SetParent(parent);
            eventData.pointerDrag.gameObject.GetComponent<RectTransform>().anchoredPosition = position;
            if (cardsMerged)
                CardMergingManager.Instance.CardsMerged(eventData.pointerDrag.transform.parent.gameObject, parent.gameObject);
        }
    }

    private Card GetTargetCard(GameObject go)
        => go.GetComponent<Card>();
}
