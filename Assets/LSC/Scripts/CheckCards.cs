using System;
using System.Collections.Generic;
using UnityEngine;

public class CheckCards : MonoBehaviour
{
    PlayerStats playerStats;

    private void Start()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
    }

    public float CheckCard(char[,] array)
    {
        List<float> tempStats = playerStats.GetStats();
        float multiplier = 1.0f;
        bool tripleChk = false;

        // Arcane
        if (ContainsAll(array, 0))
        {
            Debug.Log("아케인 - "+tempStats[4]);
            multiplier *= tempStats[4];
        }

        // Attribute
        if (AllSame(array, 0))
        {
            Debug.Log("속성 조합 - "+ tempStats[5]);
            multiplier *= tempStats[5];
        }

        // Flush
        if (AllSame(array, 1))
        {
            if (ContainsAll(array, 2))
            {
                Debug.Log("스트레이트 - " + tempStats[2]);
                multiplier *= tempStats[2];
            }else if (AllSame(array, 2))
            {
                Debug.Log("트리플 - " + tempStats[3]);
                multiplier *= tempStats[3];
                tripleChk = true;
            }
            else
            {
                Debug.Log("플러시 - " + tempStats[1]);
                multiplier *= tempStats[1];
                tripleChk = true;
            }
        }

        if (!tripleChk && CheckDouble(array))
        {
            Debug.Log("더블 - "+tempStats[0]);
            multiplier *= tempStats[0];
        }

        return multiplier;
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