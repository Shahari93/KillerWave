using UnityEngine.SceneManagement;
using UnityEngine;

// Load the title screen after 3 seconds
public class LoadSceneComponent : MonoBehaviour
{
    float timer = 0f;
    public string loadThisScene;

    private void Start()
    {
        GameManager.Instance.GetComponentInChildren<ScoreManager>().ResetScore();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 3)
        {
            SceneManager.LoadScene(loadThisScene);
        }
    }
}
