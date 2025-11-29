using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float attackRange = 3f;

    [Header("SkillSetting")]
    [SerializeField] private float dashCooldown = 2.5f;
    [SerializeField] private float dashSpeed = 12f;
    [SerializeField] private float dashTime = 1f;
    [SerializeField] private float shootCooldown = 2.5f;
    [SerializeField] private float summonCooldown = 2.5f;
    [SerializeField] private float phase2SpeedMultiplier = 0.7f;

    [Header("Reference")]
    private BossHealth bossHealth;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Transform player;
    private List<GameObject> spawnedMinions = new List<GameObject>();

    [SerializeField] private GameObject spawnVFXPrefab;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject[] minionPrefabs;

    private Vector2 moveDir;
    private int currentSkill = 0;
    private bool isDoingSkill = false;
    private KnockBack knockBack;
    private AudioManager audioManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bossHealth = GetComponent<BossHealth>();
        knockBack = GetComponent<KnockBack>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void FixedUpdate()
    {
        if (bossHealth.currentHealth <= 0 || knockBack.GettingKnockedBack) { return; }

        if (isDoingSkill) { return; }

        moveDir = (player.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            rb.MovePosition(rb.position + moveDir * (speed * Time.fixedDeltaTime));
        }
        else
        {
            if (!isDoingSkill)
                StartCoroutine(DoSkill());
        }
        if (moveDir.x < 0)
            spriteRenderer.flipX = true;
        else if (moveDir.x > 0)
            spriteRenderer.flipX = false;
    }

    public IEnumerator DoSkill()
    {
        isDoingSkill = true;

        bool phase2 = bossHealth.currentHealth <= bossHealth.maxHeath / 2;

        switch (currentSkill)
        {
            case 0:
                yield return DashSkill(phase2);
                break;
            case 1:
                yield return ShootTriple(phase2);
                break;
            case 2:
                yield return SummonMinions(phase2);
                break;
            case 3:
                if (phase2)
                    yield return ShootCircle();
                break;
        }

        currentSkill++;
        if (phase2)
        {
            if (currentSkill > 3) 
                currentSkill = 0;
        } else
        {
            if (currentSkill > 2) 
                currentSkill = 0;
        }

            isDoingSkill = false;
    }

    // Dash skill
    public IEnumerator DashSkill(bool phase2)
    {
        audioManager.PlaySFX(audioManager.dash);

        float currentDashSpeed = phase2 ? dashSpeed * 1.2f : dashSpeed;
        float cd = phase2 ? dashCooldown * phase2SpeedMultiplier : dashCooldown;

        float t = 0f;

        while (t < dashTime)
        {
            rb.MovePosition(rb.position + moveDir * currentDashSpeed * Time.fixedDeltaTime);
            t += Time.fixedDeltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(cd);
    }


    // Shoot triple rock
    public IEnumerator ShootTriple(bool phase2)
    {
        float cd = phase2 ? shootCooldown * phase2SpeedMultiplier : shootCooldown;

        animator.SetTrigger("Shoot");

        audioManager.PlaySFX(audioManager.bossThrow);
        Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
        float angleMain = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        ShootProjectile(angleMain);
        ShootProjectile(angleMain + 15f);
        ShootProjectile(angleMain - 15f);

        yield return new WaitForSeconds(cd);
    }

    private void ShootProjectile(float angle)
    {
        if (!projectilePrefab) return;

        Quaternion rot = Quaternion.Euler(0f, 0f, angle);
        Instantiate(projectilePrefab, transform.position, rot);
    }

    // Summon minion skill
    public IEnumerator SummonMinions(bool phase2)
    {
        float cd = phase2 ? shootCooldown * phase2SpeedMultiplier : shootCooldown;

        animator.SetTrigger("Summon");
        audioManager.PlaySFX(audioManager.bossSpawn);

        int count = phase2 ? 2 : 1;

        for (int i = 0; i < count; i++)
        {
            Vector2 offset = Random.insideUnitCircle * 2f;
            Vector2 spawnPos = (Vector2)transform.position + offset;

            GameObject minionToSpawn = minionPrefabs[Random.Range(0, minionPrefabs.Length)];
            GameObject spawned = Instantiate(minionToSpawn, spawnPos, Quaternion.identity);
            spawnedMinions.Add(spawned);
            Instantiate(spawnVFXPrefab, spawnPos, Quaternion.identity);
        }

        yield return new WaitForSeconds(cd);
    }

    public void KillAllMinions()
    {
        foreach (var minion in spawnedMinions)
        {
            if (minion != null)
                Destroy(minion);
        }
        spawnedMinions.Clear();
    }

    // Shoot Circle (Phase 2)
    public IEnumerator ShootCircle()
    {
        animator.SetTrigger("Shoot");
        audioManager.PlaySFX(audioManager.bossThrow);
        int bulletCount = 20;
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = (360f / bulletCount) * i;
            Quaternion rot = Quaternion.Euler(0, 0, angle);
            Instantiate(projectilePrefab, transform.position, rot);
        }

        yield return new WaitForSeconds(1.5f);
    }
}

