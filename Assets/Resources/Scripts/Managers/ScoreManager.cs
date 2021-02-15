using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    static int playerScore; //we don't want other classes to have access. It's also static because we don't need duplicate references for this variable.
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
    }

    public void ResetScore()
    {
        playerScore = 00000000;
    }
}
