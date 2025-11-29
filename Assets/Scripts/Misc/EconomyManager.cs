using TMPro;
using UnityEngine;

public class EconomyManager : Singleton<EconomyManager>
{
    private TMP_Text goldText;
    private int currentGold = 0;

    const string COIN_AMOUNT_TEXT = "Gold Amount Text";

    private void Start()
    {
        if (goldText == null)
        {
            GameObject textObj = GameObject.Find(COIN_AMOUNT_TEXT);
            if (textObj != null)
            {
                goldText = textObj.GetComponent<TMP_Text>();
            }
        }

        UpdateUI();
    }

    public void UpdateCurrentGold()
    {
        currentGold += 1;
        UpdateUI();
    }

    public int GetCurrentGold()
    {
        return currentGold;
    }

    public bool SpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            UpdateUI();
            return true;
        }
        return false;
    }

    private void UpdateUI()
    {
        if (goldText != null)
        {
            goldText.text = currentGold.ToString("D3");
        }
    }
}
