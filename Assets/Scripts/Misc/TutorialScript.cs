using TMPro;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tutorialText;

    [SerializeField] GameObject spriteMask;
    [SerializeField] GameObject shuffleButton;
    [SerializeField] GameObject endTurnButton;

    const int tutorialCount = 16;
    string[] tutorialTexts = new string[tutorialCount];

    [SerializeField] Transform[] maskTransforms = new Transform[tutorialCount];

    int currentTutorialCount;

    private void Awake()
    {
        tutorialTexts[0] = "Welcome. This short tutorial will explain you how to play this game of cards";
        tutorialTexts[1] = "The goal of this two-player game is to be the first to reach a predetermined number of points. Six cards are drawn " +
            "and the players alternate turns.";
        tutorialTexts[2] = "Points are gained for every one or five, and for three or more of a kind of any other number. Scoring is as follows:";
        tutorialTexts[3] = "One is worth 100 points; Five is worth 50 points; three of a kind for any other number is worth 100 " +
            "points multiplied by the given number e.g 3 Fours are worth 400 points.";
        tutorialTexts[4] = "Four or more of a kind except one and five is worth double the points of three of a kind and so on " +
            "e.g 3 twos are worth 200, 4 twos are worth 400 and 5 twos are worth 800.";
        tutorialTexts[5] = "If you have no remaining cards, you are rewarded with an extra 75 points.";
        tutorialTexts[6] = "These are your cards in that turn. You can click on one and 'hold' it.";
        tutorialTexts[7] = "Holden cards will stay on the middle. You can click on them again to 'unhold' them.";
        tutorialTexts[8] = "When you are holding a valid combination of cards you have two options.";
        tutorialTexts[9] = "If you press shuffle, your points will be stored and all the cards you were holding will be destroyed. " +
            "The cards which you were not holding will be reshuffled.";
        tutorialTexts[10] = "This gives you the opportunity to earn more points.";
        tutorialTexts[11] = "However, if your new set of cards can not be made into a valid combination, you will loose are your stored points and your turn will end.";
        tutorialTexts[12] = "If you do not want to risk loosing your points, you can end your turn and your stored points will be permanently saved.";
        tutorialTexts[13] = "This is your stored points for that round.";
        tutorialTexts[14] = "This is your permanently saved points. The goal of the game is to get this number to 3000.";
        tutorialTexts[15] = "Good luck. You will need it.";
    }
    private void Start()
    {
        shuffleButton.SetActive(false);
        endTurnButton.SetActive(false);
        currentTutorialCount = 0;
        ChangeTutorialCountBy(0);
    }

    public void ChangeTutorialCountBy(int i)
    {
        if (currentTutorialCount < tutorialCount - 1) currentTutorialCount += i;
        else
        {
            GetComponent<SceneLoader>().LoadScene(0);
        }
        bool b = currentTutorialCount > 7 && currentTutorialCount < 13;
        shuffleButton.SetActive(b);  
        endTurnButton.SetActive(b);  
        
        tutorialText.text = tutorialTexts[currentTutorialCount];
        spriteMask.transform.position = maskTransforms[currentTutorialCount].position;
        spriteMask.transform.localScale = maskTransforms[currentTutorialCount].localScale;
    }
}
