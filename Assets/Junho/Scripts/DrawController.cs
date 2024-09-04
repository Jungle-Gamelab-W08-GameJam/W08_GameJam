using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class DrawController : MonoBehaviour
{
    public GameObject DeckCanvas;
    public GameObject FightButton;
    public GameObject CardPhase;
    public GameObject drawCard1;
    public GameObject drawCard2;
    public GameObject drawCard3;
    public GameObject HandCard1;
    public GameObject HandCard2;
    public GameObject HandCard3;


    private List<string> stringList; // Changed to List for easier manipulation
    private string[] drawCards = new string[3];
    private string[] handCards = new string[3];

    private bool previousActiveState = false;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        CheckDeckCanvasStatus();
    }

    // AAA~CCC 까지의 문자열을 생성하는 함수
    private string[] GenerateArray()
    {
        List<string> list = new List<string>();

        for (char first = 'A'; first <= 'C'; first++)
        {
            for (char second = 'A'; second <= 'C'; second++)
            {
                for (char third = 'A'; third <= 'C'; third++)
                {
                    list.Add($"{first}{second}{third}");
                }
            }
        }

        return list.ToArray();
    }

    public void DecisionDraw()
    {
        // Check if stringList is empty and regenerate it if necessary
        if (stringList.Count == 0)
        {
            stringList = new List<string>(GenerateArray());

            // Remove cards that are already in handCards
            foreach (var handCard in handCards)
            {
                if (!string.IsNullOrEmpty(handCard))
                {
                    stringList.Remove(handCard);
                }
            }
        }

        // Empty the drawCards array
        for (int i = 0; i < drawCards.Length; i++)
        {
            drawCards[i] = null;
        }

        // Randomly select and remove elements for drawCards
        for (int i = 0; i < drawCards.Length; i++)
        {
            if (stringList.Count > 0)
            {
                int randomIndex = Random.Range(0, stringList.Count);
                drawCards[i] = stringList[randomIndex];
                stringList.RemoveAt(randomIndex);
            }
        }

        // Assign the new drawCards to the corresponding GameObjects (e.g., TextMeshPro or UI Text component)
        //drawCard1.GetComponentInChildren<TextMeshProUGUI>().text = drawCards[0];
        //drawCard2.GetComponentInChildren<TextMeshProUGUI>().text = drawCards[1];
        //drawCard3.GetComponentInChildren<TextMeshProUGUI>().text = drawCards[2];


        // Assign the new drawCards to the corresponding GameObjects by loading images
        drawCard1.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Images/{drawCards[0]}");
        drawCard2.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Images/{drawCards[1]}");
        drawCard3.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Images/{drawCards[2]}");

        KingManager.Instance.DrawCards = drawCards;


    }
    public void SwapCardData(DragController card1, DragController card2)
    {
        // 배열 요소 교환
        if (card1.isHandCard && card2.isHandCard)
        {
            // 둘 다 HandCard일 경우
            string tempCard = handCards[card1.cardIndex];
            handCards[card1.cardIndex] = handCards[card2.cardIndex];
            handCards[card2.cardIndex] = tempCard;
        }
        else if (!card1.isHandCard && !card2.isHandCard)
        {
            // 둘 다 DrawCard일 경우
            string tempCard = drawCards[card1.cardIndex];
            drawCards[card1.cardIndex] = drawCards[card2.cardIndex];
            drawCards[card2.cardIndex] = tempCard;
        }
        else if (card1.isHandCard && !card2.isHandCard)
        {
            // card1이 HandCard이고 card2가 DrawCard일 경우
            string tempCard = handCards[card1.cardIndex];
            handCards[card1.cardIndex] = drawCards[card2.cardIndex];
            drawCards[card2.cardIndex] = tempCard;
        }
        else if (!card1.isHandCard && card2.isHandCard)
        {
            // card1이 DrawCard이고 card2가 HandCard일 경우
            string tempCard = drawCards[card1.cardIndex];
            drawCards[card1.cardIndex] = handCards[card2.cardIndex];
            handCards[card2.cardIndex] = tempCard;
        }

        // Assign the new drawCards to the corresponding GameObjects (e.g., TextMeshPro or UI Text component)
        //drawCard1.GetComponentInChildren<TextMeshProUGUI>().text = drawCards[0];
        //drawCard2.GetComponentInChildren<TextMeshProUGUI>().text = drawCards[1];
        //drawCard3.GetComponentInChildren<TextMeshProUGUI>().text = drawCards[2];
        //HandCard1.GetComponentInChildren<TextMeshProUGUI>().text = handCards[0];
        //HandCard2.GetComponentInChildren<TextMeshProUGUI>().text = handCards[1];
        //HandCard3.GetComponentInChildren<TextMeshProUGUI>().text = handCards[2];


        // Assign the new drawCards to the corresponding GameObjects by loading images
        HandCard1.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Images/{handCards[0]}");
        HandCard2.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Images/{handCards[1]}");
        HandCard3.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Images/{handCards[2]}");
        drawCard1.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Images/{drawCards[0]}");
        drawCard2.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Images/{drawCards[1]}");
        drawCard3.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Images/{drawCards[2]}");

        KingManager.Instance.DrawCards = drawCards;
        Debug.Log(KingManager.Instance.DrawCards);
    }

    private void CheckDeckCanvasStatus()
    {
        if (DeckCanvas.activeSelf && !previousActiveState)
        {
            // DeckCanvas가 방금 활성화되었을 때 실행되는 로직
            FightButton.SetActive(true);
            CardPhase.SetActive(false);

            previousActiveState = true;
        }
        else if (!DeckCanvas.activeSelf && previousActiveState)
        {
            // DeckCanvas가 방금 비활성화되었을 때 실행되는 로직
            FightButton.SetActive(false);
            CardPhase.SetActive(false);
            stringList = null;
            drawCards = new string[3];
            handCards = new string[3];

            previousActiveState = false;
        }
    }

    public void ClickFightButton()
    {
        FightButton.SetActive(false);

        stringList = new List<string>(GenerateArray()); // Initialize the list

        // Randomly select and remove elements for drawCards
        for (int i = 0; i < drawCards.Length; i++)
        {
            int randomIndex = Random.Range(0, stringList.Count);
            drawCards[i] = stringList[randomIndex];
            stringList.RemoveAt(randomIndex);
        }

        // Assign drawCards to the corresponding GameObjects (e.g., TextMeshPro or UI Text component)
        //drawCard1.GetComponentInChildren<TextMeshProUGUI>().text = drawCards[0];
        //drawCard2.GetComponentInChildren<TextMeshProUGUI>().text = drawCards[1];
        //drawCard3.GetComponentInChildren<TextMeshProUGUI>().text = drawCards[2];
        drawCard1.GetComponent<DragController>().cardIndex = 0;
        drawCard2.GetComponent<DragController>().cardIndex = 1;
        drawCard3.GetComponent<DragController>().cardIndex = 2;
        drawCard1.GetComponent<DragController>().isHandCard = false;
        drawCard2.GetComponent<DragController>().isHandCard = false;
        drawCard3.GetComponent<DragController>().isHandCard = false;

        // Assign the new drawCards to the corresponding GameObjects by loading images
        drawCard1.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Images/{drawCards[0]}");
        drawCard2.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Images/{drawCards[1]}");
        drawCard3.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Images/{drawCards[2]}");


        // Randomly select and remove elements for handCards
        for (int i = 0; i < handCards.Length; i++)
        {
            int randomIndex = Random.Range(0, stringList.Count);
            handCards[i] = stringList[randomIndex];
            stringList.RemoveAt(randomIndex);
        }

        // Assign handCards to the corresponding GameObjects
        //HandCard1.GetComponentInChildren<TextMeshProUGUI>().text = handCards[0];
        //HandCard2.GetComponentInChildren<TextMeshProUGUI>().text = handCards[1];
        //HandCard3.GetComponentInChildren<TextMeshProUGUI>().text = handCards[2];
        HandCard1.GetComponent<DragController>().cardIndex = 0;
        HandCard2.GetComponent<DragController>().cardIndex = 1;
        HandCard3.GetComponent<DragController>().cardIndex = 2;
        HandCard1.GetComponent<DragController>().isHandCard = true;
        HandCard2.GetComponent<DragController>().isHandCard = true;
        HandCard3.GetComponent<DragController>().isHandCard = true;

        HandCard1.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Images/{handCards[0]}");
        HandCard2.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Images/{handCards[1]}");
        HandCard3.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Images/{handCards[2]}");

        KingManager.Instance.DrawCards = drawCards;

        CardPhase.SetActive(true);
    }
}
