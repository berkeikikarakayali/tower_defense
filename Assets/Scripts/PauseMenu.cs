using UnityEngine;
using UnityEngine.SceneManagement; 

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    [Header("UI References")]

    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(settingsMenuUI.activeSelf)
            {

                CloseSettings();

            } else if (GameIsPaused)
            {
            
            Resume();

            } else
            {

            Pause();

            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);    
        settingsMenuUI.SetActive(false); 
        Time.timeScale = 1f; //restore time      
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);     
        Time.timeScale = 0f;             
        GameIsPaused = true;
    }


    //settings menu
    public void OpenSettings()
    {
        pauseMenuUI.SetActive(false);   
        settingsMenuUI.SetActive(true); 
    }

    // back to the pause menu
    public void CloseSettings()
    {
        settingsMenuUI.SetActive(false); 
        pauseMenuUI.SetActive(true);     
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; //restore time
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void LoadMainMenu()
    {
        
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene("MainMenu");
    }
}
