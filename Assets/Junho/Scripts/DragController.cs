using UnityEngine;
using UnityEngine.EventSystems;

public class DragController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    private Vector2 originalPosition;

    private void Awake()
    {
   rectTransform = GetComponent<RectTransform>(); // rectTransform을 먼저 초기화
        originalPosition = rectTransform.anchoredPosition; // 이제 원래 위치를 안전하게 저장할 수 있음
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f; // Make the card semi-transparent while dragging
        canvasGroup.blocksRaycasts = false; // Disable raycasts so the card can be dropped on others
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta; // Move the card along with the cursor
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f; // Reset the transparency
        canvasGroup.blocksRaycasts = true; // Enable raycasts again
        rectTransform.anchoredPosition = originalPosition;
    }
}
