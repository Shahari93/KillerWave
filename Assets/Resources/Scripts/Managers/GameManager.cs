using System;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    GameObject mainCamera;
    GameObject dirLight;
    public static int playerLives = 3; // how many lifes the player have at the start
    public static int currentSceneIndex = 0; //keep the number of the current scene we are on
    public static int gameplaySceneInBuild = 3; //hold the first level we play, which we will use later on

    private bool died = false;
    public bool Died
    {
        get
        {
            return died;
        }
        set
        {
            died = value;
        }
    }

    static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").gameObject;
        dirLight = GameObject.Find("Directional Light"); // Grabs the light from the scene and stores it as a reference.
        CheckGameManagerIsInTheScene(); // Call the singleton method
        currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex; // getting the current build index
        LightAndCameraSetup(currentSceneIndex);
    }

    private void Start()
    {
        SetLivesDisplay(playerLives);
    }

    public void SetLivesDisplay(int livesAmount)
    {
        if (GameObject.Find("lives")) // checking if there is a game object named lives
        {
            GameObject lives = GameObject.Find("lives"); // store it in a gameobject
            if (lives.transform.childCount < 1) // if it has no child
            {
                for (int i = 0; i < livesAmount; i++)
                {
                    GameObject life = GameObject.Instantiate(Resources.Load("Prefabs/life")) as GameObject; // instantiate 4 life game objects
                    life.transform.SetParent(lives.transform);
                }
            }
            // set visuals
            for (int i = 0; i < lives.transform.childCount; i++)
            {
                lives.transform.GetChild(i).localScale = new Vector3(1, 1, 1);
            }

            // remove visuals
            for (int i = 0; i < (lives.transform.childCount - livesAmount); i++)
            {
                lives.transform.GetChild(lives.transform.childCount - i - 1 /*- livesAmount*/).localScale = Vector3.zero;
            }
        }
    }

    private void CheckGameManagerIsInTheScene() // Our singleton pattern
    {
        if (instance == null) // if we don't have any GameManager class in the scene
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject); // if we have a GameManager class in the scene
        }
        DontDestroyOnLoad(this); // We are not destroying the game manager when moving between scenes
    }

    private void LightAndCameraSetup(int sceneNumber)
    {
        switch (sceneNumber)
        {
            case 3:
            case 4:
            case 5:
                {
                    CameraSetup();
                    LightSetup();
                    break;
                }
        }
    }

    private void LightSetup()
    {
        dirLight.transform.eulerAngles = new Vector3(50, -30, 0); // It sets the rotation of the light with EulerAngles.
        dirLight.GetComponent<Light>().color = new Color32(152, 204, 255, 255); // Finally, it changes the light's color.
    }

    private void CameraSetup()
    {
        //Camera Transform
        mainCamera.transform.position = new Vector3(0, 0, -300f);
        mainCamera.transform.eulerAngles = new Vector3(0, 0, 0);

        //Camera Properties
        mainCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor; // removes the skybox and set a solid color
        mainCamera.GetComponent<Camera>().backgroundColor = new Color(0, 0, 0, 255); // change the solid color to black
    }

    public void LiveLost() // method that handle what happens when the player loses lives
    {
        if (playerLives >= 1)
        {
            playerLives--;
            GetComponent<ScenesManager>().ResetScene(); // getting component from the GameManager Game Object
        }
        else
        {
            playerLives = 3; // reseting the player lives back to 3
            GetComponent<ScenesManager>().GameOver();
        }
    }
}
