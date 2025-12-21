using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerModifyUI : MonoBehaviour
{
    public GameObject UIPanel; //UI reference
    public float yOffset = 50f;
    public float zOffset = -15f;
    [Header("Upgrade Button")]
    public Button upgradeButton;
    public TextMeshProUGUI upgradeText;
    [Header("Upgrade Button")]
    public Button sellButton;
    public TextMeshProUGUI sellText;

    public Node selectedNode;

    public void SetTarget(Node targetNode)
    {
        selectedNode = targetNode;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(targetNode.transform.position);

        float screenCenterHeight = Screen.height / 2f;
        if(screenPos.y < screenCenterHeight)
        {
            screenPos.y += yOffset;
        } else
        {
            screenPos.y -= yOffset;
        }

        screenPos.x += zOffset;
        transform.position = screenPos;
        
        UIPanel.SetActive(true); // Show the menu

        Tower currentTower = selectedNode.tower;
        if (!currentTower.IsMaxLevel())
        {
            upgradeButton.gameObject.SetActive(true);
            upgradeText.text = "UPGRADE\n$" + currentTower.upgradeCost;
            upgradeButton.interactable = (BaseStats.Money >= currentTower.upgradeCost);
        } else
        {
            upgradeButton.gameObject.SetActive(false);
        }

        sellText.text = "SELL\n$" + currentTower.sellValue;
        sellButton.interactable = true;  
    }
        public void Hide()
    {
        UIPanel.SetActive(false); // Close the menu
    }

    public void OnUpgradeClick()
    {
        if(selectedNode != null)
        {
            BuildManager.buildManager.UpgradeTowerOn(selectedNode);
        }
    }

    public void OnSellClick()
    {
        if(selectedNode != null)
        {
            BuildManager.buildManager.SellTowerOn(selectedNode);
        }
    }
}
