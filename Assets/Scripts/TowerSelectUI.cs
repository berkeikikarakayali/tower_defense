using UnityEngine;

public class TowerSelectUI : MonoBehaviour //attach it to panel that hold turret buttons
{
    public GameObject UIPanel; // holds the UI Panel
    private Node targetNode; // remembers the Node we clicked on

    public float yOffset = 50f;
    public float zOffset = -15f;
    
    public void SetTarget(Node newTarget)
    {
        targetNode = newTarget;
        
        // Move the menu to the correct position on screen
        Vector3 screenPos = Camera.main.WorldToScreenPoint(targetNode.transform.position);
        screenPos.y += yOffset;
        screenPos.x += zOffset;
        transform.position = screenPos;
        
        UIPanel.SetActive(true); // Show the menu
        
    }

    public void Hide()
    {
        UIPanel.SetActive(false); // Close the menu
    }

    public void SelectTower(int towerIndex) 
    {
        // "towerIndex" will be the number that typed in the Inspector (starts from 0 like array)
        BuildManager.buildManager.BuildTowerOn(targetNode, towerIndex);
        Hide();
    }
}