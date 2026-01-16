using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro; // For the Text Mesh Pro UI element
using UnityEngine.SceneManagement;
using UnityEngine.UI; //wave start button

public class Spawner : MonoBehaviour {
    
    public enum SpawnState { SPAWNING, WAITING, COMPLETE }; // game flow

    [Header("Wave Setup")]
    public WaveSetup waveSetup; //the wavesetup object reference that we created
    //public float waveWaitTime = 3f; // How long to wait between waves

    [Header("References")]
    public Transform spawnPoint; // Where the enemy should spawn
    public TextMeshProUGUI statusText; // The text on the screen for the countdown and messages
    public Button startButton; //wave start button;
    public bool isWaveActive = false; //to check for the wave is active

    [Header("AutoStartWave Setup")]
    public bool isAutoStartEnabled = false; //if active, automatically countdown starts
    public float autoStartWaitTime = 3f;
    private Coroutine autoStartCoroutine;


    [Header("Dynamic Weather Settings")]
    public WeatherType defaultWeather; //clear weather
    public WeatherType earthquakeWeather; //only for the 10th wave earthquake
    public List<WeatherType> otherWeathers; //fog,rain,wind random for 3 6 9 th waves

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
        if (startButton == null)
        {
            Debug.LogError("No start button referenced!");
            enabled = false;
            return;
        }

        startButton.onClick.AddListener(OnStartWaveButtonClicked); //we tell the game what to do
        ShowStartButton(); //for the first wave
    }
    void Update ()
    {   
        //if the spawning process is running, or the level is completed, do nothing
        if (state == SpawnState.SPAWNING || state == SpawnState.COMPLETE)
        {
            return;
        }

        if (isWaveActive && state == SpawnState.WAITING) //if we are waiting, check if the enemies are dead?
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

    public void SetAutoStart(bool isEnabled)
    {
        isAutoStartEnabled = isEnabled;

        // if "Auto Start" enabled while we are waiting
        if (isAutoStartEnabled && state == SpawnState.WAITING && !isWaveActive)
        {
            // if open, close and start countdown
            if (startButton.gameObject.activeSelf)
            {
                startButton.gameObject.SetActive(false);
                if (autoStartCoroutine != null) StopCoroutine(autoStartCoroutine);
                autoStartCoroutine = StartCoroutine(AutoStartCountdown());
            }
        }
        //if countdown was active, and player close it
        else if (!isAutoStartEnabled && state == SpawnState.WAITING && !isWaveActive)
        {
            if (autoStartCoroutine != null) StopCoroutine(autoStartCoroutine);
            ShowStartButton(); //default
        }
    }

    void OnStartWaveButtonClicked()
    {
        startButton.gameObject.SetActive(false); //disable the button
        StartCoroutine(StartNextWaveRoutine()); //start wave
    }

    void ShowStartButton()
    {
        state = SpawnState.WAITING;
        isWaveActive = false; 

        if (isAutoStartEnabled)
        {
            startButton.gameObject.SetActive(false);
            if (autoStartCoroutine != null) StopCoroutine(autoStartCoroutine);
            autoStartCoroutine = StartCoroutine(AutoStartCountdown());
        } else {
            startButton.gameObject.SetActive(true); // show button
            statusText.text = "Wave " + (nextWaveIndex + 1) + " Ready";
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
            //StartCoroutine(CountdownForNextWave()); // yes, start the countdown for the next wave
            ShowStartButton();
         } else
        {
            LevelCompleted(); // no, all waves are finished
        }
    }

    IEnumerator AutoStartCountdown() // to handle countdown between waves
    {
        float countdown = autoStartWaitTime;
        while (countdown > 0)
        {
            countdown -= Time.deltaTime;
            //clamp to not show negative number, F2 to show like 2.XX
            statusText.text = "Next Wave: " + Mathf.Clamp(countdown, 0f, Mathf.Infinity).ToString("F2");
            yield return null; //wait for the next frame
        }
        OnStartWaveButtonClicked();
    }

    IEnumerator StartNextWaveRoutine() 
    {
        state = SpawnState.SPAWNING; 
        isWaveActive = true; 

        ChooseWeather(); //dynamic weather
        yield return StartCoroutine(SpawnWave(waveSetup.waves[nextWaveIndex]));
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
    void ChooseWeather()
    {
        int currentWaveNumber = nextWaveIndex + 1; // to determine the which number of waves we are in currently
        WeatherType selectedWt = defaultWeather; //default;
        
        if (currentWaveNumber == 10)
        {
            selectedWt = earthquakeWeather;
        }

        if(currentWaveNumber % 3 == 0)
        {
            if ( otherWeathers != null && otherWeathers.Count > 0)
            {
                int randomInd = Random.Range(0, otherWeathers.Count);
                Debug.Log(randomInd);
                selectedWt = otherWeathers[randomInd];
            }
        }

        if (WeatherManager.weatherManager != null)
        {
            WeatherManager.weatherManager.SetWeather(selectedWt);
        }

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
        startButton.gameObject.SetActive(false); //close button
        if (autoStartCoroutine != null) StopCoroutine(autoStartCoroutine); // Stop any timer
        Debug.Log("All waves are finished! Good job!");
        state = SpawnState.COMPLETE;
        statusText.text = "YOU WIN!";

        GameManager gm = FindFirstObjectByType<GameManager>();
        if(gm != null)
        {
            gm.WinLevel();
        }
    }
}