using UnityEngine;
using UnityEngine.EventSystems;
public class Tower : MonoBehaviour
{
    [Header("Basic Tower Stats")]
    public int cost = 100;
    public float range = 15f;

    [Header("PVisuals")]
    public Transform rangeSphere; // Her kulenin menzilini göstermeye ihtiyacı vardır

    // Bu fonksiyonu burada yazıyoruz ki her child (Turret, Laser vs) tekrar yazmak zorunda kalmasın
    public void UpdateRangeSphere()
    {
        if (rangeSphere != null)
        {
            float diameter = range * 2f / 3f;
            rangeSphere.localScale = new Vector3(diameter, diameter, diameter);
        }
    }
    
    // Unity Eventleri (Mouse üzerine gelince menzil gösterme)
    // Virtual yapıyoruz ki ileride özel bir kule bunu değiştirmek isterse değiştirebilsin.
    public virtual void OnMouseEnter()
    {
        // Prevent interaction with the node if the mouse is interacting with UI
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (rangeSphere != null) rangeSphere.gameObject.SetActive(true);
    }

    public virtual void OnMouseExit()
    {
        if (rangeSphere != null) rangeSphere.gameObject.SetActive(false);
    }
    
    // Inspector'da değer değişince menzili güncelle
    public virtual void OnValidate()
    {
        UpdateRangeSphere();
    }
}