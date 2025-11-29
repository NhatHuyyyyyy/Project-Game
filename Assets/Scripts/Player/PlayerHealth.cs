using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlayerHealth : Singleton<PlayerHealth>
{
    public bool isDead { get; private set; }

    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;
    [SerializeField] private Transform spawnPoint;

    private AudioManager audioManager;
    private Slider healthSlider;
    private int currentHealth;
    private bool canTakeDamage = true;
    private KnockBack knockBack;
    private Flash flash;
    private Vector3 spawnPosition;

    private static Dictionary<string, Vector3> sceneCheckpoints = new Dictionary<string, Vector3>();

    const string HEALTH_SLIDER_TEXT = "Health Slider";
    const string TOWN_TEXT = "Scene1";
    readonly int DEATH_HASH = Animator.StringToHash("Death");



    protected override void Awake()
    {
        base.Awake();

        flash = GetComponent<Flash>();
        knockBack = GetComponent<KnockBack>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        isDead = false;
        currentHealth = maxHealth;

        string currentScene = SceneManager.GetActiveScene().name;

        if (sceneCheckpoints.ContainsKey(currentScene))
        {
            transform.position = sceneCheckpoints[currentScene];
        }
        else
        {
            transform.position = spawnPoint.position; 
        }

        UpdateHealthSlider();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (isDead) return;
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();
        BossAI boss = other.gameObject.GetComponent<BossAI>();

        if (enemy || boss) 
        {
            TakeDamage(1, other.transform);
        }
    }

    public void HealPlayer(int amount)
    {
        if (isDead || currentHealth >= maxHealth) return;
        {
            currentHealth += amount;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            UpdateHealthSlider();
        }
    }

    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (!canTakeDamage || isDead) { return; }

        ScreenShakeManager.Instance.ShakeScreen();
        knockBack.GetKnockedBack(hitTransform, knockBackThrustAmount);
        StartCoroutine(flash.FlashRoutine());
        canTakeDamage = false;
        currentHealth -= damageAmount;
        StartCoroutine(DamageRecoveryTime());
        UpdateHealthSlider();
        CheckIfPlayerDeath();
    }

    public void UpdateSpawnPoint(Vector3 newSpawnPosition)
    {
        string currentScene = SceneManager.GetActiveScene().name;
        sceneCheckpoints[currentScene] = newSpawnPosition;
    }

    private void CheckIfPlayerDeath()
    {
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            Destroy(ActiveWeapon.Instance.gameObject);
            currentHealth = 0;
            GetComponent<Animator>().SetTrigger(DEATH_HASH);
            StartCoroutine(DeadLoadSceneRoutine());
        }
    }

    private IEnumerator DeadLoadSceneRoutine()
    {
        audioManager.PlaySFX(audioManager.lose);
        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
        SceneManager.LoadScene(TOWN_TEXT);
    }

    private IEnumerator DamageRecoveryTime()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private void UpdateHealthSlider()
    {
        if (healthSlider == null)
        {
            healthSlider = GameObject.Find(HEALTH_SLIDER_TEXT).GetComponent<Slider>();
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
}
