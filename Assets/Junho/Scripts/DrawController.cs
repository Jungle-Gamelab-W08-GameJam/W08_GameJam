using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DrawController : MonoBehaviour
{
    public GameObject drawCard1;
    public GameObject drawCard2;
    public GameObject drawCard3;
    public GameObject HandCard1;
    public GameObject HandCard2;
    public GameObject HandCard3;

    private List<string> stringList; // Changed to List for easier manipulation
    private string[] drawCards = new string[3];
    private string[] handCards = new string[3];


    // Start is called before the first frame update
    void Start()
    {
        stringList = new List<string>(GenerateArray()); // Initialize the list

        // Randomly select and remove elements for drawCards
        for (int i = 0; i < drawCards.Length; i++)
        {
            int randomIndex = Random.Range(0, stringList.Count);
            drawCards[i] = stringList[randomIndex];
            stringList.RemoveAt(randomIndex);
        }

        // Assign drawCards to the corresponding GameObjects (e.g., TextMeshPro or UI Text component)
        drawCard1.GetComponentInChildren<TextMeshProUGUI>().text = drawCards[0];
        drawCard2.GetComponentInChildren<TextMeshProUGUI>().text = drawCards[1];
        drawCard3.GetComponentInChildren<TextMeshProUGUI>().text = drawCards[2];
        drawCard1.GetComponent<DragController>().cardIndex = 0;
        drawCard2.GetComponent<DragController>().cardIndex = 1;
        drawCard3.GetComponent<DragController>().cardIndex = 2;
        drawCard1.GetComponent<DragController>().isHandCard = false;
        drawCard2.GetComponent<DragController>().isHandCard = false;
        drawCard3.GetComponent<DragController>().isHandCard = false;


        // Randomly select and remove elements for handCards
        for (int i = 0; i < handCards.Length; i++)
        {
            int randomIndex = Random.Range(0, stringList.Count);
            handCards[i] = stringList[randomIndex];
            stringList.RemoveAt(randomIndex);
        }

        // Assign handCards to the corresponding GameObjects
        HandCard1.GetComponentInChildren<TextMeshProUGUI>().text = handCards[0];
        HandCard2.GetComponentInChildren<TextMeshProUGUI>().text = handCards[1];
        HandCard3.GetComponentInChildren<TextMeshProUGUI>().text = handCards[2];
        HandCard1.GetComponent<DragController>().cardIndex = 0;
        HandCard2.GetComponent<DragController>().cardIndex = 1;
        HandCard3.GetComponent<DragController>().cardIndex = 2;
        HandCard1.GetComponent<DragController>().isHandCard = true;
        HandCard2.GetComponent<DragController>().isHandCard = true;
        HandCard3.GetComponent<DragController>().isHandCard = true;
    }

    // Update is called once per frame
    void Update()
    {

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
        drawCard1.GetComponentInChildren<TextMeshProUGUI>().text = drawCards[0];
        drawCard2.GetComponentInChildren<TextMeshProUGUI>().text = drawCards[1];
        drawCard3.GetComponentInChildren<TextMeshProUGUI>().text = drawCards[2];



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
        Debug.Log(drawCards[0] + drawCards[1] + drawCards[2] + handCards[0] + handCards[1] + handCards[2]);


        // Assign the new drawCards to the corresponding GameObjects (e.g., TextMeshPro or UI Text component)
        drawCard1.GetComponentInChildren<TextMeshProUGUI>().text = drawCards[0];
        drawCard2.GetComponentInChildren<TextMeshProUGUI>().text = drawCards[1];
        drawCard3.GetComponentInChildren<TextMeshProUGUI>().text = drawCards[2];
        HandCard1.GetComponentInChildren<TextMeshProUGUI>().text = handCards[0];
        HandCard2.GetComponentInChildren<TextMeshProUGUI>().text = handCards[1];
        HandCard3.GetComponentInChildren<TextMeshProUGUI>().text = handCards[2];

    }
}
