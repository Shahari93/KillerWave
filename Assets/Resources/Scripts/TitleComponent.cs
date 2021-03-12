using UnityEngine.SceneManagement;
using UnityEngine;

// Load the shop scene, and set the player lives to 3
public class TitleComponent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.playerLives = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            SceneManager.LoadScene("shop");
        }
    }
}
