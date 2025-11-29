using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [SerializeField] public int maxHeath = 10;
    [SerializeField] private GameObject deadVFXPrefab;
    [SerializeField] private float knockBackThrust = 10f;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject healthBoss;
    [SerializeField] private GameObject portal;

    public int currentHealth;
    private KnockBack knockBack;
    private Flash flash;
    private Animator anim;
    private bool isDead = false;
    private AudioManager audioManager;

    private BossAI bossAI;

    private void Awake()
    {
        knockBack = GetComponent<KnockBack>();
        flash = GetComponent<Flash>();
        anim = GetComponent<Animator>();
        bossAI = GetComponent<BossAI>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        currentHealth = maxHeath;

        UpdateHealthSlider();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthSlider();
        knockBack.GetKnockedBack(PlayerController.Instance.transform, knockBackThrust);
        StartCoroutine(flash.FlashRoutine());
        StartCoroutine(CheckDetectDeathRoutine());
    }

    private IEnumerator CheckDetectDeathRoutine()
    {
        yield return new WaitForSeconds(flash.GetRetoreMatTime());
        DetectDeath();
    }

    private void DetectDeath()
    {
        if (currentHealth > 0 || isDead) return;

        isDead = true;
        anim.SetTrigger("Death");
        audioManager.PlaySFX(audioManager.bossDeath);
        Instantiate(deadVFXPrefab, transform.position, Quaternion.identity);
        bossAI.KillAllMinions();
        bossAI.enabled = false;

        if (portal != null)
        {
            portal.SetActive(true); 
        }

        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(1f); 
        Destroy(gameObject);
        healthBoss.SetActive(false);
    }

    private void UpdateHealthSlider()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHeath;
            healthSlider.value = currentHealth;
        }
    }
}
