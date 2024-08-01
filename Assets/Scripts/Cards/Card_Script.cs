using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Script : MonoBehaviour
{
    public int index;
    [HideInInspector] public int holdIndex;

    [SerializeField] BoxCollider2D fullCol;
    [SerializeField] BoxCollider2D halfCol;

    Dice_Generator generator;
    Card_Manager cardManager;
    GameManager gameManager;
    SpriteRenderer sr;

    [HideInInspector] public bool selected;
    [HideInInspector] public bool moveOut;
    bool mouseOver;
    bool moveToHoldPos;
    public bool useFullCol;

    Vector2 holdPos;
    Transform backCard;
    private void Start()
    {
        selected = false;
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        GameObject gm = GameObject.FindGameObjectWithTag("GameManager");
        backCard = GameObject.FindGameObjectWithTag("BackCard").transform;
        gameManager = gm.GetComponent<GameManager>();
        generator = gm.GetComponent<Dice_Generator>();
        cardManager = gm.GetComponent<Card_Manager>();
        sr.flipY = gameManager.turn == GameManager.Turn.second;
        sr.flipX = sr.flipY;
        transform.position = backCard.position;
        transform.localScale = new Vector2(.94f, .94f);
        cardManager.cardsNotInHold[index] = gameObject;
    }

    private void Update()
    {
        fullCol.enabled = useFullCol;
        halfCol.enabled = !useFullCol;
        if (selected)
        {
            if (holdIndex == 5) useFullCol = true;
            else if (!cardManager.holdPositionIsFull[holdIndex + 1]) useFullCol = true;
            else useFullCol = false;
        }
        else
        {
            useFullCol = index == 5 || generator.currentlyInHold[index + 1];
            transform.position = Vector2.MoveTowards(transform.position, cardManager.currentCardPositions[index], generator.cardMoveSpeed * Time.deltaTime);
        }

        if (!mouseOver) sr.sortingOrder = selected ? holdIndex : index;
        if (moveToHoldPos) transform.position = Vector2.MoveTowards(transform.position, holdPos, generator.cardMoveSpeed * Time.deltaTime);
    }
    private void OnMouseEnter()
    {
        if (gameManager.turn == GameManager.Turn.second && gameManager.enemyActive) return;
        mouseOver = true;
        sr.sortingOrder = 10;

        int multiplier = gameManager.turn == GameManager.Turn.first ? 1 : -1;
        if (selected) sr.gameObject.transform.position = new Vector2(transform.position.x, transform.position.y + -.45f * multiplier);
        else sr.gameObject.transform.position = new Vector2(transform.position.x, transform.position.y + .45f * multiplier);
    }
    private void OnMouseExit()
    {
        mouseOver = false;
        sr.gameObject.transform.position = new Vector2(transform.position.x, transform.position.y);
    }
    void OnMouseDown()
    {
        if (gameManager.turn == GameManager.Turn.second && gameManager.enemyActive) return;
        MouseDown();
    }

    public void MouseDown()
    {
        generator.InteractDie(index);
        if (PlayerPrefs.GetInt("sfx") == 1)
            generator.GetComponent<AudioSource>().Play();
        moveToHoldPos = !moveToHoldPos;
        if (selected)
        {
            cardManager.holdPositionIsFull[holdIndex] = false;
            cardManager.cardsNotInHold[index] = gameObject;
        }
        else
        {
            cardManager.cardsNotInHold[index] = null;
            for (int i = 0; i < cardManager.currentCardsInHoldPositions.Length; i++)
            {
                if (!cardManager.holdPositionIsFull[i])
                {
                    holdIndex = i;
                    holdPos = cardManager.currentCardsInHoldPositions[i];
                    cardManager.holdPositionIsFull[i] = true;
                    cardManager.cardsInHold[i] = gameObject;
                    break;
                }
            }
        }
        selected = !selected;
    }
}
