using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

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
    public List<int> scrollCost = new List<int>();

    private PlayerStats playerStats;

    void Start()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
    }

    public void OnBuyButton(int stat)
    {
        if (playerStats.gold > scrollCost[stat])
        {
            playerStats.gold -= scrollCost[stat];
            playerStats.UpdateGoldText();
            playerStats.ChangeStat(stat, increaseRate[stat], successRate[stat]);
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }
}
