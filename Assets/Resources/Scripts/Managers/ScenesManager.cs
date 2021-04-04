using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager : MonoBehaviour
{
    float gameTimer = 0f; // current counter to time how long the level has left until it is over
    float[] endLevelTimer = { 30f, 30f, 45f }; // holds the time until each level ends
    int currentSceneNumber = 0; // hold the number that denotes which scene our player is currently on
    bool gameEnding = false; // trigger the end of the level animation for the player's ship


    [HideInInspector] public Scene scenes;
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
    [HideInInspector] public MusicMode musicMode;
    public enum MusicMode
    {
        startLevelMusic, //The music will be playing and will be set to its maximum volume
        endLevelMusic, // No music is playing
        fadeOut // The music's volume will fade to zero
    }

    private void Start()
    {
        StartCoroutine(MusicVolume(MusicMode.startLevelMusic));
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene aScene, LoadSceneMode aMode)
    {
        StartCoroutine(MusicVolume(MusicMode.startLevelMusic));
        GetComponent<GameManager>().SetLivesDisplay(GameManager.playerLives);
        // If the score game object is in the scene
        if (GameObject.Find("Score"))
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

    IEnumerator MusicVolume(MusicMode musicMode)
    {
        switch (musicMode)
        {
            case MusicMode.endLevelMusic:
                GetComponentInChildren<AudioSource>().Stop();
                break;
            case MusicMode.fadeOut:
                GetComponentInChildren<AudioSource>().volume -= Time.deltaTime / 3;
                break;
            case MusicMode.startLevelMusic:
                if (GetComponentInChildren<AudioSource>().clip != null)
                {
                    GetComponentInChildren<AudioSource>().Play();
                    GetComponentInChildren<AudioSource>().volume = 1f;
                }
                break;
        }
        yield return new WaitForSeconds(0.1f); // we set 0.1f to give the game time to change the settings from the switch statement
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
                    if (!GetComponentInChildren<AudioSource>().clip)
                    {
                        AudioClip levelMusic = Resources.Load<AudioClip>("Sound/lvlMusic") as AudioClip;
                        GetComponentInChildren<AudioSource>().clip = levelMusic;
                        GetComponentInChildren<AudioSource>().Play();
                    }
                    // if level is not completed
                    if (gameTimer < endLevelTimer[currentSceneNumber - 3]) //We only need to know what build index number levels 1, 2, and 3 are on. So, to avoid the first three scenes, we must subtract by 3.
                    {
                        gameTimer += Time.deltaTime;
                    }
                    // if level is completed
                    else
                    {
                        StartCoroutine(MusicVolume(MusicMode.fadeOut));
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
                            // TODO: change the parameter from string to int, and find how to get the last scene from the build settings
                            SendInJsonFormat(SceneManager.GetActiveScene().name);


                            Invoke("NextLevel", 4);
                        }
                    }
                }
                break;
            default:
                {
                    GetComponentInChildren<AudioSource>().clip = null;
                    break;
                }
        }
    }


    // Only if the player finished the game
    private void SendInJsonFormat(string lastLevel)
    {
        if(lastLevel == "level3")
        {
            // Write in the data for the Json format
            GameStats gameStats = new GameStats();
            gameStats.completeData = System.DateTime.Now.ToString(); // getting the real date of when the player completed the game
            gameStats.score = ScoreManager.playerScore;
            gameStats.livesLeft = GameManager.playerLives;
            string json = JsonUtility.ToJson(gameStats,true); // set to true because we want to be able to read the data
            Debug.Log(json);
            Debug.Log(Application.persistentDataPath + "/GameStatsSaved.json"); // prints to the console where we save the json file
            System.IO.File.WriteAllText(Application.persistentDataPath + "/GameStatsSaved.json", json); // where we save the json file
        }
    }

    public void BeginGame(int gameLevel)
    {
        SceneManager.LoadScene(gameLevel);
    }

    public void ResetScene()
    {
        StartCoroutine(MusicVolume(MusicMode.endLevelMusic));
        gameTimer = 0f;
        SceneManager.LoadScene(GameManager.currentSceneIndex);
    }

    public void NextLevel()
    {
        gameEnding = false;
        gameTimer = 0f;
        SceneManager.LoadScene(GameManager.currentSceneIndex + 1);
        StartCoroutine(MusicVolume(MusicMode.startLevelMusic));
    }

    public void GameOver()
    {
        SceneManager.LoadScene("gameOver");
    }
}
