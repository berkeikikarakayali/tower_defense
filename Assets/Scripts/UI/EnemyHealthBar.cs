using UnityEngine;
using UnityEngine.UI;
public class EnemyHealthBar : MonoBehaviour
{
    public Image healthImage;
    
    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        // Handles the math
        healthImage.fillAmount = currentHealth / maxHealth;
    }
}
