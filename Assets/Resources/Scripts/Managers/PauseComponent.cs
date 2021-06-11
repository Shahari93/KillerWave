using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio; // in order to get the audio mixer we need to auido library

public class PauseComponent : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer = null;
    [SerializeField] GameObject musicSlider = null;
    [SerializeField] GameObject effectsSlider = null;
    [SerializeField] GameObject pauseScreen = null;
    [SerializeField] GameObject areYouSureScreen = null;
    [SerializeField] private float delayTime = 2.5f;
    [SerializeField] Text timerText = null;
    public string[] messages;
    public float intervalTime = 1;

    private void Awake()
    {
        pauseScreen.SetActive(false);
        areYouSureScreen.SetActive(false);
        SetPauseButtonActive(false);
        timerText.gameObject.SetActive(false);
        Invoke("DelayPauseAppear", delayTime); // delay for X seconds until we run the DelayPauseAppear method

        //we are reapplying our saved PlayerPrefs values for our music and effects volume
        audioMixer.SetFloat("musicVol", PlayerPrefs.GetFloat("musicVolome"));
        audioMixer.SetFloat("effectVol", PlayerPrefs.GetFloat("effectVolome"));

        //setting the value from the player prefs to the music slider
        musicSlider.GetComponent<Slider>().value = GetMusicValueFromMixer();
        effectsSlider.GetComponent<Slider>().value = GetEffectsValueFromMixer();
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

    public void ResumeGame()
    {
        StartCoroutine(Timer()); // when the button is pressed the timer coroutine will work
    }

    public void AreYouSure()
    {
        Time.timeScale = 1f; // Set the game's timeScale to zero, which will stop all moving, animating objects in the scene.
        GameManager.Instance.GetComponent<ScoreManager>().ResetScore();
        GameManager.Instance.GetComponent<ScenesManager>().BeginGame(0);
    }

    public void QuitGame()
    {
        //Time.timeScale = 1f; // Set the game's timeScale to zero, which will stop all moving, animating objects in the scene.
        pauseScreen.SetActive(false);
        areYouSureScreen.SetActive(true);
        GameManager.playerLives = 3;
    }

    /// Timer
    public IEnumerator Timer()
    {
        timerText.gameObject.SetActive(true);
        int messageDisplay = messages.Length - 1; // start at 3
        while (messageDisplay >= 0)
        {
            timerText.text = messages[messageDisplay];
            yield return new WaitForSecondsRealtime(intervalTime);
            messageDisplay -= 1;
            if (messageDisplay <= 0)
            {
                SetPauseButtonActive(true); //Turn off the pause button (because we have the QUIT button to use instead).
                timerText.gameObject.SetActive(false);
                pauseScreen.SetActive(false); //We set the pause screen game object's activity to true
                Time.timeScale = 1f; // Set the game's timeScale to zero, which will stop all moving, animating objects in the scene.
                if (areYouSureScreen.activeSelf)
                {
                    areYouSureScreen.SetActive(false);
                    timerText.gameObject.SetActive(false);
                }
            }
        }
    }

    #region setting and getting sliders value
    public void SetMusicVolumeFromSlider()
    {
        audioMixer.SetFloat("musicVol"/*musicVol is the name of the music mixer*/, musicSlider.GetComponent<Slider>().value); // setting the music volume by the value of the slider
        PlayerPrefs.SetFloat("musicVolome", musicSlider.GetComponent<Slider>().value); // saving the music volume by the value of the slider to player prefs
    }

    public void SetEffectVolumeFromSlider()
    {
        audioMixer.SetFloat("effectVol", effectsSlider.GetComponent<Slider>().value); // setting the effects volume by the value of the slider
        PlayerPrefs.SetFloat("effectVolome", effectsSlider.GetComponent<Slider>().value); // saving the effects volume by the value of the slider to player prefs
    }
    float GetMusicValueFromMixer()
    {
        float musicMixerValue;
        bool result = audioMixer.GetFloat("musicVol", out musicMixerValue); // checking if there is a key called musicVol in the audio mixer
        if (result)
        {
            return musicMixerValue;
        }
        else
        {
            return 0;
        }
    }
    float GetEffectsValueFromMixer()
    {
        float effectsMixerValue;
        bool result = audioMixer.GetFloat("effectVol", out effectsMixerValue);
        if (result)
        {
            return effectsMixerValue;
        }
        else
        {
            return 0;
        }
    }
    #endregion
}
