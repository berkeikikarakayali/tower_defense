using UnityEngine;

public class BuildManager : MonoBehaviour 
{
    public static BuildManager buildManager;
    public Vector3 offset = new Vector3(-3f, 0.876517f, -3f);
    
    
    // An Array to hold Tower Prefabs
    public GameObject[] towerPrefabs; 
    // Reference to tower selection UI
    public TowerSelectUI towerSelectUI;

	void Awake()
    {
        if (buildManager != null)
        {
            Debug.LogError("There are multiple build managers in the scene");
            return;
        }
        buildManager = this;
    }

    public void SelectNode(Node node)
    {
        if (towerSelectUI == null)
        {
            Debug.LogError("UI reference is missing");
            return;
        }
        towerSelectUI.SetTarget(node);
    }
    
    public void BuildTowerOn(Node node, int towerID)
    {
        int index = towerID;
        if (index < 0 || index >= towerPrefabs.Length)
        {
            Debug.LogError("Tower ID is invalid");
            return;
        }

        // Get the tower from the array
        GameObject selectedTower = towerPrefabs[index];
        // Build the tower
        // We added an offset to create a tower object right at the center of the surface of the node
        GameObject t = Instantiate(selectedTower, node.transform.position + offset, Quaternion.identity);
        node.tower = t;
    }
}