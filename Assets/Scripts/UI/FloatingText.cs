using UnityEngine;
using TMPro;
public class FloatingText : MonoBehaviour
{
    private TextMeshPro textMesh;
    public float duration = 1.5f; //how many second the text will stay
    public Vector3 offset = new Vector3(0,2,0); //offset, on top of enemy
    void Start()
    {
        Destroy(gameObject, duration); //destroy after "duration"
        transform.localPosition += offset; 
    }

    void Update()
    {
        transform.position += new Vector3(0, 2f * Time.deltaTime, 0); //move upword until it gets destroyed
        if (Camera.main != null) //face camera
        {
            transform.rotation = Camera.main.transform.rotation;
        }
    }

    public void SetText(string text, Color? color = null)
    {
        textMesh = GetComponent<TextMeshPro>();
        textMesh.text = text;

        if(color != null)
        {
            textMesh.color = (Color)color;
        }
    }
}
