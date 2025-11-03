using UnityEngine;

public class BaseStats : MonoBehaviour
{
    public static int Health;
    public int startingHealth;
    void Start()
    {
        Health = startingHealth;
    }
}
