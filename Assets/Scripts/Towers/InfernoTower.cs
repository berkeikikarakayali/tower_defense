using UnityEngine;
using System.Collections;
public class InfernoTower : Tower
{
    [Header("Laser Stats")] //Edit all via Inspector
    public float damage = 30f; //How much damage per second
    public float turnSpeed = 10f;
    public float dmgIncRate = 1.1f; //How fast the laser damage increase
    public float maxRate = 10f; //Max 3x
    public float tickRate = 0.1f;
    private float damageToDeal;

    [Header("References")]
    public Transform middlePart; //The part that spins
    public Transform firePoint; //Tip of the gun
    public LineRenderer laserLine; //The laser beam visual
    public float currentRate = 1f;
    private Coroutine laserCoroutine; //To start and stop Coroutine

    void Update()
    {
        if (target == null)
        {
            StopLaser();
            return;
        }
        LockOnTarget();
        EnableLaser();
        if (laserCoroutine == null)
        {
            laserCoroutine = StartCoroutine(LaserSystem());
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

    void EnableLaser()
    {
        if(!laserLine.enabled) laserLine.enabled = true;
        laserLine.SetPosition(0,firePoint.position); //Start point of the laser
        laserLine.SetPosition(1,target.position);    //End point
    }

    void StopLaser()
    {
        if (laserLine.enabled) laserLine.enabled = false; //Disable laser line

        if (laserCoroutine != null) //Stop coroutine
        {
            StopCoroutine(laserCoroutine);
            laserCoroutine = null;
        }

        //Reset rate
        currentRate = 1f;
    }

    IEnumerator LaserSystem()
    {
        while(target != null)
        {
            if(currentRate < maxRate)
            {
                currentRate *= dmgIncRate; 
            }
        if (currentRate > maxRate) currentRate = maxRate;
        float damagePerSecond = damage * currentRate;
        float tickDamage = damagePerSecond * tickRate;
        
        Enemy e = target.GetComponent<Enemy>();
            if (e != null)
            {   
                e.TakeDamage(tickDamage);
            }
            yield return new WaitForSeconds(tickRate);
        }
        laserCoroutine = null;
    }
}