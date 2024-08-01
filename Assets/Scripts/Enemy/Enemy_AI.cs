using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AI : MonoBehaviour
{
    public float timeSinceLastAction;
    [SerializeField] float timeBetweenAction;

    [HideInInspector] public bool currentlyInAction;
    bool lastCardHoldFinished;
    bool _cardHoldingDone;
    public bool cardHoldingDone
    {
        get { return _cardHoldingDone; }
        set
        {
            _cardHoldingDone = value;
            if (value) AfterCardHold();
        }
    }
    [HideInInspector] public bool[] numberChecked = new bool[6];

    GameManager gameManager;
    Card_Manager cardManager;
    Dice_Generator diceGenerator;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        cardManager = GetComponent<Card_Manager>();
        diceGenerator = GetComponent<Dice_Generator>();
    }

    private void Start()
    {
        InvokeRepeating("Action", timeBetweenAction+3, timeBetweenAction);
    }

    private void Update()
    {
        if(gameManager.turn == GameManager.Turn.second)
        timeSinceLastAction += Time.deltaTime;
        if (currentlyInAction) timeSinceLastAction = 0;
    }
    void Action()
    {
        if (currentlyInAction) return;
        if (!diceGenerator.cardGeneratingDone) return; 
        if (gameManager.turn == GameManager.Turn.first) return;
        if (cardHoldingDone) return;
        if (!numberChecked[0])
        {
            numberChecked[0] = true;
            if(diceGenerator.numberAmount[0] > 0)
            {
                currentlyInAction = true;
                StartCoroutine(HoldCards(1));
            }          
            return;
        }
        if (!numberChecked[4])
        {
            numberChecked[4] = true;
            if(diceGenerator.numberAmount[4] > 0)
            {
                currentlyInAction = true;
                StartCoroutine(HoldCards(5));
            }      
            return;
        }
        for (int i = 1; i < 6; i++)
        {
            if (i == 4) continue;
            if(diceGenerator.numberAmount[i] >= 3 && !numberChecked[i])
            {
                numberChecked[i] = true;
                currentlyInAction = true;
                StartCoroutine(HoldCards(i+1));
            }
        }

        if(lastCardHoldFinished)
        cardHoldingDone = true;
    }
    public void ResetChecks()
    {
        for (int i = 0; i < 6; i++) numberChecked[i] = false;
    }

    IEnumerator HoldCards(int number)
    {
        currentlyInAction = true;
        lastCardHoldFinished = false;
        List<GameObject> cardsToHold = new List<GameObject>();
        for(int i = 0; i < 6; i++) if (diceGenerator.diceNumbers[i] == number)  cardsToHold.Add(cardManager.cardsNotInHold[i]);
        foreach(GameObject card in cardsToHold)
        {
            if (card != null)
            card.GetComponent<Card_Script>().MouseDown();
            yield return new WaitForSeconds(timeBetweenAction);
        }
        lastCardHoldFinished = true;
        currentlyInAction = false;
    }

    void AfterCardHold()
    {
        // WORK IN PROGRESS
        /* currentlyInAction = true;
        if (diceGenerator.remainingDiceInHand >= 3) diceGenerator.Reroll();
        else*/ diceGenerator.EndTurn(true);
    }
}
