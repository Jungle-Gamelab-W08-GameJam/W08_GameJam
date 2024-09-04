using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DragController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector2 originalPosition;

    private DrawController drawController;

    public int cardIndex; // 카드의 인덱스를 추적하기 위한 변수
    public bool isHandCard; // HandCard인지 DrawCard인지를 나타내는 플래그

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
        canvasGroup = GetComponent<CanvasGroup>();
        drawController = FindObjectOfType<DrawController>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        rectTransform.anchoredPosition = originalPosition;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag != gameObject)
        {
            // 드롭된 카드와 드래그된 카드의 컨트롤러를 가져옴
            DragController draggedCardController = eventData.pointerDrag.GetComponent<DragController>();

            // DrawController를 통해 카드 텍스트 및 배열 요소를 교환
            drawController.SwapCardData(this, draggedCardController);
        }
    }
}
