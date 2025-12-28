using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class WeatherManager : MonoBehaviour
{
    public static WeatherManager weatherManager; //singleton
    public List<Tower> activeTowers = new List<Tower>(); // Keeps track of all active towers

    // Global multipliers
    public static float GlobalRangeMultiplier = 1f;
    public static float GlobalEnemySpeedMultiplier = 1f;
    public static float GlobalBulletSpeedMultiper = 1f;

    [Header("UI References")]
    public TextMeshProUGUI infoText;

    public WeatherType startWeather; // Default weather
    public Transform effects; //effect holder empty object at 0,0,0
    private GameObject currentEffect;

    private Coroutine earthquakeCoroutine; // Reference to control the earthquake loop
    
    public WeatherType earthquakeWeather;
    public WeatherType windyWeather;
    
    void Awake()
    {
        if(weatherManager != null)
        {
            Destroy(gameObject);
            return;
        }
        weatherManager = this;
    }

    void Start()
    {
        if (startWeather != null)
        {
            SetWeather(startWeather);
        }
    }

    public void SetWeather(WeatherType weather)
    {
        Debug.Log("Weather change; "+ weather.weatherName);

        //Update Global Variables
        GlobalRangeMultiplier = weather.rangeMultiplier;
        GlobalEnemySpeedMultiplier = weather.enemySpeedMultiplier;
        GlobalBulletSpeedMultiper = weather.bulletSpeedMultiplier;

        //Handle visual effects
        if (currentEffect != null)
        {
            Destroy(currentEffect, 0.1f);
        }

        if( weather.particleEffect != null && effects != null)
        {
            currentEffect = Instantiate(weather.particleEffect, effects);
        }

        //Update Tower Ranges
        for (int i = 0; i < activeTowers.Count; i++)
        {
            if (activeTowers[i] != null)
            {
                activeTowers[i].UpdateRangeSphere(); //we update each tower that is active, 
            }
        }

        //Earthquake section
        //Stop if the previous earthquake routine is running
        if(earthquakeCoroutine != null)
        {
            StopCoroutine(earthquakeCoroutine);
            earthquakeCoroutine = null;
        }

        //Start new routine
        if (weather.isEarthquake)
        {
            earthquakeCoroutine = StartCoroutine(EarthquakeRoutine(weather));
        }

        if (infoText != null)
        {
            infoText.text = $"{weather.weatherName}\n<size=80%>{weather.description}</size>";
            infoText.color = weather.isEarthquake ? Color.red : Color.white;
        }

        StopCoroutine("HideWeatherInfo");
        StartCoroutine("HideWeatherInfo");
    }

    IEnumerator EarthquakeRoutine (WeatherType weatherData)
    {
        int destroyedCount = 0; //counter for destroyed towers

        float waitTime = weatherData.shakeInterval;
        if (waitTime <= 0.1f) waitTime = 1f;

        while(true)
        {
            //shake 
            if(CameraShake.cameraShake != null)
            {
                StartCoroutine(CameraShake.cameraShake.Shake(weatherData.shakeDuration));
            }
            if(destroyedCount < weatherData.maxDestructionLimit && activeTowers.Count > 0)
            {   
            float currentChance = (destroyedCount == 0) ? weatherData.firstCrashChance : weatherData.nextCrashChance;
                
                // Random.value returns a float between 0 and 1
                if (Random.value <= currentChance*3)
                {
                    DestroyRandomTower(weatherData.destructionFX);
                    destroyedCount++; 
                }
            }
            yield return new WaitForSeconds(waitTime);
        }
    }

    //function to destroy a random tower
    void DestroyRandomTower(GameObject fx)
    {
        int randomIndex = Random.Range(0, activeTowers.Count);
        Tower victim = activeTowers[randomIndex]; // Selected victim

        if (victim != null)
        {
            Debug.Log("Earthquake: " + victim.name + " destroyed!");
            
            // Spawn destruction fx if available
            if (fx != null) 
                Instantiate(fx, victim.transform.position, Quaternion.identity);
            
            // Destroy the object
            Destroy(victim.gameObject);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetWeather(windyWeather);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetWeather(earthquakeWeather);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            SetWeather(startWeather);
        }
    }

    IEnumerator HideWeatherInfo()
    {
        yield return new WaitForSeconds(5f);
        infoText.text = ""; 
    }
}
