using UnityEngine;

public class CannonTower : Tower
{
    [Header("Cannon Specifics")]
    public GameObject cannonBallPrefab;
    public Transform middlePart; //The part that spins
    public Transform firePoint;
    public float turnSpeed = 10f;
    public float fireRate = 0.5f; // Seconds between shots

    private float fireCountdown = 0f;

    void Start()
    {
        InvokeRepeating("FindTarget", 0f, 0.5f);
        UpdateRangeSphere();
    }

    void Update()
    {
        if (target == null) return;
        LockOnTarget();

        // Shooting Logic
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        GameObject ball_ins = Instantiate(cannonBallPrefab, firePoint.position, firePoint.rotation);
        CannonBall ball = ball_ins.GetComponent<CannonBall>();

        if (ball != null)
        {
            // We pass the position, for Arc
            ball.Launch(target.position);
        }
    }

        void LockOnTarget()
    {
        //Get the direction to the target
        Vector3 directionToLook = target.position - middlePart.position;
        //Ignore height differences 
        directionToLook.y = 0;
        // Create the target rotation
        Quaternion lookRotation = Quaternion.LookRotation(directionToLook);
        // rotate towards that rotation
        middlePart.rotation = Quaternion.Lerp(middlePart.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }   
}