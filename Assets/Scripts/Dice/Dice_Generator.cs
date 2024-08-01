using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Dice_Generator : MonoBehaviour
{
    [HideInInspector] public int[] diceNumbers = new int[diceAmount];
    [HideInInspector] public int[] numberAmount = new int[diceAmount];
    int[] currentDiceHold = new int[diceAmount];
    int[] numberAmountHold = new int[diceAmount];

    [HideInInspector] public bool[] diceInRound = new bool[diceAmount];
    [HideInInspector] public bool[] currentlyInHold = new bool[diceAmount];

    const int diceAmount = 6;
    [HideInInspector] public int roundScore;
    [HideInInspector] public int heldScore;
    [HideInInspector] public int savedScore;

    [HideInInspector] public int remainingDiceAmount; // Total cards in that round
    [HideInInspector] public int remainingDiceInHand; // Cards that are not in the middle
    [HideInInspector] public int diceHeld; // Cards in the middle

    [SerializeField] float secondsInBetweenCardSpawns;
    public float cardMoveSpeed;

    bool firstTurn;
    public bool cardGeneratingDone;

    [SerializeField] GameObject card;
    [SerializeField] Transform cardSpawnPosition;
    [SerializeField] Sprite[] cardSprites;

    [SerializeField] TextMeshProUGUI firstHeldScoreText;
    [SerializeField] TextMeshProUGUI firstSavedScoreText;
    [SerializeField] TextMeshProUGUI secondHeldScoreText;
    [SerializeField] TextMeshProUGUI secondSavedScoreText;
    [SerializeField] GameObject reRollButton;
    [SerializeField] GameObject endTurnButton;
    [SerializeField] Image reRollCrossMark;
    [SerializeField] Image endTurnCrossMark;

    GameManager gameManager;
    Card_Manager cardManager;
    AnimationManager animManager;
    Enemy_AI enemyAI;
    AudioSource audioSource;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        cardManager = GetComponent<Card_Manager>();
        animManager = Camera.main.GetComponent<AnimationManager>();
        enemyAI = GetComponent<Enemy_AI>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        firstTurn = true;
        StartTurn();
    }

    private void Update()
    {
        bool aiTurn = gameManager.turn == GameManager.Turn.second && gameManager.enemyActive;
        bool canEndTurn = (IsDiceHoldValid() || diceHeld == 0) && cardGeneratingDone;
        reRollCrossMark.gameObject.SetActive(!(IsDiceHoldValid() && cardGeneratingDone) || remainingDiceInHand == 0);
        reRollButton.GetComponent<Button>().enabled = IsDiceHoldValid() && cardGeneratingDone && remainingDiceInHand > 0;
        endTurnCrossMark.gameObject.SetActive(!canEndTurn);
        endTurnButton.GetComponent<Button>().enabled = canEndTurn;
        reRollButton.SetActive(!animManager.animPlaying && !aiTurn);
        endTurnButton.SetActive(!animManager.animPlaying && !aiTurn);

        firstSavedScoreText.text = gameManager.firstScore.ToString();
        secondSavedScoreText.text = gameManager.secondScore.ToString();

        string s = (heldScore + roundScore).ToString();
        firstHeldScoreText.text = s;
        secondHeldScoreText.text = s;
        firstHeldScoreText.gameObject.SetActive(gameManager.turn == GameManager.Turn.first);
        secondHeldScoreText.gameObject.SetActive(gameManager.turn == GameManager.Turn.second);
    }
    void StartTurn()
    {
        cardGeneratingDone = false;
        if (!firstTurn)
        {
            if (gameManager.turn == GameManager.Turn.second) StartCoroutine(animManager.Rotate(false));
            else StartCoroutine(animManager.Rotate(true));
        }

        for (int i = 0; i < diceAmount; i++)
        {
            numberAmountHold[i] = 0;
            currentDiceHold[i] = 0;
            currentlyInHold[i] = false;
            diceInRound[i] = true;
            enemyAI.ResetChecks();
        }
        remainingDiceAmount = diceAmount;
        StartCoroutine(GenerateDice());
    }
    IEnumerator GenerateDice()
    {
        remainingDiceInHand = 0;
        yield return new WaitUntil(() => !animManager.animPlaying);
        for (int i = 0; i < diceAmount; i++)
        {
            if (diceInRound[i]) 
            {
                remainingDiceInHand++;
                diceNumbers[i] = Random.Range(1, 7);
                GameObject card = Instantiate(this.card, cardSpawnPosition.transform);
                if(PlayerPrefs.GetInt("sfx") == 1)
                audioSource.Play();
                cardManager.cards[i] = card;
                SpriteRenderer sr = card.transform.GetChild(0).GetComponent<SpriteRenderer>();
                Card_Script cs = card.GetComponent<Card_Script>();
                card.GetComponent<Card_Script>().index = i;
                card.name = $"Index: {cs.index} Value: {diceNumbers[i]}";
                sr.sprite = cardSprites[diceNumbers[i]-1];
                sr.sortingOrder = i;
                numberAmount[diceNumbers[i]-1]++;
            }
            else diceNumbers[i] = 0;

            if (i + 1 != diceAmount) if (diceInRound[i + 1]) yield return new WaitForSeconds(secondsInBetweenCardSpawns);
        }
        cardGeneratingDone = true;
        enemyAI.currentlyInAction = false;
        if (!IsValidHand() && remainingDiceInHand != 0) EndTurn(false);
    }

    bool IsValidHand()
    {
        bool enoughTwo = numberAmount[1] >= 3;
        bool enoughThree = numberAmount[2] >= 3;
        bool enoughFour = numberAmount[3] >= 3;
        bool enoughSix = numberAmount[5] >= 3;

        if (numberAmount[0] > 0 || numberAmount[4] > 0 || enoughTwo || enoughThree || enoughFour || enoughSix || remainingDiceAmount == 0) return true;
        return false;
    }

    public void InteractDie(int dieIndex) 
    {
        if (currentlyInHold[dieIndex]) UnholdDie(dieIndex);
        else  HoldDie(dieIndex);
        currentlyInHold[dieIndex] = !currentlyInHold[dieIndex];
    }
    void HoldDie(int dieIndex)
    {
        diceHeld++;
        remainingDiceInHand--;
        currentDiceHold[dieIndex] = diceNumbers[dieIndex];
        numberAmountHold[diceNumbers[dieIndex]-1]++;
        roundScore = CalculateScore();
    }
    void UnholdDie(int dieIndex)
    {
        diceHeld--;
        remainingDiceInHand++;
        currentDiceHold[dieIndex] = 0;
        numberAmountHold[diceNumbers[dieIndex] - 1]--;
        roundScore = CalculateScore();
    }

    void ResetCards()
    {
        roundScore = 0;
        enemyAI.cardHoldingDone = false;
        for (int i = 0; i < diceAmount; i++)
        {
            cardManager.holdPositionIsFull[i] = false;
            currentDiceHold[i] = 0;
            numberAmount[i] = 0;
            numberAmountHold[i] = 0;
        }
        foreach (GameObject c in cardManager.cards) Destroy(c);
    }
    public bool IsDiceHoldValid()
    {
        if (diceHeld == 0) return false;

        bool enoughTwo = numberAmountHold[1] >= 3 || numberAmountHold[1] == 0;
        bool enoughThree = numberAmountHold[2] >= 3 || numberAmountHold[2] == 0;
        bool enoughFour = numberAmountHold[3] >= 3 || numberAmountHold[3] == 0;
        bool enoughSix = numberAmountHold[5] >= 3 || numberAmountHold[5] == 0;

        if ((numberAmountHold[0] >= 0 || numberAmountHold[4] >= 0) && enoughTwo && enoughThree && enoughFour && enoughSix && roundScore > 0 || remainingDiceInHand == diceAmount) return true;
        return false;
    }

    int CalculateScore()
    {
        int sum = 0;
        sum += numberAmountHold[0] * 100;
        sum += numberAmountHold[4] * 50;

        for(int i = 1; i < diceAmount; i++)
        {
            if (i == 4) continue;
            if (numberAmountHold[i] >= 3) sum += (i + 1) * 100 * (int)Mathf.Pow(2, numberAmountHold[i] - 3);
        }

        if (remainingDiceInHand == 0) sum += 75;
        return sum;
    }
    public void Reroll()
    {
        cardGeneratingDone = false;
        diceHeld = 0;
        int d = 0;
        heldScore += roundScore;
        roundScore = 0;
        for(int i = 0; i < diceAmount; i++)
        {
            numberAmount[i] = 0;
            if (currentlyInHold[i])
            {
                diceInRound[i] = false;
                d++;
            }
        }
        remainingDiceAmount = 6-d;
        if (remainingDiceAmount == 0) EndTurn(true);
        ResetCards();
        StartCoroutine(GenerateDice());
    }
    public void EndTurn(bool saveScore)
    {
        remainingDiceInHand = 0;
        firstTurn = false;
        if (saveScore)
        {
            savedScore += heldScore;
            if (gameManager.turn == GameManager.Turn.first) gameManager.firstScore += savedScore + roundScore;
            else gameManager.secondScore += savedScore + roundScore;
        }
        gameManager.turn = gameManager.turn == GameManager.Turn.first ? GameManager.Turn.second : GameManager.Turn.first;
        diceHeld = 0;
        heldScore = 0;
        roundScore = 0;
        savedScore = 0;
        ResetCards();
        StartTurn();
    }
}
