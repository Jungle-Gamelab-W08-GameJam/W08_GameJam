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

        // string �迭�� char �迭�� ��ȯ
        for (int i = 0; i < tempCards.Length; i++)
        {
            for (int j = 0; j < tempCards[i].Length; j++)
            {
                tempArray[i, j] = tempCards[i][j];
            }
        }

        float damage = checkCards.CheckCard(tempArray);
        Debug.Log("�� ���� : "+damage);

        
        //monsterHPImage.fillAmount = currMonsterHP / monsterMaxHP;
		UpdateMonsterHP();
        if (currMonsterHP <= damage)
        {
            Debug.Log("���� ���");
            playerStats.GetGold(Mathf.Abs((int)(damage - currMonsterHP)));
            currMonsterHP = 0;
            MonsterDead();
        }
        else
        {
            currMonsterHP -= damage;
            playerStats.ChangeHP(-currMonsterATK);
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
            battleScene.SetActive(false);
            shopManager.OnShopUI();
        }

        UpdateMonsterHP();
    }
}