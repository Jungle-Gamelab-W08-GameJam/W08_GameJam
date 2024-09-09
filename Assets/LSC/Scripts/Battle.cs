using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Battle : MonoBehaviour
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
    private long[] monsterHPs;
    [SerializeField]
    private int[] monsterATKs;

    [SerializeField]
    public int floor;
    [SerializeField]
    private double currMonsterHP;
    [SerializeField]
    private long monsterMaxHP;
    [SerializeField]
    private int currMonsterATK;
    [SerializeField]
    private GameObject battleScene;
    [SerializeField]
    private GameObject addGoldText;
    [SerializeField]
    private GameObject heatText;
    [SerializeField]
    private TextMeshProUGUI floorText;
    [SerializeField]
    private TextMeshProUGUI monsterATKText;
    [SerializeField]
    private Sprite[] monsterImgs;
    [SerializeField]
    private Image monsterImg;

    [SerializeField]
    private TextMeshProUGUI currHPText;

    public GameObject coinText;
    public GameObject shopText;

    public AudioSource audioSource;
    public AudioSource clickAudioSource;
    [SerializeField]
    private AudioClip shopBgm;

    public AudioSource attackAudioSource;
    public AudioSource mesoSound;

    public float fadeDuration = 1.0f;
    public float delayBeforeFade = 3.0f;

    double damage;

    void Start()
    {
        floor = 1;
        monsterMaxHP = monsterHPs[floor];
        currMonsterHP = monsterMaxHP;
        currMonsterATK = monsterATKs[floor];

        OnButtons();
        UpdateMonsterHP();
        UpdateFloorText();
    }

    private void Update()
    {
        UpdatePlayerHP();
    }

    void OnButtons()
    {
        battleButton.onClick.RemoveAllListeners();

        battleButton.onClick.AddListener(OnBattle);
    }

    public void OnBattle()
    {
        clickAudioSource.Play();
        battleButton.interactable = false;
        damage = checkCards.damage;
        drawController.PlayerAttackAnimation();
        StartCoroutine(HandleBattleAfterAnimation());
        StartCoroutine(attackSound());
    }

    public void UpdateMonsterHP()
    {
        monsterHPImage.fillAmount = (float)currMonsterHP / monsterMaxHP;
        if (currMonsterHP % 1 == 0)
        {
            monsterHPText.text = currMonsterHP.ToString("F0") + '/' + monsterMaxHP;
        }
        else
        {
            monsterHPText.text = currMonsterHP.ToString("F2") + '/' + monsterMaxHP;
        }
        monsterATKText.text = currMonsterATK.ToString();
    }

    public void UpdatePlayerHP()
    {
        playerHPImage.fillAmount = playerStats.currHP / playerStats.maxHP;
        currHPText.text = "최대 체력 : " + playerStats.maxHP.ToString();
        playerHPText.text = playerStats.currHP.ToString() + '/' + playerStats.maxHP;
    }

    public void UpdateFloorText()
    {
        floorText.text = floor.ToString() + "층";
    }

    public void MonsterDead()
    {
        floor++;
        monsterMaxHP = monsterHPs[floor];
        currMonsterHP = monsterMaxHP;
        currMonsterATK = monsterATKs[floor];
        mesoSound.Play();
        //drawController.ClickFightButton();

        if (floor % 5 == 1 && floor != 1)
        {
            if (floor != 6)
            {
                for (int i = 0; i < shopManager.scrollCost.Count; i++)
                {
                    //shopManager.scrollCost[i] *= 2 * ((floor / 5) - 1); 
                    shopManager.scrollCost[i] *= 5; 
                }
            }
            audioSource.clip = shopBgm;
            audioSource.Play();
            drawController.ClickFightButton();
            coinText.SetActive(false);
            shopText.SetActive(true);
            battleScene.SetActive(false);
            shopManager.OnShopUI();
        }

        UpdateMonsterHP();
    }

    IEnumerator FadeOutAndDeactivate()
    {
        yield return new WaitForSeconds(delayBeforeFade);
        Color originalColor = addGoldText.GetComponent<TMP_Text>().color;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            addGoldText.GetComponent<TMP_Text>().color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        addGoldText.SetActive(false);
        addGoldText.GetComponent<TMP_Text>().color = Color.yellow;
    }

    IEnumerator attackSound()
    {
        yield return new WaitForSeconds(0.6f);
        attackAudioSource.Play();
        yield return new WaitForSeconds(0.65f);
        attackAudioSource.Play();
    }

    IEnumerator HandleBattleAfterAnimation()
    {
        yield return new WaitForSeconds(1.8f);
        drawController.DecisionDraw();
        battleButton.interactable = true;
        currMonsterHP -= damage;

        if (currMonsterHP <= 0)
        {
            double tempHP = currMonsterHP;
            playerStats.GetGold(Mathf.Abs((float)(tempHP * 10)));
            double tempGold = Mathf.Abs((float)(tempHP * 10));
            addGoldText.GetComponent<TMP_Text>().text = tempGold.ToString("F0") + "메소 획득!";
            addGoldText.SetActive(true);
            heatText.GetComponent<TMP_Text>().text = "-" + damage.ToString("F0");
            heatText.SetActive(true);
            StartCoroutine(HeatTextFadeOutAndDeactivate());
            StartCoroutine(FadeOutAndDeactivate());
            currMonsterHP = 0;
            MonsterDead();
            UpdateFloorText();
            monsterImg.sprite = monsterImgs[floor];
        }
        else
        {
            playerStats.ChangeHP(-currMonsterATK);
            UpdateMonsterHP();
            heatText.GetComponent<TMP_Text>().text = "-" + damage.ToString("F0");
            heatText.SetActive(true);
            StartCoroutine(HeatTextFadeOutAndDeactivate());
        }
    }

    IEnumerator HeatTextFadeOutAndDeactivate()
    {
        yield return new WaitForSeconds(delayBeforeFade);
        Color originalColor = heatText.GetComponent<TMP_Text>().color;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            heatText.GetComponent<TMP_Text>().color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        heatText.SetActive(false);
        heatText.GetComponent<TMP_Text>().color = Color.red;
    }
}