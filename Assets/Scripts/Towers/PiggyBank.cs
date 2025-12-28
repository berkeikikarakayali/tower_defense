using UnityEngine;

public class PiggyBank : Tower
{
    public int incomeValue = 2; //2$ per interval
    public float interval = 5; //time
    public GameObject moneyText; 
    //reference to FloatingText prefab /TextMeshPro 
    //we will show how much money it gives on top of the bank like enemy death text


    private Spawner spawnerScript; //spawne script reference to track is the wave active
    public override void Start()
    {
        base.Start();
        spawnerScript = FindFirstObjectByType<Spawner>();

        if (spawnerScript == null)
        {
            Debug.LogError("Bank could not find a Spawner script!");
        }   
        InvokeRepeating("GenerateMoney", 0f, interval);
    }

    void GenerateMoney()
    {
        if (spawnerScript == null) return;
        if (spawnerScript.isWaveActive == false) return;

        if (moneyText != null)
        {
            GameObject text_ins = Instantiate(moneyText, transform.position, Quaternion.identity);
            FloatingText text_script = text_ins.GetComponent<FloatingText>();
            if(text_script != null)
            {
                text_script.SetText("+$"+ incomeValue);
            }
        }
        AudioManager.audioManager.PlaySound("CoinPickup");
        BaseStats.addMoney(incomeValue);
    }
}
