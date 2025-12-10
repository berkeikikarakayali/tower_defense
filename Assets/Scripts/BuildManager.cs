using UnityEngine;

public class BuildManager : MonoBehaviour 
{
    public static BuildManager buildManager;
    public Vector3 offset = new Vector3(-3f, 0.876517f, -3f); //For placing the towers
    
    [Header("References")]
    // An Array to hold Tower Prefabs
    public Tower[] towerPrefabs; 
    // Reference to tower selection UI
    public TowerSelectUI towerSelectUI;
    public TowerModifyUI towerModifyUI;
    public Node selectedNode;

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
        if (towerSelectUI == null || towerModifyUI == null)
        {
            Debug.LogError("UI reference is missing in BuildManager");
            return;
        }

        //if we clicked the same node, deselect it
        if (selectedNode == node)
        {
            DeselectNode();
            return;
        }
        
        selectedNode = node; //to remember which node we clicked

        towerSelectUI.SetTarget(node);
        if (selectedNode.tower == null)
        {
            towerModifyUI.Hide();
            towerSelectUI.SetTarget(node);
        } else
        {
            towerSelectUI.Hide();
            towerModifyUI.SetTarget(node);
        }
    }
    
    void DeselectNode()
    {
        selectedNode = null;
        towerSelectUI.Hide();
        towerModifyUI.Hide();
    }
    public void BuildTowerOn(Node node, int towerID)
    {
        if (towerID < 0 || towerID >= towerPrefabs.Length)
        {
            Debug.LogError("Tower ID is invalid");
            return;
        }
		// Get the tower from the array
        Tower selectedTower = towerPrefabs[towerID];
        
		if (selectedTower == null) {
            Debug.LogError("Tower Prefab slot is empty! Fill in the Inspector menu.");
		}		


		if (BaseStats.Money >= selectedTower.cost) { //check if the player has enough money to construct the selected tower
			BaseStats.takeMoney(selectedTower.cost);
        	// Build the tower
        	// We added an offset to create a tower object right at the center of the surface of the node
        	Tower t = Instantiate(selectedTower, node.transform.position + offset, Quaternion.identity);
        	node.tower = t; //assign tower to node
            node.tower.currentNode = node; //assign node to tower
            Debug.Log("Tower Built!");
		} else {
            Debug.Log("Not Enough money. You have " + BaseStats.Money);
        }
        DeselectNode();
    }

    public void UpgradeTowerOn(Node node)
    {
        Tower currentTower = node.tower;
        if (currentTower.IsMaxLevel()) return;
        
        if(BaseStats.Money >= currentTower.upgradeCost) //If the player has enough money to upgrade
        {
            BaseStats.takeMoney(currentTower.upgradeCost); //take corresponding money
            Destroy(currentTower.gameObject); // Destroy current tower gameobject

            //Same as building, We added an offset to create a tower object right at the center of the surface of the node
            Tower newTower = Instantiate(currentTower.nextUpgradePrefab, node.transform.position + offset, Quaternion.identity);

            node.tower = newTower; //assign tower to node
            node.tower.currentNode = node; //assign node to tower
            Debug.Log("Tower Upgraded!");
        } else {
            Debug.Log("Not Enough money to upgrade. You have " + BaseStats.Money);
        }
        DeselectNode();
    }

    public void SellTowerOn(Node node)
    {
        Tower currentTower = node.tower;
        BaseStats.addMoney(currentTower.sellValue);
        Destroy(currentTower.gameObject);
        node.tower = null;

        Debug.Log("Tower Sold!");
        DeselectNode();
    }
}