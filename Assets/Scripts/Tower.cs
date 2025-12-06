using UnityEngine;
using UnityEngine.EventSystems;
public class Tower : MonoBehaviour
{
	//Features that most of the towers have, to improve 
    [Header("Basic Tower Stats")] 
    public int cost = 15; //cost of the tower, can be updated individually later.
    public float range = 15f; //range of the tower, can be updated individually later.

    [Header("Basic Tower Visuals")]
    public Transform rangeSphere; //sphere object that act like a range of the tower

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