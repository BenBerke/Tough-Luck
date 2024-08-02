
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject winMenuParent;
    [SerializeField] TextMeshProUGUI winText;

    public Turn turn;

    int _firstScore;
    public int firstScore
    {
        get { return _firstScore; }
        set
        {
            _firstScore = value;
            CheckForScore();
        }
    }
    int _secondScore;
    public int secondScore
    {
        get { return _secondScore; }
        set
        {
            _secondScore = value;
            CheckForScore();
        }
    }

    int destScore;

    public bool enemyActive;

    [SerializeField] TextMeshProUGUI firstScoreText;

    public enum Turn
    {
        first,
        second
    }

    private void Start()
    {
        destScore = PlayerPrefs.GetInt("destScore");
        if (PlayerPrefs.GetInt("ai") == 1) enemyActive = true;
        else enemyActive = false;
        winMenuParent.SetActive(false);
        turn = Turn.first;
        if (enemyActive) GetComponent<Enemy_AI>().enabled = true;
        else GetComponent<Enemy_AI>().enabled = false;
    }

    private void Update()
    {
        firstScoreText.text = firstScore.ToString();
    }

    void CheckForScore()
    {
        if (_firstScore >= destScore) Win(true);
        if (_secondScore >= destScore) Win(false);
    }

    void Win(bool firstPlayer)
    {
        GetComponent<Enemy_AI>().enabled = false;
        winText.text = firstPlayer ? (enemyActive ? "You Won!" : "First Player Won!") : (enemyActive ? "Tough Luck!" : "Second Player Won!");
        winMenuParent.SetActive(true);
        Destroy(gameObject);
    }
}
