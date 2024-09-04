using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private List<string> statName = new List<string>();
    [SerializeField]
    private List<float> stats = new List<float>();
    [SerializeField]
    private TextMeshProUGUI statText;
    [SerializeField]
    private TextMeshProUGUI goldText;

    private Transform button;
    
    public int gold;
    public int hp;

    [SerializeField]
    private TextMeshProUGUI upgradeText;
    [SerializeField]
    private TextMeshProUGUI resultText;

    private ShopManager shopManager;

    void Start()
    {
        shopManager = GameObject.FindWithTag("Shop").GetComponent<ShopManager>();
        UpdateStatText();
        UpdateGoldText();
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
                UpdateStatText();

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
                sb.AppendLine("<color=\"red\">Fail...</color>");
                resultText.text = sb.ToString();
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

    private void UpdateStatText()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < stats.Count; i++)
        {
            sb.Append(statName[i]).Append(": ").Append(stats[i].ToString("F2")).Append("\n");
        }
        statText.text = sb.ToString();
    }

    public void UpdateGoldText()
    {
        goldText.text = gold.ToString();
        goldText.text += " G";
    }
}
