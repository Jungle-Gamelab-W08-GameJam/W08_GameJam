using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Battle : MonoBehaviour
{
    public ShopManager shopManager;
    public PlayerStats playerStats;
    public DrawController drawController;
    public CheckCards checkCards;
    public Button battleButton;

    public Image playerHPImage;
    public Image monsterHPImage;

    public TMP_Text playerHPText;
    public TMP_Text monsterHPText;

    [SerializeField]
    private int[] monsterHPs;
    [SerializeField]
    private int[] monsterATKs;

    [SerializeField]
    private int floor;
    [SerializeField]
    private float currMonsterHP;
    [SerializeField]
    private int monsterMaxHP;
    [SerializeField]
    private int currMonsterATK;
    [SerializeField]
    private GameObject battleScene;

    void Start()
    {
        floor = 1;
        monsterMaxHP = monsterHPs[floor];
        currMonsterHP = monsterMaxHP;
        currMonsterATK = monsterATKs[floor];

        OnButtons();
    }

    void OnButtons()
    {
        battleButton.onClick.RemoveAllListeners();

        battleButton.onClick.AddListener(OnBattle);
        battleButton.onClick.AddListener(drawController.DecisionDraw);
    }

    public void OnBattle()
    {
        string[] tempCards = KingManager.Instance.DrawCards;
        foreach (string card in tempCards) { 
            Debug.Log(card);
        }
        char[,] tempArray = new char[3, 3];

        // string 배열을 char 배열로 변환
        for (int i = 0; i < tempCards.Length; i++)
        {
            for (int j = 0; j < tempCards[i].Length; j++)
            {
                tempArray[i, j] = tempCards[i][j];
            }
        }

        float damage = checkCards.CheckCard(tempArray);
        Debug.Log("총 배율 : "+damage);

        
        //monsterHPImage.fillAmount = currMonsterHP / monsterMaxHP;

        if (currMonsterHP <= damage)
        {
            Debug.Log("몬스터 사망");
            playerStats.GetGold(Mathf.Abs((int)(damage - currMonsterHP)));
            currMonsterHP = 0;
            MonsterDead();
        }
        else
        {
            currMonsterHP -= damage;
            playerStats.ChangeHP(-currMonsterATK);
        }
        Debug.Log("현재 몬스터 체력 : " + currMonsterHP + "/"+monsterMaxHP+", 플레이어 체력 : "+playerStats.currHP);
    }

    public void MonsterDead()
    {
        floor++;
        monsterMaxHP = monsterHPs[floor];
        currMonsterHP = monsterMaxHP;
        currMonsterATK = monsterATKs[floor];
        drawController.ClickFightButton();

        if(floor % 5 == 0)
        {
            battleScene.SetActive(false);
            shopManager.OnShopUI();
        }
    }
}