using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingManager : MonoBehaviour
{
    public static KingManager Instance { get; private set; }

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
}
