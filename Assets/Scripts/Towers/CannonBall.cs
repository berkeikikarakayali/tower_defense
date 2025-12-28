using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [Header("CannonBall Stats")]
    public float speed = 15f;
    public int damage = 30;
    public float explosionRadius = 2f; //Explosion area
    public float height = 5f; //maximum height

    [Header("VEffects")]
    public GameObject explosionFX;

    public LayerMask enemyLayer;

    private Vector3 startPoint;
    private Vector3 targetPoint;
    private float duration;
    private float startTime;
    private bool isLaunched = false;

    public void Launch(Vector3 _targetPoint)
    {
        startPoint = transform.position;
        targetPoint = _targetPoint;
        float currentSpeed = speed * WeatherManager.GlobalBulletSpeedMultiper;
        float distance = Vector3.Distance(startPoint, targetPoint);
        duration = distance / currentSpeed;
        startTime = Time.time;
        isLaunched = true;
    }

    void Update()
    {
        if (!isLaunched) return;
        float timePassed = Time.time - startTime;
        float percentage = timePassed / duration;
        if(percentage >= 1f)
        {
            Explode();
            return;
        }
        Vector3 currentPosition = Vector3.Lerp(startPoint, targetPoint, percentage);
        float yOffset = Mathf.Sin(percentage * Mathf.PI) * height;
        transform.position = currentPosition + Vector3.up * yOffset;
    }

    void Explode()
    {
        Collider[] enemiesHit = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer);
        foreach (Collider coll in enemiesHit)
        {
            Enemy e = coll.GetComponent<Enemy>();
            if (e != null)
            {
                e.TakeDamage(damage);
            }
        }

        if (explosionFX != null)
        {
            GameObject effectIns = Instantiate(explosionFX, transform.position, transform.rotation);
            Destroy(effectIns, 2f);
        }

        Destroy(gameObject);
    }
}