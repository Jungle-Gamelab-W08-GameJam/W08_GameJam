using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Battle : MonoBehaviour
{
    public PlayerStats playerStats;
    public CheckCards checkCards;
    public Button battleButton;

    [SerializeField]
    private int[] monsterHPs;


    [SerializeField]
    private int floor;
    [SerializeField]
    private int currMonsterHP;
    [SerializeField]
    private int monsterMaxHP;

    void Start()
    {
        floor = 1;
        monsterMaxHP = monsterHPs[floor];
        currMonsterHP = monsterMaxHP;

        OnButtons();
    }

    void OnButtons()
    {
        battleButton.onClick.RemoveAllListeners();

        battleButton.onClick.AddListener(OnBattle);
    }

    public void OnBattle()
    {
        string[] tempCards = KingManager.Instance.DrawCards;
        char[,] tempArray = new char[3, 3];

        // string �迭�� char �迭�� ��ȯ
        for (int i = 0; i < tempCards.Length; i++)
        {
            for (int j = 0; j < tempCards[i].Length; j++)
            {
                tempArray[i, j] = tempCards[i][j];
            }
        }

        float damage = playerStats.GetATK()*checkCards.CheckCard(tempArray);
        Debug.Log("�� ���� : "+damage/ playerStats.GetATK()+"�� ������ : " + damage);
    }
}