using UnityEngine;

public class Turret : MonoBehaviour
{

    public float range = 15f;
    public float turnSpeed = 5f;
    public LayerMask enemyLayer;
    
    public Transform TurretMiddlePart;
    public Transform target;
    public Transform rangeSphere;
    void Start()
    {
        InvokeRepeating("FindTarget", 0f, 0.4f);
        UpdateRangeSphere();
    }
    
    void OnValidate()
    {
        UpdateRangeSphere();
    }

    void UpdateRangeSphere()
    {
        // Check if we've assigned the range sphere
        if (rangeSphere != null)
        {
            // To make its radius equal to our 'range' multiply it with 2/3
            float diameter = range*2/3;
            
            rangeSphere.localScale = new Vector3(diameter, diameter, diameter);
        }
    }
    
    void FindTarget()
    {
        //Invisible sphere at our position with our range
        //Get an array of all colliders on the enemyLayer inside it.
        Collider[] enemies = Physics.OverlapSphere(transform.position, range, enemyLayer);
        
        float shortestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        //Loop through every enemy collider we found.
        //We should find the closest one in the array.
        foreach (Collider enemy in enemies)
        {
            //Calculate the distance to this enemy.
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            //Check if this enemy is closer than the last one.
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy.transform; // We target the collider's transform
            }
        }

        //We set our target
        //because OverlapSphere already guaranteed they are in range.
        if (nearestEnemy != null)
        {
            target = nearestEnemy;
        }
        else
        {
            target = null;
        }
    }
    
    void Update()
    {
        if (target == null)
        {
            return;
        }
        //Get the direction to the target
        Vector3 directionToLook = target.position - transform.position;
        //ignore height differences
        directionToLook.y = 0;
        // Create the target rotation
        Quaternion lookRotation = Quaternion.LookRotation(directionToLook);
        // rotate towards that rotation
        TurretMiddlePart.rotation = Quaternion.Lerp(TurretMiddlePart.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }
}

