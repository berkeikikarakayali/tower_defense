using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 5f;
    
    private Transform currentWaypoint;
    private int currentWaypointIndex = 0;

    void Start()
    {
        if (Path.Waypoints != null && Path.Waypoints.Length > 0)
        {
            currentWaypoint = Path.Waypoints[currentWaypointIndex];
        }
        else
        {
             Debug.LogError("No waypoints found");
             return;
        }
    }
    void Update()
    {
        if (currentWaypoint == null)
        {
            return;
        }
        
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);
        
        if (Vector3.Distance(transform.position, currentWaypoint.position) < 0.1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= Path.Waypoints.Length)
            {
                ReachedEndOfPath();
            }
            else
            {
                currentWaypoint = Path.Waypoints[currentWaypointIndex];
            }
        }
    }
    
    void ReachedEndOfPath() // Later we will decrease health etc. here
    {
        Debug.Log("Enemy reached the end.");
        Destroy(gameObject);
    }
    
}
