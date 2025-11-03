using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float moveSpeed = 5f; // How fast the enemy moves
    public int health = 100; // How much health the enemy starts
    public int damageToBase = 1; // Shows when the enemy reaches the base how much health it will decrease
    
    private Transform targetWaypoint;
    private int currentWaypointIndex = 0;
    
    void Start()
    {
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
        if (targetWaypoint == null)
        {
            return;
        }
        
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);
        
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            GetNextWaypoint();
        }
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
        BaseStats.Health -= damageToBase;  
        Destroy(gameObject);
    }
}
