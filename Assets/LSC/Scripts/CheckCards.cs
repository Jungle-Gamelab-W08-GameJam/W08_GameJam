using System;
using UnityEngine;

public class CheckCards : MonoBehaviour
{
    float CheckCombination(char[,] array)
    {
        float multiplier = 1.0f;

        // Arcane
        if (ContainsAll(array, 0))
        {
            Debug.Log("������ - 2.5��");
            multiplier *= 2.5f;
        }

        // Attribute
        if (AllSame(array, 0))
        {
            Debug.Log("�Ӽ� ���� - 3��");
            multiplier *= 3f;
        }

        // Staraight
        if (ContainsAll(array, 1))
        {
            Debug.Log("��Ʈ����Ʈ - 5��");
            multiplier *= 5f;
        }

        // Flush
        if (AllSame(array, 1))
        {
            Debug.Log("�÷��� - 3��");
            multiplier *= 3f;
        }

        // Double, Triple
        int count = CountSame(array, 2);
        if (count == 2)
        {
            Debug.Log("���� - 2��");
            multiplier *= 2f;
        }
        else if (count == 3)
        {
            Debug.Log("Ʈ���� - 7��");
            multiplier *= 7f;
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

    int CountSame(char[,] array, int index)
    {
        int sameCount = 1;
        if (array[0, index] == array[1, index])
        {
            sameCount++;
        }
        if (array[0, index] == array[2, index])
        {
            sameCount++;
        }
        if (array[1, index] == array[2, index] && array[0, index] != array[2, index])
        {
            sameCount++;
        }
        return sameCount;
    }
}