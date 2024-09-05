using System;
using System.Collections;
using System.Collections.Generic;
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
        UpdateMonsterHP();
        UpdatePlayerHP();
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

        for (int i = 0; i < tempCards.Length; i++)
        {
            for (int j = 0; j < tempCards[i].Length; j++)
            {
                tempArray[i, j] = tempCards[i][j];
            }
        }

        float damage = checkCards.CheckCard(tempArray);
        currMonsterHP -= damage;
        Debug.Log("Damage : "+damage);

        if (currMonsterHP <= 0)
        {
            float tempHP = currMonsterHP;
            playerStats.GetGold(Mathf.Abs((int)(tempHP * 10)));
            currMonsterHP = 0;
            MonsterDead();
        }
        else
        {
            playerStats.ChangeHP(-currMonsterATK);
            UpdateMonsterHP();
            UpdatePlayerHP();
        }
    }

    public void UpdateMonsterHP()
    {
        monsterHPImage.fillAmount = currMonsterHP / monsterMaxHP;
        monsterHPText.text = currMonsterHP.ToString() + '/' + monsterMaxHP;
    }

    public void UpdatePlayerHP()
    {
        playerHPImage.fillAmount = playerStats.currHP/playerStats.maxHP;
        playerHPText.text = playerStats.currHP.ToString() + '/' + playerStats.maxHP;
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
            if(floor != 5)
            {
                for (int i = 0; i < shopManager.scrollCost.Count; i++)
                {
                    shopManager.scrollCost[i] *= 2 * ((floor / 5) - 1);
                }
            }
            battleScene.SetActive(false);
            shopManager.OnShopUI();
        }

        UpdateMonsterHP();
    }
}