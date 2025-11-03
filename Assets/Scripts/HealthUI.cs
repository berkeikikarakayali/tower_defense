using UnityEngine;
using TMPro;
public class HealthUI : MonoBehaviour
{
    public TMP_Text healthText;
    
    private int previousHealthValue;
    
    void Start()
    {
        previousHealthValue = BaseStats.Health;
        healthText.text = previousHealthValue.ToString() + " HEALTH";
    }
    void Update()
    {
        if (BaseStats.Health != previousHealthValue)
        {   
            //Debug.Log(BaseStats.Health);
            healthText.text = BaseStats.Health.ToString() + " HEALTH";
            previousHealthValue = BaseStats.Health;
        }
    }
}
