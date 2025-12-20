using UnityEngine;
using System.Collections;
using TMPro; // For the Text Mesh Pro UI element
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour {
    
    public enum SpawnState { SPAWNING, WAITING, COMPLETE }; // game flow

    [Header("Wave Setup")]
    public WaveSetup waveSetup; //the wavesetup object reference that we created
    public float waveWaitTime = 3f; // How long to wait between waves

    [Header("References")]
    public Transform spawnPoint; // Where the enemy should spawn
    public TextMeshProUGUI  statusText; // The text on the screen for the countdown and messages
    public bool isWaveActive = false; //to check for the wave is active

    private int nextWaveIndex = 0;
    private float searchCountDown = 1f; // How often to check for enemies, to optimize the game higher
    private SpawnState state = SpawnState.WAITING;

    void Start()
    {
        // To make sure everything is assigned
        if (spawnPoint == null)
        {
            Debug.LogError("No spawn point referenced!");
            enabled = false;
            return;
        }
        if (waveSetup == null)
        {
            Debug.LogError("No wave setup referenced!");
            enabled = false;
            return;
        }
        if (statusText == null)
        {
            Debug.LogError("No status text referenced!");
            enabled = false;
            return;
        }
    }
    void Update ()
    {   
        //if the spawning process is running, or the level is completed, do nothing
        if (state == SpawnState.SPAWNING || state == SpawnState.COMPLETE)
        {
            return;
        }

        if (state == SpawnState.WAITING) //if we are waiting, check if the enemies are dead?
        {
            if (EnemiesCleared())
            {
                WaveFinished(); //no enemies left, the wave is finished
            } else
            {
                return; 
            }
        }
    }
    bool EnemiesCleared()
    {
        //Instead of checking 60 times a second, check once per second
        searchCountDown -= Time.deltaTime;
        if (searchCountDown <= 0f)
        {
            searchCountDown = 1f; //timer
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return true; // No enemies found
            }
        }
        return false; // Enemies found (or wait for the next check)
    }
    void WaveFinished() //called when all the enemies are dead
    {
        isWaveActive = false; //wave is over
        if (nextWaveIndex < waveSetup.waves.Length) //check if there are more wave left
        {
            StartCoroutine(CountdownForNextWave()); // yes, start the countdown for the next wave
         } else
        {
            LevelCompleted(); // no, all waves are finished
        }
    }

    IEnumerator CountdownForNextWave() // to handle countdown between waves
    {
        state = SpawnState.SPAWNING; // change the state to spawning, so update doesn't trigger this function again
        isWaveActive = false; //not active countdown

        float countdown = waveWaitTime;
        while (countdown > 0)
        {
            countdown -= Time.deltaTime;
            //clamp to not show negative number, F2 to show like 2.XX
            statusText.text = "Next Wave: " + Mathf.Clamp(countdown, 0f, Mathf.Infinity).ToString("F2");

            yield return null; //wait for the next frame
        }

        StartCoroutine(SpawnWave(waveSetup.waves[nextWaveIndex]));
        nextWaveIndex++;
    }
    IEnumerator SpawnWave (Wave currentWave)
    {
        Debug.Log("Spawning Wave:" + currentWave.name);
        state = SpawnState.SPAWNING;
        isWaveActive = true; //wave is active
        statusText.text = currentWave.name; //show wave name on screen
        
        foreach (WaveGroup group in currentWave.groups)
        {
            for (int i = 0; i < group.count; i++)
            {
                SpawnEnemy(group.enemyPrefab); //To create an enemy
                yield return new WaitForSeconds(group.spawnRate);
            }
        }
        state = SpawnState.WAITING; // spawning process is finished, wait for player to kill the enemies
    }
    
    void SpawnEnemy (GameObject _enemy) //Creates a copy of the prefab
    {
        GameObject newEnemy = Instantiate(_enemy, spawnPoint.position, spawnPoint.rotation);
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        if(enemyScript != null && nextWaveIndex > 0)
        {
            enemyScript.ChangeDifficulty(nextWaveIndex);
        }
    }

    void LevelCompleted()
    {
        isWaveActive = false; //level completed
        Debug.Log("All waves are finished! Good job!");
        state = SpawnState.COMPLETE;
        statusText.text = "YOU WIN!";

        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
    
        if (nextLevelIndex < SceneManager.sceneCountInBuildSettings)
        {   
            PlayerPrefs.SetInt("LevelSaved", nextLevelIndex);
            PlayerPrefs.Save();
        }
    }
}