using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public float currHP;
    public int maxHP;

    [SerializeField]
    private List<string> statName = new List<string>();
    [SerializeField]
    private List<float> stats = new List<float>();
    [SerializeField]
    private TextMeshProUGUI statText;
    [SerializeField]
    private TextMeshProUGUI goldText;
    [SerializeField]
    private Transform button;
    
    public int gold;

    [SerializeField]
    private TextMeshProUGUI upgradeText;
    [SerializeField]
    private TextMeshProUGUI resultText;
    [SerializeField]
    private TextMeshProUGUI mulText;
    [SerializeField]
    private TextMeshProUGUI hpText;

    private ShopManager shopManager;

    void Start()
    {	
		currHP = maxHP;
        shopManager = GameObject.FindWithTag("Shop").GetComponent<ShopManager>();
        UpdateStatText(99);
        UpdateGoldText();
        UpdateMulText(99);
        UpdateHPText();
    }

    public void GetButton(GameObject obj)
    {
        button = obj.transform.parent;
    }

    public void ChangeStat(int code)
    {
        float increaseRate = shopManager.increaseRate * shopManager.feverIncreseRate;
        List<float> successRate = shopManager.successRate.ConvertAll(x => x * shopManager.feverSuccessRate);

        
        resultText.text = "";

        StartCoroutine(TryUpgrade(code, increaseRate, successRate));
    }

    IEnumerator TryUpgrade(int code, float increaseRate, List<float> successRate)
    {
        ButtonDisable(false);
        UpdateMulText(99);
        UpdateStatText(99);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < 10; i++)
        {
            StringBuilder temp = new StringBuilder();
            temp.Append("Upgrade ").Append(statName[code]).Append("(").Append(successRate[i].ToString()).Append("%)");
            upgradeText.text = temp.ToString();

            if (Random.Range(1, 101) <= successRate[i])
            {
                sb.AppendLine("<color=\"blue\">Success!</color>");
                resultText.text = sb.ToString();
                if (i == 0) stats[code] *= 1 + (increaseRate / 100);
                else stats[code] *= (Mathf.Pow(1 + (increaseRate / 100), Mathf.Pow(2, i - 1)));
                UpdateMulText(i);

                if(i >= 4) // fever enter
                {
                    if (!shopManager.onFever) shopManager.EnterFever();
                }

                if (Random.Range(1, 101) <= 5) // bonus enter
                {
                    shopManager.EnterBonus();
                }

                yield return new WaitForSeconds(0.2f);
            }
            else
            {
                sb.Insert(0, "<color=\"red\">Fail...</color>\n");
                resultText.text = sb.ToString();
                UpdateStatText(code);
                break;
            }
        }
        ButtonDisable(true);
    }

    private void ButtonDisable(bool flag)
    {
        foreach (Transform sibling in button)
        {
            Button siblingButton = sibling.GetComponent<Button>();
            siblingButton.interactable = flag;
        }
    }

    private void UpdateStatText(int code)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < stats.Count; i++)
        {
            if (i == code)
            {
                sb.Append("<color=\"green\">").Append(statName[i]).Append(": ").Append(stats[i].ToString("F2")).Append("x</color>\n");
            }
            else sb.Append(statName[i]).Append(": ").Append(stats[i].ToString("F2")).Append("x\n");
        }
        statText.text = sb.ToString();
    }

    public void UpdateGoldText()
    {
        goldText.text = gold.ToString();
        goldText.text += " G";
    }

    public void UpdateMulText(int code)
    {
        int[] list = {16213, 1177, 257, 89, 37, 17, 8, 4, 2, 1};
        mulText.text = "";
        for (int i = 0;i < 10;i++)
        {
            if (i == 9 - code)
            {
                mulText.text += $"<color=\"green\">{list[i]*shopManager.feverIncreseRate} %</color>\n";
            }
            else mulText.text += $"{list[i] * shopManager.feverIncreseRate} %\n";
        }
    }

    public void UpdateHPText()
    {
        hpText.text = "HP: ";
        hpText.text += currHP.ToString();
        hpText.text += " / ";
        hpText.text += maxHP.ToString();
    }

    public List<float> GetStats()
    {
        return stats;
    }

    public void GetGold(int getGold) {
        gold += getGold;
        UpdateGoldText();
    }

    public void ChangeHP(int changeHP)
    {
        currHP = Mathf.Min(currHP + changeHP, maxHP);
        UpdateHPText();
        if (currHP <= 0) {
            Debug.Log("Game Over");
            // gameOver
        }
    }
}
