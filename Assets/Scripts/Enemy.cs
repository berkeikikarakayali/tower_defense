using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float moveSpeed = 5f; // How fast the enemy moves
    public float health = 100; // How much health the enemy starts
    public int damageToBase = 1; // Shows when the enemy reaches the base how much health it will decrease
	public int deathValue = 10; //the amount that we will gain when we kill an Enemy
    [Header("Effects")]
    public GameObject deathEffect; //reference to particle system prefab
    public GameObject deathText; //reference to EnemyDeathText prefab /TextMeshPro
    private float startHealth;
    private Transform targetWaypoint;
    private int currentWaypointIndex = 0;
    
    void Awake()
    {
        // Capture max health at start
        startHealth = health;
        // Initialize the bar
        if ( GetComponent<EnemyHealthBar>() != null)
        {
            GetComponent<EnemyHealthBar>().UpdateHealth(health, startHealth);
        }
        
        // When the enemy spawns needs to find the first waypoint
        // We need to make sure the Path script and its Waypoints array are working
        if (Path.Waypoints != null && Path.Waypoints.Length > 0)
        {
            targetWaypoint = Path.Waypoints[currentWaypointIndex];
        }
        else
        {
             Debug.LogError("No waypoints found");
             return;
        }
    }
    
    void Update()
    {
        if (targetWaypoint == null) return;
        
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);
        transform.LookAt(targetWaypoint); 
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            GetNextWaypoint();
        }
    }
    
    //Damage handling
    public void TakeDamage(float damage)
    {
        // Subtract the damage amount from current health
        health -= damage;
        // Update UI
        if (GetComponent<EnemyHealthBar>() != null)
        {
            GetComponent<EnemyHealthBar>().UpdateHealth(health, startHealth);
        }
        
        // If health drops to 0 or less, the enemy dies
        if (health <= 0)
        {
            Death();
        }
    }
    
    void Death()
    {
        if (deathEffect != null)
        {
            GameObject effectIns = Instantiate(deathEffect, transform.position, Quaternion.identity);

            //Destroy the particle object after 2 seconds
            Destroy(effectIns, 2f);
        }
        
        if (deathText != null)
        {
            GameObject text_ins = Instantiate(deathText, transform.position, Quaternion.identity);
            EnemyDeathText text_script = text_ins.GetComponent<EnemyDeathText>();
            if(text_script != null)
            {
                text_script.SetText("+$"+ deathValue);
            }
        }

      	BaseStats.addMoney(deathValue);
        Destroy(gameObject);
    }
    
    //To handle getting the next waypoint
    void GetNextWaypoint()
    {
        currentWaypointIndex++;
        if (currentWaypointIndex >= Path.Waypoints.Length)
        {
            ReachedEndOfPath();
            return;
        }
        else
        {
            targetWaypoint = Path.Waypoints[currentWaypointIndex];
        }
    }
    
    void ReachedEndOfPath() // Later we will decrease health etc. here
    {
        //Debug.Log("Enemy reached the end.");
        BaseStats.decreaseHealth(damageToBase);
        Destroy(gameObject);
    }

    public void ChangeDifficulty(int waveNumber)
    {
        // Calculate the multiplier 
        float multiplier = Mathf.Pow(1.1f, waveNumber);
        health = startHealth * multiplier; //set the values;
    }
}
