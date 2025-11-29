using UnityEngine;
using TMPro;
using System.Collections;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [Header("UI Reference")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private TMP_Text feedbackText; 

    [Header("Shop Settings")]
    [SerializeField] private int potionPrice = 10;
    [SerializeField] private int healAmount = 1; 

    private void Awake()
    {
        Instance = this;
        shopPanel.SetActive(false); 
    }

    public void OpenShop()
    {
        shopPanel.SetActive(true);
        if (feedbackText) feedbackText.text = "Welcome!";
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
    }

    public void BuyHealthPotion()
    {
        if (EconomyManager.Instance.SpendGold(potionPrice))
        {

            PlayerHealth.Instance.HealPlayer(healAmount);
            UpdateFeedback("Purchased!");
        }
        else
        {
            UpdateFeedback("Not enough Gold!");
        }
    }

    private void UpdateFeedback(string message)
    {
        if (feedbackText != null)
        {
            StopAllCoroutines();
            StartCoroutine(FeedbackRoutine(message));
        }
    }

    private IEnumerator FeedbackRoutine(string message)
    {
        feedbackText.text = message;
        yield return new WaitForSeconds(2f); 
        feedbackText.text = ""; 
    }
}