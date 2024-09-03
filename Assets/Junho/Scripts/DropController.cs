using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DropController : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;
        if (droppedObject != null)
        {
            // Snap the dropped object to this drop zone
            droppedObject.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

            // Swap text between dropped card and drop zone card
            TextMeshProUGUI droppedText = droppedObject.GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI dropZoneText = GetComponentInChildren<TextMeshProUGUI>();

            string temp = droppedText.text;
            droppedText.text = dropZoneText.text;
            dropZoneText.text = temp;
        }
    }
}
