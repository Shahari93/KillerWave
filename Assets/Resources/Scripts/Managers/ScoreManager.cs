using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static int playerScore; //we don't want other classes to have access. It's also static because we don't need duplicate references for this variable.
    public int PlayerScore
    {
        get
        {
            return playerScore;
        }
    }

    public void SetScore(int incomingScore)
    {
        playerScore += incomingScore;
        if (GameObject.Find("Score"))
        {
            GameObject.Find("Score").GetComponent<Text>().text = playerScore.ToString();
        }
    }

    public void ResetScore()
    {
        playerScore = 00000000;
        if (GameObject.Find("Score"))
        {
            GameObject.Find("Score").GetComponent<Text>().text = playerScore.ToString();
        }
    }
}
