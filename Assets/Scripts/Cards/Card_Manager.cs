using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Manager : MonoBehaviour
{
    [HideInInspector] public Vector2[] firstCardPositions = new Vector2[6];
    [HideInInspector] public Vector2[] firstCardsInHoldPositions = new Vector2[6];
    [HideInInspector] public Vector2[] secondCardPositions = new Vector2[6];
    [HideInInspector] public Vector2[] secondCardsInHoldPositions = new Vector2[6];
    [HideInInspector] public Vector2[] currentCardPositions;
    [HideInInspector] public Vector2[] currentCardsInHoldPositions;

    [HideInInspector] public GameObject[] cardsInHold = new GameObject[6];
    [HideInInspector] public GameObject[] cardsNotInHold = new GameObject[6];
    [HideInInspector] public GameObject[] cards = new GameObject[6];

    [HideInInspector] public bool[] holdPositionIsFull = new bool[6];

    [SerializeField] Transform firstStartingPos;
    [SerializeField] Transform firstSelectedCardPos;
    [SerializeField] Transform secondStartingPos;
    [SerializeField] Transform secondSelectedCardPos;


    GameManager gm;
    private void Awake()
    {
        gm = GetComponent<GameManager>();
        for(int i = 0; i < 6; i++)
        {
            firstCardPositions[i] = new Vector2(firstStartingPos.position.x + i, firstStartingPos.position.y);
            firstCardsInHoldPositions[i] = new Vector2(firstSelectedCardPos.position.x + i, firstSelectedCardPos.position.y);
            secondCardPositions[i] = new Vector2(secondStartingPos.position.x + i, secondStartingPos.position.y);
            secondCardsInHoldPositions[i] = new Vector2(secondSelectedCardPos.position.x + i, secondSelectedCardPos.position.y);
        }
    }

    private void Update()
    {
        currentCardPositions = gm.turn == GameManager.Turn.first ? firstCardPositions : secondCardPositions;
        currentCardsInHoldPositions = gm.turn == GameManager.Turn.first ? firstCardsInHoldPositions : secondCardsInHoldPositions;
    }
}
