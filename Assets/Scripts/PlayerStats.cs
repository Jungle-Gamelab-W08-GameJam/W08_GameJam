using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting.FullSerializer;
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

    private Button button;
    
    public int gold;

    void Start()
    {
        UpdateStatText();
        UpdateGoldText();
    }

    public void GetButton(GameObject obj)
    {
        button = obj.GetComponent<Button>();
    }

    public void ChangeStat(int code, float increaseRate, float successRate)
    {
        StartCoroutine(TryUpgrade(code, increaseRate, successRate));
    }

    IEnumerator TryUpgrade(int code, float increaseRate, float successRate)
    {
        button.interactable = false;

        for (int i = 0; i < 10; i++)
        {
            if (Random.Range(1, 101) <= successRate)
            {
                Debug.Log("success");
                stats[code] *= 1 + (increaseRate / 100);
                UpdateStatText();
                yield return new WaitForSeconds(2.0f);
            }
            else
            {
                Debug.Log("fail");
                break;
            }
        }

        button.interactable = true;
    }

    private void UpdateStatText()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < stats.Count; i++)
        {
            sb.Append(statName[i]).Append(": ").Append(stats[i]).Append("\n");
        }
        statText.text = sb.ToString();
    }

    public void UpdateGoldText()
    {
        goldText.text = gold.ToString();
        goldText.text += " G";
    }
}
