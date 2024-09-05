using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Test_Battle : MonoBehaviour
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
    }

    void OnButtons()
    {
        battleButton.onClick.RemoveAllListeners();

        battleButton.onClick.AddListener(OnBattle);
        battleButton.onClick.AddListener(drawController.DecisionDraw);
    }

    public void OnBattle()
    {
        battleButton.interactable = false;
        drawController.PlayerAttackAnimation();
        StartCoroutine(HandleBattleAfterAnimation());
    }

    public void UpdateMonsterHP()
    {
        monsterHPImage.fillAmount = currMonsterHP / monsterMaxHP;
        monsterHPText.text = currMonsterHP.ToString() + '/' + monsterMaxHP;
    }

    public void UpdatePlayerHP()
    {
        playerHPImage.fillAmount = playerStats.currHP / playerStats.maxHP;
        playerHPText.text = playerStats.currHP.ToString() + '/' + playerStats.maxHP;
    }

    public void MonsterDead()
    {
        floor++;
        monsterMaxHP = monsterHPs[floor];
        currMonsterHP = monsterMaxHP;
        currMonsterATK = monsterATKs[floor];
        drawController.ClickFightButton();

        if (floor % 5 == 0)
        {
            battleScene.SetActive(false);
            shopManager.OnShopUI();
        }

        UpdateMonsterHP();
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
    }

    IEnumerator HandleBattleAfterAnimation()
    {
        yield return new WaitForSeconds(4f);
        battleButton.interactable = true;
        float damage = checkCards.damage;
        currMonsterHP -= damage;


        if (currMonsterHP <= 0)
        {
            float tempHP = currMonsterHP;
            playerStats.GetGold(Mathf.Abs((int)tempHP));
            int tempGold = Mathf.Abs((int)tempHP);
            addGoldText.GetComponent<TMP_Text>().text = tempGold.ToString() + "��� ȹ��!";
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
}