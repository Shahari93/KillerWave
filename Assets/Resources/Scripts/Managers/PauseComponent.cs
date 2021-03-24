using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PauseComponent : MonoBehaviour
{
    [SerializeField] GameObject pauseScreen = null;
    [SerializeField] private float delayTime = 2.5f;

    private void Awake()
    {
        pauseScreen.SetActive(false);
        SetPauseButtonActive(false);
        Invoke("DelayPauseAppear", delayTime); // delay for 5 seconds until we run the DelayPauseAppear method
    }

    private void SetPauseButtonActive(bool switchButton)
    {
        //the colors of the pause button. ColorBlock holds all the colors in the button component (normalColor, highlightedColor, pressedColor, and disabledColor)
        ColorBlock col = GetComponentInChildren<Toggle>().colors;

        if (switchButton == false)
        {
            col.normalColor = new Color32(0, 0, 0, 0);
            col.highlightedColor = new Color32(0, 0, 0, 0);
            col.pressedColor = new Color32(0, 0, 0, 0);
            col.disabledColor = new Color32(0, 0, 0, 0);
            GetComponentInChildren<Toggle>().interactable = false;
        }
        else
        {
            col.normalColor = new Color32(245, 245, 245, 255);
            col.highlightedColor = new Color32(245, 245, 245, 255);
            col.pressedColor = new Color32(200, 200, 200, 255);
            col.disabledColor = new Color32(200, 200, 200, 128);
            GetComponentInChildren<Toggle>().interactable = true;
        }

        GetComponentInChildren<Toggle>().colors = col;
        GetComponentInChildren<Toggle>().transform.GetChild(0).gameObject.SetActive(switchButton);
    }

    private void DelayPauseAppear()
    {
        SetPauseButtonActive(true);
    }

    public void PauseGame()
    {
        pauseScreen.SetActive(true); //We set the pause screen game object's activity to true
        SetPauseButtonActive(false); //Turn off the pause button (because we have the QUIT button to use instead).
        Time.timeScale = 0f; // Set the game's timeScale to zero, which will stop all moving, animating objects in the scene.
    }
    //TODO: Set a timer that show the player when he can come back to play
    public void ResumeGame()
    {
        pauseScreen.SetActive(false); //We set the pause screen game object's activity to true
        SetPauseButtonActive(true); //Turn off the pause button (because we have the QUIT button to use instead).
        Time.timeScale = 1f; // Set the game's timeScale to zero, which will stop all moving, animating objects in the scene.
    }
    //TODO: Add a new screen that ask the player if he sure that he wants to quit
    public void QuitGame()
    {
        Time.timeScale = 1f; // Set the game's timeScale to zero, which will stop all moving, animating objects in the scene.
        GameManager.Instance.GetComponent<ScoreManager>().ResetScore();
        GameManager.Instance.GetComponent<ScenesManager>().BeginGame(0);
    }
}
