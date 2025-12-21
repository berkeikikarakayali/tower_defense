using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Buton kontrolü için şart

public class MainMenu : MonoBehaviour
{
    [Header("References")]
    public GameObject mainPanel;     
    public GameObject settingsPanel;
    public Button continueButton;

    void Start()
    {
        if(settingsPanel != null) settingsPanel.SetActive(false);
        if(mainPanel != null) mainPanel.SetActive(true);

        if (PlayerPrefs.HasKey("LevelSaved"))
        {
            continueButton.interactable = true; // Tıklanabilir
        }
        else
        {
            continueButton.interactable = false; // Sönük (Grayed out)
        }
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteKey("LevelSaved"); 
        SceneManager.LoadScene(1); 
    }
    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
    public void ContinueGame()
    {
        int levelToLoad = PlayerPrefs.GetInt("LevelSaved");
        SceneManager.LoadScene(levelToLoad);
    }
    public void OpenSettings()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
}