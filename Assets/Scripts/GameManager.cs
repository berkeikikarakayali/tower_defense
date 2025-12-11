using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public bool IsGameOver;
    void Start()
    {
        IsGameOver = false;
    }

    public void EndGame()
    {
        IsGameOver = true;
        Debug.Log("Game Over!");
    }
}
