using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.PackageManager.Requests;
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

    public float increaseRate;
    public List<float> successRate = new List<float>();
    public List<int> scrollCost = new List<int>();
    public int hpCost;

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

    void Start()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        SetCost();
        SetProb();
        ExitFever();
        ExitBonus();
    }

    public void OnBuyButton(int stat)
    {
        if (playerStats.gold >= scrollCost[stat] * bonusCost)
        {
            playerStats.gold -= scrollCost[stat] * bonusCost;
            playerStats.UpdateGoldText();

            if (onFever)
            {
                feverLeft--;
                feverText.text = "Fever Time! Left Count: ";
                feverText.text += feverLeft.ToString();
                if (feverLeft==0) ExitFever();
            }
                
            if (onBonus)
            {
                bonusLeft--;
                if (bonusLeft==0) ExitBonus();
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
        if (playerStats.gold >= hpCost)
        {
            playerStats.gold -= hpCost;

            playerStats.UpdateGoldText();
            playerStats.ChangeHP(playerStats.maxHP);
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }

    private void SetCost()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < scrollCost.Count; i++)
        {
            sb.AppendLine((scrollCost[i] * bonusCost).ToString() + " G");
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

    public void EnterFever()
    {
        onFever = true;
        
        feverLeft = 5;
        feverIncreseRate = 1.5f;
        feverSuccessRate = 1.2f;

        feverText.text = "Fever Time! Left Count: ";
        feverText.text += feverLeft.ToString();

        playerStats.UpdateMulText(99);
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

        bonusText.text = "Bonus Scroll!";
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
    }

    public void CloseShopUI()
    {
        SetCost();
        SetProb();
        ExitFever();
        ExitBonus();
        playerStats.UpdateMulText(99);
        shopUI.SetActive(false);
        battleUI.SetActive(true);
    }
}
