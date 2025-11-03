using UnityEngine;

public class BaseStats : MonoBehaviour
{
    public static int Health;
    public int startingHealth = 20;
    void Start()
    {
        Health = startingHealth;
    }
}
