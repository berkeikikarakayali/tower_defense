using UnityEngine;

[CreateAssetMenu(fileName = "NewWeather", menuName = "TowerDefense/Weather Type")]
public class WeatherType : ScriptableObject
{
    public string weatherName;
    public float rangeMultiplier = 1f;
    public float enemySpeedMultiplier = 1f;
    public float bulletSpeedMultiplier = 1f;
    public GameObject particleEffect; //reference to particle effect prefab
}
