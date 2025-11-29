using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private GameObject deathVFXPrefab;
    [SerializeField] private float knockBackThrust = 15f;

    [Header("Damage Text")]
    [SerializeField] private GameObject damageTextPrefab;

    private int currentHealth;
    private KnockBack knockBack;
    private Flash flash;

    private void Awake()
    {
        flash = GetComponent<Flash>();
        knockBack = GetComponent<KnockBack>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        knockBack.GetKnockedBack(PlayerController.Instance.transform, knockBackThrust);
        StartCoroutine(flash.FlashRoutine());
        StartCoroutine(CheckDetectDeathRoutine());

        if (damageTextPrefab)
        {
            Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 0.5f, 0);
            GameObject damageText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity);
            // Add damage in text
            damageText.GetComponent<DamageText>().Setup(damage);
        }
    }

    private IEnumerator CheckDetectDeathRoutine()
    {
        yield return new WaitForSeconds(flash.GetRetoreMatTime());
        DetectDeath();
    }

    public void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            GetComponent<PickUpSpawner>().DropItems();
            Destroy(gameObject);
        }
    }
}
