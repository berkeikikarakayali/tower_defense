using UnityEngine;

public class Turret : MonoBehaviour
{
    
    public float range = 15f;
    public float turnSpeed = 5f;
    public LayerMask enemyLayer;

    public float fireRate = 1f;
    private float fireCountdown = 0f; 
    public GameObject bulletPrefab; // The bullet prefab to spawn
    public Transform firePoint; // The empty GameObject at the tip
    
    
    public Transform turretMiddlePart;
    public Transform rangeSphere;
    private Transform target;
    
    void Start()
    {
        InvokeRepeating("FindTarget", 0f, 0.4f);
        UpdateRangeSphere();
    }   
    
    void OnValidate()
    {
        // This function runs whenever a value changed in the Inspector(range)
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

        LockOnTarget();
        
        fireCountdown -= Time.deltaTime; // Decrease timer
        
        // If countdown reaches 0, we shoot the bullet
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate; // Reset timer based on fire rate
        }
        void Shoot()
        {
            // Create the bullet object
            GameObject bullet_ins = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            
            // Get the Bullet script
            Bullet bullet = bullet_ins.GetComponent<Bullet>();

            if (bullet != null) //If exists, tell it who the target is
            {
                bullet.Follow(target);
            }
        }

        void LockOnTarget()
        {
            //Get the direction to the target
            Vector3 directionToLook = target.position - turretMiddlePart.position;
            //Ignore height differences 
            //directionToLook.y = 0;
            // Create the target rotation
            Quaternion lookRotation = Quaternion.LookRotation(directionToLook);
            // rotate towards that rotation
            turretMiddlePart.rotation = Quaternion.Lerp(turretMiddlePart.rotation, lookRotation, Time.deltaTime * turnSpeed);
        }
    }
}

