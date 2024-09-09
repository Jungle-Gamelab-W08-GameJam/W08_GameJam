using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingManager : MonoBehaviour
{
    public static KingManager Instance { get; private set; }

    [SerializeField]
    private GameObject TipCanvas;

    private string[] drawCards;

    public string[] DrawCards
    {
        get { return drawCards; }
        set { drawCards = value; }
    }

    void Awake()
    {
        // singleton pattern. not essential!!!!!
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Tap 키를 눌렀을 때 TipCanvas 활성화 상태를 토글
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // TipCanvas의 활성화 상태를 반전시킵니다.
            TipCanvas.SetActive(!TipCanvas.activeSelf);
        }
    }
}
