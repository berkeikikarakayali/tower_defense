using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public Color hoverColor = Color.green; // The color of the node when we hover over it/also can be changed from inspector window
    private Color startColor;  // To remember the original color of the node
    
    private Renderer rend;

    public Tower tower; // This variable will hold the tower on this node (if there is one)

    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }

    void OnMouseEnter()
    {
        // Check if the mouse is hovering over a UI element
        // If so, exit the function to prevent the node from highlighting underneath the UI
        if (EventSystem.current.IsPointerOverGameObject()) return;
        
        rend.material.color = hoverColor;
    }

    void OnMouseExit()
    {
        rend.material.color = startColor;
    }

    void OnMouseDown()
    {
        // Prevent interaction with the node if the mouse is interacting with UI
        if (EventSystem.current.IsPointerOverGameObject()) return;

        // If a tower is already placed here prevent building a new one
        // to do : upgrading or selling the tower
        if (tower != null) 
        {
            Debug.Log("Tower already placed on this node.");
            return;
        }

        // Tell BuildManager to select this node for construction.
        BuildManager.buildManager.SelectNode(this);
    }
}