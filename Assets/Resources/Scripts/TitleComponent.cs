using UnityEngine.SceneManagement;
using UnityEngine;

// Load the shop scene, and set the player lives to 3
public class TitleComponent : MonoBehaviour
{
    // No need for this because now we set the amount of lives to X depents if the player is conected to the internet
    void Start()
    {
        if (GameManager.playerLives <= 2)
        {
            GameManager.playerLives = 3;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            SceneManager.LoadScene("shop");
        }
    }
}
