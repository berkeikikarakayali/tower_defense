using UnityEngine;

[CreateAssetMenu(fileName = "NewWeather", menuName = "TowerDefense/Weather Type")]
public class WeatherType : ScriptableObject
{
    public string weatherName;
    
    [TextArea]
    public string description;

    [Header("Multipliers")]
    public float rangeMultiplier = 1f;
    public float enemySpeedMultiplier = 1f;
    public float bulletSpeedMultiplier = 1f;

    [Header("Visual")]
    public GameObject particleEffect; //reference to particle effect prefab

    [Header("Earthquake Setup")]
    public bool isEarthquake = false;     
    public float shakeInterval = 10f; // How often
    public float firstCrashChance = 0.1f; // Chance to destroy before first tower destruction (10%)
    public float nextCrashChance = 0.05f; // Change to destroy next tower(s)
    public float shakeDuration = 2.0f; //how long it will last
    public int maxDestructionLimit = 2; // Maximum number of towers can be destructed
    public GameObject destructionFX; // Effect spawned when a tower is destroyed
}
