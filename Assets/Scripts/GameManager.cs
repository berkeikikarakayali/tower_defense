using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    
    public static bool IsGameOver;


    [Header("References")]
    public GameObject winPanel;
    public GameObject losePanel;

    void Start()
    {
        IsGameOver = false;
        Time.timeScale = 1f; //time flows

        if (AudioManager.audioManager != null)
        {
        AudioManager.audioManager.PlayMusic("LevelTheme"); 
        }
    }

    void Update()
    {
        if (IsGameOver) return;

        if (BaseStats.Health <= 0) EndGame();
    }

    public void EndGame()
    {
        IsGameOver = true;
        Debug.Log("Game Over!");

        if(losePanel != null) {
            losePanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else {
            Debug.LogError("No LosePanel reference.");
        }
    }

    public void WinLevel()
    {
        IsGameOver = true;
        Debug.Log("Level Won!");
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.SetInt("LevelSaved", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.Save();

        if (winPanel != null)
        {
            winPanel.SetActive(true);
            Time.timeScale = 0f;
        } else
        {
            Debug.LogError("No WinPanel reference.");
        }
    }
}
