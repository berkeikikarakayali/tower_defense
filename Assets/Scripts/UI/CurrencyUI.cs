using UnityEngine;
using TMPro;
public class CurrencyUI : MonoBehaviour
{
    public TMP_Text currencyText;
    
    private int previousCurrencyValue;
    
    void Start()
    {
        previousCurrencyValue = BaseStats.Money;
        currencyText.text = previousCurrencyValue.ToString();
    }
    void Update()
    {
        if (BaseStats.Money != previousCurrencyValue)
        {   
            //Debug.Log(BaseStats.Money);
            currencyText.text = BaseStats.Money.ToString();
            previousCurrencyValue = BaseStats.Money;
        }
    }
}