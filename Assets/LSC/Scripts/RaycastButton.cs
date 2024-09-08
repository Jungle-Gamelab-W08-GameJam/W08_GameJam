using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class RaycastButton : MonoBehaviour
{
    public GraphicRaycaster graphicRaycaster;   // UI에 Raycast를 쏘기 위한 컴포넌트
    public EventSystem eventSystem;             // EventSystem 필요


    [SerializeField]
    private bool hasExecuted = false;
    public AudioSource audioSource;

    void Update()
    {
        if (IsPointerOverUIElement())
        {
            if (!hasExecuted)
            {
                audioSource.Play();
                hasExecuted = true;
            }
        }
        else
        {
            hasExecuted = false;
        }
    }

    bool IsPointerOverUIElement()
    {
        PointerEventData pointerEventData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition 
        };

        List<RaycastResult> results = new List<RaycastResult>();

        graphicRaycaster.Raycast(pointerEventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("Button"))
            {
                return true;
            }
        }

        return false;
    }
}