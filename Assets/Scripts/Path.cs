using UnityEngine;

public class Path : MonoBehaviour
{
    public static Transform[] Waypoints;

    void Awake() // We used awake if we didn't use it when the enemy script called for the start function it would cause null reference
    {
        
        // Get all the waypoint children and put them in the array
        Waypoints = new Transform[transform.childCount];
        for (int i = 0; i < Waypoints.Length; i++)
        {
            Waypoints[i] = transform.GetChild(i);
        }
    }
}