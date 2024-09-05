using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CheckCards : MonoBehaviour
{
    PlayerStats playerStats;

    public Image[] images = new Image[6];
    public TMP_Text damageText;
    public float damage;


    private void Start()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
    }

    private void Update()
    {
        string[] tempCards = KingManager.Instance.DrawCards;
        char[,] tempArray = new char[3, 3];

        for (int i = 0; i < tempCards.Length; i++)
        {
            for (int j = 0; j < tempCards[i].Length; j++)
            {
                tempArray[i, j] = tempCards[i][j];
            }
        }

        CheckCard(tempArray);
    }

    public void CheckCard(char[,] array)
    {
        bool[] check = new bool[6] { false, false, false, false, false, false };
        List<float> tempStats = playerStats.GetStats();
        float multiplier = 1.0f;

        // Arcane
        if (ContainsAll(array, 0))
        {
            check[1] = true;
            multiplier *= tempStats[4];
        }
        else
        {
            check[1] = false;
        }

        // Attribute
        if (AllSame(array, 0))
        {
            check[2] = true;    
            multiplier *= tempStats[5];
        }
        else
        {
            check[2] = false;
        }

        // Flush
        if (AllSame(array, 1))
        {
            // Straight
            if (ContainsAll(array, 2))
            {
                check[4] = true;
                multiplier *= tempStats[2];
            }
            // Triple
            else if (AllSame(array, 2))
            {
                multiplier *= tempStats[3];
                check[5] = true;
            }
            else
            {
                check[4] = false;
                check[5] = false;
                multiplier *= tempStats[1];
                check[3] = true;
            }
        }
        else
        {
            check[3] = false;
        }

        if (!check[3] && !check[5] && CheckDouble(array))
        {
            check[0] = true;
            multiplier *= tempStats[0];
        }
        else
        {
            check[0] = false;
        }

        for(int i = 0; i< 6; i++)
        {
            Color color = images[i].color;
            if (check[i])
            {
                color.a = 1f;        
            }
            else
            {
                color.a = 0.35f;
            }
            images[i].color = color;
        }

        damage = multiplier;
        damageText.text ="현재 데미지\n"+multiplier.ToString("F2");
    }

    bool ContainsAll(char[,] array, int index)
    {
        bool[] tempChk = new bool[3] {false, false, false};
        for(int i = 0; i<3; i++)
        {
            switch(array[i, index])
            {
                case 'A': tempChk[0] = true; break;
                case 'B': tempChk[1] = true; break;
                case 'C': tempChk[2] = true; break;
            }
        }
        
        for(int i = 0; i<3; i++)
        {
            if (!tempChk[i])
            {
                return false;
            }
        }
        return true;
    }

    bool AllSame(char[,] array, int index)
    {
        return array[0, index] == array[1, index] && array[1, index] == array[2, index];
    }

    bool CheckDouble(char[,] array)
    {
        return (array[0, 1] == array[1, 1] && array[0, 2] == array[1, 2]) || (array[0, 1] == array[2, 1] && array[0, 2] == array[2, 2]) || (array[1, 1] == array[2, 1] && array[1, 2] == array[2, 2]);
    }
}