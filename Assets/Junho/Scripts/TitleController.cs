using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleController : MonoBehaviour
{
    private Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1.2f);  // 마우스 올렸을 때 크기
    private Vector3 originalScale;  // 원래 크기
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnter()
    {
        // 마우스가 버튼 위로 올라갈 때 버튼 크기를 확대
        transform.localScale = hoverScale;
        Debug.Log("gksms");
    }

    public void OnPointerExit()
    {
        // 마우스가 버튼을 벗어났을 때 원래 크기로 복구
        transform.localScale = originalScale;
        Debug.Log("gksms");
    }
}


