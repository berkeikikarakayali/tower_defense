using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void Retry()
    {
        //load the same scene again
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void NextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if ( nextSceneIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(nextSceneIndex);
        } else
        {
            Debug.Log("End of the game!");
            if (AudioManager.audioManager != null)
        {
        AudioManager.audioManager.PlayMusic("CreditsTheme"); 
        }
            SceneManager.LoadScene("CreditsScene");
        }
    }
}
