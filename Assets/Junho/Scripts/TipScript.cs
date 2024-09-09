using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Tap 키를 눌렀을 때
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // 현재 오브젝트의 활성화 상태를 반전시킵니다.
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}