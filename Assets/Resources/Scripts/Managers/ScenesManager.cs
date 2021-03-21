using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager : MonoBehaviour
{
    float gameTimer = 0f; // current counter to time how long the level has left until it is over
    float[] endLevelTimer = { 30f, 30f, 45f }; // holds the time until each level ends
    int currentSceneNumber = 0; // hold the number that denotes which scene our player is currently on
    bool gameEnding = false; // trigger the end of the level animation for the player's ship


    public Scene scenes;
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

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene aScene, LoadSceneMode aMode)
    {
        GetComponent<GameManager>().SetLivesDisplay(GameManager.playerLives);
        // If the score game object is in the scene
        if(GameObject.Find("Score"))
        {
            GameObject.Find("Score").GetComponent<Text>().text = GetComponent<ScoreManager>().PlayerScore.ToString();
        }
    }

    private void Update()
    {
        if (currentSceneNumber != SceneManager.GetActiveScene().buildIndex)
        {
            currentSceneNumber = SceneManager.GetActiveScene().buildIndex;
            GetScene();
        }
        GameTimer();
    }

    private void GetScene()
    {
        scenes = (Scene)currentSceneNumber; // we are casting the current scene numer (int variable) as an enum
    }

    // keep track of our game's time
    private void GameTimer()
    {
        switch (scenes)
        {
            case Scene.level1:
            case Scene.level2:
            case Scene.level3:
                {
                    // if level is not completed
                    if (gameTimer < endLevelTimer[currentSceneNumber - 3]) //We only need to know what build index number levels 1, 2, and 3 are on. So, to avoid the first three scenes, we must subtract by 3.
                    {
                        gameTimer += Time.deltaTime;
                    }
                    // if level is completed
                    else
                    {
                        if (!gameEnding)
                        {
                            gameEnding = true;
                            // if scene is not level 3
                            if (SceneManager.GetActiveScene().name != "level3")
                            {
                                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTransition>().LevelEnd = true;
                            }
                            // if scene is level 3
                            else
                            {
                                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTransition>().GameCompleted = true;
                            }
                            Invoke("NextLevel", 4);
                        }
                    }
                }
                break;
        }
    }

    public void BeginGame(int gameLevel)
    {
        SceneManager.LoadScene(gameLevel);
    }

    public void ResetScene()
    {
        gameTimer = 0f;
        SceneManager.LoadScene(GameManager.currentSceneIndex);
    }

    public void NextLevel()
    {
        gameEnding = false;
        gameTimer = 0f;
        SceneManager.LoadScene(GameManager.currentSceneIndex + 1);
    }

    public void GameOver()
    {
        SceneManager.LoadScene("gameOver");
    }
}
