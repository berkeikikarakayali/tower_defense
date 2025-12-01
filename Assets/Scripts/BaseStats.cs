using UnityEngine;

public class BaseStats : MonoBehaviour
{
    public static int Health;
    public int startingHealth = 20;
    
    public static int Money; //Static because we need to reach this variable from other scripts.
    public int startingMoney = 20;
    void Start()
    {
        Health = startingHealth;
        Money = startingMoney;
    }
}
