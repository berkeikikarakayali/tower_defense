using UnityEngine;
using System.Collections.Generic;

public class WeatherManager : MonoBehaviour
{
    public static WeatherManager weatherManager; //singleton
    public List<Tower> activeTowers = new List<Tower>();

    public static float GlobalRangeMultiplier = 1f;
    public static float GlobalEnemySpeedMultiplier = 1f;
    public static float GlobalBulletSpeedMultiper = 1f;

    public WeatherType startWeather;
    public Transform effects;
    
    private GameObject currentEffect;
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

        GlobalRangeMultiplier = weather.rangeMultiplier;
        GlobalEnemySpeedMultiplier = weather.enemySpeedMultiplier;
        GlobalEnemySpeedMultiplier = weather.bulletSpeedMultiplier;

        if (currentEffect != null)
        {
            Destroy(currentEffect);
        }

        if( weather.particleEffect != null && effects != null)
        {
            currentEffect = Instantiate(weather.particleEffect, effects);
        }
        for (int i = 0; i < activeTowers.Count; i++)
        {
            if (activeTowers[i] != null)
            {
                activeTowers[i].UpdateRangeSphere(); //we update each tower that is active, 
            }
        }
    }
}
