using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    [SerializeField] private GameObject visualCue;
    private bool playerInRange;
    private bool isShopOpen = false;

    private void Awake()
    {
        if (visualCue != null) visualCue.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isShopOpen)
            {
                ShopManager.Instance.OpenShop();
                isShopOpen = true;
            }
            else
            {
                ShopManager.Instance.CloseShop();
                isShopOpen = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            if (visualCue != null) visualCue.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            if (visualCue != null) visualCue.SetActive(false);

            ShopManager.Instance.CloseShop();
            isShopOpen = false;
        }
    }
}
