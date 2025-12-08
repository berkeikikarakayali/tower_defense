using UnityEngine;
using UnityEngine.EventSystems;
public class Tower : MonoBehaviour
{
	//Features that most of the towers have, to improve 
    [Header("Basic Tower Stats")] 
    public int cost = 15; //cost of the tower, can be updated individually later.
    public float range = 15f; //range of the tower, can be updated individually later.
    public LayerMask enemyLayer; //to know what an "Enemy" is for all towers
    protected Transform target; //protected, to acces from child. 

    [Header("Basic Tower Visuals")]
    public Transform rangeSphere; //sphere object that act like a range of the tower

    protected void FindTarget()
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
    public void UpdateRangeSphere() 
    {
		//if there is any sphere connected adjust it to look like the range of the tower, can 
        if (rangeSphere != null)
        {
            float diameter = range * 2f / 3f;
            rangeSphere.localScale = new Vector3(diameter, diameter, diameter);
        }
    }
    public virtual void OnMouseEnter() //when we hover over a turret, we activate the sphere to see the range 
    {
  
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (rangeSphere != null) rangeSphere.gameObject.SetActive(true);
    }

    public virtual void OnMouseExit()
    {
        if (rangeSphere != null) rangeSphere.gameObject.SetActive(false);
    }

    public virtual void OnValidate()
    {
        UpdateRangeSphere();
    }

}