using UnityEngine;

[System.Serializable]
public class WaveGroup //this class hold a group of enemies (like 5 basic enemies at rate 2)
{
    public GameObject enemyPrefab; //what to spawn
    public int count; // how many to spawn
    public float spawnRate; //how fast they spawn

}   

[System.Serializable]
public class Wave //single wave, which can contain multiple groups of enemies
{
    public string name = "Wave X";
    public WaveGroup[] groups;

}


public class WaveSetup : MonoBehaviour //used MonoBehaviour to save this as a prefab, and use in different levels
{
    public Wave[] waves;
}
