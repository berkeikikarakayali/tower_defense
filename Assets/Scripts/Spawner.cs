using UnityEngine;
using System.Collections;
using TMPro; // For the Text Mesh Pro UI element

public class WaveSpawner : MonoBehaviour {
    
    [Header("References")]
    public Transform enemyPrefab; // The enemy I want to spawn
    public Transform spawnPoint; // Where the enemy should spawn from
    public TextMeshProUGUI  countdownText; // The text on the screen for the timer
    
    [Header("Wave Timings")]
    public float waveWaitTime = 5f; // How long to wait between waves
    public int maxWaves = 10; // The total number of waves for this level
    
    private float timer = 2f; // Timer for the next wave
    private int currentWaveIndex = 0;

    void Start()
    {
        // To make sure everything is assigned
        if (spawnPoint == null)
        {
            Debug.LogError("No spawn point referenced!");
            enabled = false;
        }
        if (enemyPrefab == null)
        {
            Debug.LogError("No enemy prefab referenced!");
            enabled = false;
        }
        if (countdownText == null)
        {
            Debug.LogError("No countdown text referenced!");
        }
    }
    void Update ()
    {
        if (timer <= 0f)
        {
            // Check if we've finished all waves
            if (currentWaveIndex >= maxWaves)
            {
                countdownText.text = "Level Completed!";
                enabled = false; // Stop the spawner
                return;
            }
            
            StartCoroutine(SpawnWave());
            timer = waveWaitTime;
            return;
        }

        timer -= Time.deltaTime;
        countdownText.text = Mathf.Round(timer).ToString();
    }

    IEnumerator SpawnWave ()
    {
        currentWaveIndex++;
        Debug.Log("Spawning Wave " + currentWaveIndex);
        
        // This loop spawns a number of enemies equal to the wave number
        for (int i = 0; i < currentWaveIndex; i++)
        {
            SpawnEnemy(); //To create an enemy
            yield return new WaitForSeconds(0.5f); //could be variable
        }
    }

    void SpawnEnemy () //Creates a copy of the prefab
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        // Debug.Log("Spawned an enemy!");
    }

}