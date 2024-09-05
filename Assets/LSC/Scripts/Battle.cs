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
    [SerializeField]
    private GameObject addGoldText;
    [SerializeField]
    private TMP_Text floorText;
    [SerializeField]
    private TMP_Text monsterAtkText;

    public float fadeDuration = 1.0f;
    public float delayBeforeFade = 3.0f;

    void Start()
    {
        floor = 1;
        monsterMaxHP = monsterHPs[floor];
        currMonsterHP = monsterMaxHP;
        currMonsterATK = monsterATKs[floor];

        OnButtons();
        UpdateMonsterHP();
        UpdatePlayerHP();
        UpdateFloorText();
        UpdateMonsterAtk();
    }

    void OnButtons()
    {
        battleButton.onClick.RemoveAllListeners();

        battleButton.onClick.AddListener(OnBattle);
        battleButton.onClick.AddListener(drawController.DecisionDraw);
    }

    void UpdateFloorText()
    {
        floorText.text = floor + "��";
    }

    void UpdateMonsterAtk()
    {
        monsterAtkText.text = currMonsterATK.ToString();
    }

    public void OnBattle()
    {
        float damage = checkCards.damage;
        currMonsterHP -= damage;
        

        if (currMonsterHP <= 0)
        {
            float tempHP = currMonsterHP;
            playerStats.GetGold(Mathf.Abs((int)tempHP * 10));
            int tempGold = Mathf.Abs((int)tempHP  10);
            addGoldText.GetComponent<TMP_Text>().text = tempGold.ToString()+"��� ȹ��!";
            addGoldText.SetActive(true);
            StartCoroutine(FadeOutAndDeactivate());
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
        if(currMonsterHP % 1 == 0)
        {
            monsterHPText.text = currMonsterHP.ToString("F0") + '/' + monsterMaxHP;
        }
        else
        {
            monsterHPText.text = currMonsterHP.ToString("F1") + '/' + monsterMaxHP;
        }
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

        UpdateFloorText();
        UpdateMonsterHP();
        UpdateMonsterAtk();
    }

    IEnumerator FadeOutAndDeactivate()
    {
        // 3�� ���
        yield return new WaitForSeconds(delayBeforeFade);

        Color originalColor = addGoldText.GetComponent<TMP_Text>().color;

        // 1�� ���� ���̵�ƿ�
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            addGoldText.GetComponent<TMP_Text>().color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // ���������� ��Ȱ��ȭ
        addGoldText.SetActive(false);
        addGoldText.GetComponent<TMP_Text>().color = Color.yellow;
    }
}