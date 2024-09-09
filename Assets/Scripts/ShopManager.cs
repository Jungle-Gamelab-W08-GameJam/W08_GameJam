using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    /*
        0 Double,
        1 Flush,
        2 Straight,
        3 Triple,
        4 Arcane,
        5 Color
    */

    public List<float> increaseRate = new List<float>();
    public List<float> successRate = new List<float>();
    public List<long> scrollCost = new List<long>();


    [Header("HP 구매 관련 변수 목록")]
    public List<long> hpCostLevelDesign = new List<long>();
    public long hpCost;
    public long hpPlusRate;
    public long hpUpgradeNumber;
    [SerializeField]
    private TextMeshProUGUI hpCostText;

    [Header("나머지")]

    private PlayerStats playerStats;

    [SerializeField]
    private TextMeshProUGUI costs;

    [SerializeField]
    private TextMeshProUGUI prob;

    [SerializeField]
    private TextMeshProUGUI feverText;
    public float feverIncreseRate;
    public float feverSuccessRate;
    public int feverLeft;
    public bool onFever;

    [SerializeField]
    private TextMeshProUGUI bonusText;
    private int bonusCost = 1;
    public int bonusLeft;
    public bool onBonus;

    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private GameObject shopUI;
    [SerializeField]
    private GameObject battleUI;
    [SerializeField]
    private Battle battle;
    [SerializeField]
    private TextMeshProUGUI scrollLeftText;
    [SerializeField]
    private List<int> scrollLeft = new List<int>();
    [SerializeField]
    private AudioClip[] bgms;

    [SerializeField]
    private AudioSource clickSound;
    [SerializeField]
    private AudioSource scrollStartSound;

    void Start()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        SetCost();
        SetProb();
        ExitFever();
        ExitBonus();
        SetHpCost();
    }

    public void OnBuyButton(int stat)
    {
        clickSound.Play();
        if (playerStats.gold >= scrollCost[stat] * bonusCost)
        {
            scrollStartSound.Play();
            if (onBonus)
            {
                bonusLeft--;
                if (bonusLeft == 0) ExitBonus();
            }
            else
            {
                scrollCost[stat] += (int)Mathf.Pow(5, (battle.floor / 5) - 1);
                if (scrollLeft[stat] <= 0)
                {
                    return;
                }
                else
                {
                    scrollLeft[stat] -= 1;
                    SetScrollLeftText();
                    playerStats.gold -= scrollCost[stat] * bonusCost;
                    playerStats.UpdateGoldText();
                }
            }

            if (onFever)
            {
                feverLeft--;
                feverText.text = "피버 타임! 남은 횟수: ";
                feverText.text += feverLeft.ToString();
                if (feverLeft == 0) ExitFever();
            }

            playerStats.ChangeStat(stat);
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }

    public void OnHPButon()
    {
        //if (playerStats.gold >= hpCost)
        //{
        //    playerStats.gold -= hpCost;

        //    playerStats.UpdateGoldText();
        //    playerStats.ChangeHP(playerStats.maxHP);
        //}
        //else
        //{
        //    Debug.Log("Not enough gold!");
        //}
        playerStats.ChangeHP(playerStats.maxHP);
    }

    public void SetHpCost()
    {
        hpUpgradeNumber = 0;
        hpCost = hpCostLevelDesign[(int)hpUpgradeNumber];
        hpCostText.text = hpCost + " 메소";
    }

    public void OnBuyHPButon()
    {
        clickSound.Play();
        if ((playerStats.gold >= hpCost) && (hpUpgradeNumber < (hpCostLevelDesign.Count - 1)))
        {
            playerStats.gold -= hpCost;
            playerStats.UpdateGoldText();
            playerStats.maxHP += (int)hpPlusRate;
            hpUpgradeNumber++;
            hpCost = hpCostLevelDesign[(int)hpUpgradeNumber];
            hpCostText.text = hpCost + " 메소";

        }
    }


    public void SetCost()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < scrollCost.Count; i++)
        {
            sb.AppendLine((scrollCost[i] * bonusCost).ToString() + " 메소");
        }
        costs.text = sb.ToString();
    }

    private void SetProb()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < successRate.Count; i++)
        {
            sb.Insert(0, (successRate[i] * feverSuccessRate).ToString() + "%\n");
        }
        prob.text = sb.ToString();
    }

    private void SetScrollLeft()
    {
        for (int i = 0; i < scrollLeft.Count; i++)
        {
            scrollLeft[i] = 10;
        }
        SetScrollLeftText();
    }

    private void SetScrollLeftText()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < scrollLeft.Count; i++)
        {
            sb.AppendLine((scrollLeft[i]).ToString() + " ");
        }
        scrollLeftText.text = sb.ToString();
    }

    public void EnterFever()
    {
        onFever = true;

        feverLeft = 5;
        feverIncreseRate = 1.5f;
        feverSuccessRate = 1.2f;

        feverText.text = "피버 타임! 남은 횟수: ";
        feverText.text += feverLeft.ToString();

        SetProb();
    }

    public void ExitFever()
    {
        onFever = false;

        feverIncreseRate = 1;
        feverSuccessRate = 1;

        feverText.text = "";

        SetProb();
    }

    public void EnterBonus()
    {
        onBonus = true;

        bonusLeft = 1;
        bonusCost = 0;
        SetCost();

        bonusText.text = "보너스 줌서!";
    }

    public void ExitBonus()
    {
        onBonus = false;
        bonusCost = 1;
        SetCost();

        bonusText.text = " ";
    }



    public void OnShopUI()
    {
        shopUI.SetActive(true);
        SetCost();
        SetScrollLeft();
    }

    public void CloseShopUI()
    {
        clickSound.Play();
        SetCost();
        SetProb();
        ExitFever();
        ExitBonus();
        OnHPButon();
        battle.audioSource.clip = bgms[battle.floor / 5];
        battle.audioSource.Play();
        //playerStats.UpdateMulText(99);
        playerStats.UpdateHPText();
        battle.coinText.SetActive(true);
        battle.shopText.SetActive(false);
        shopUI.SetActive(false);
        battleUI.SetActive(true);
    }
}
