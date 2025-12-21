using UnityEngine;

public class Credits : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (AudioManager.audioManager != null)
        {
            AudioManager.audioManager.PlayMusic("CreditsTheme"); 
        }
    }

}
