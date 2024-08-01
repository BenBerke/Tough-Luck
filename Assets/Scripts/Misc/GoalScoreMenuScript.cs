using TMPro;
using UnityEngine;

public class GoalScoreMenuScript : MonoBehaviour
{
    int score;

    [SerializeField] TextMeshProUGUI scoreText;

    private void Start()
    {
        score = PlayerPrefs.GetInt("destScore");
        ChangeScore(0);
    }
    public void ChangeScore(int i)
    {
        if (i < 0 && score + i <= 0) return;
        score += i;
        PlayerPrefs.SetInt("destScore", score);
        scoreText.text = score.ToString();
    }

    public void PlayerPrefSaveScore()
    {
        PlayerPrefs.SetInt("destScore", score);
    }
}
