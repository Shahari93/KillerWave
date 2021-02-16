using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    Scene scenes;
    public enum Scene
    {
        bootUP,
        titleScreen,
        shop,
        level1,
        level2,
        level3,
        gameOver
    }

    public void BeginGame()
    {
        SceneManager.LoadScene("testLevel");
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // means getting the value number of the scene.
    }

    public void GameOver()
    {
        SceneManager.LoadScene("gameOver");
    }
}
