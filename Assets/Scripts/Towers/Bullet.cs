using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    
    public float speed = 100f;
    public int damage = 50;
    
    // Will called by the Turret to tell the bullet what to follow
    public void Follow(Transform newTarget)
    {
        target = newTarget;
    }
    
    // Update is called once per frame
    void Update()
    {
        // If the enemy was destroyed by another turret before this bullet's hit
        if (target == null) 
        {
            Destroy(gameObject);
            return; 
        }
        float currentSpeed = speed * WeatherManager.GlobalBulletSpeedMultiper;


        //Calculate direction and distance for this frame
        Vector3 direction = target.position - transform.position;
        float distanceToMoveThisFrame = currentSpeed * Time.deltaTime; //To move at the same speed every computer

        // If the distance to target is less than the distance we move this frame, we hit
        if (direction.magnitude <= distanceToMoveThisFrame)
        {
            HitTarget();
            return;
        }
        
        // We normalize the direction (make length 1) to control only direction, not speed
        transform.Translate(direction.normalized * distanceToMoveThisFrame, Space.World);
        //Make bullet look at target
        transform.LookAt(target);
    }
    
    void HitTarget()
    {   
        // Try to find the "Enemy" script on the object to use TakeDamage function
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        Destroy(gameObject); // Destroy the bullet
    }
}
