using UnityEngine;

public class Grape : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject grapeProjectilePrefab;

    private Animator anim;
    private SpriteRenderer spriteRenderer;

    readonly int ATTACK_HASH = Animator.StringToHash("Attack");

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Attack()
    {
        anim.SetTrigger(ATTACK_HASH);

        if (transform.position.x - PlayerController.Instance.transform.position.x < 0)
        {
            spriteRenderer.flipX = false;
        } else
        {
            spriteRenderer.flipX = true;
        }
    }

    public void SpawnProjectileAnimEvent()
    {
        Instantiate(grapeProjectilePrefab, transform.position, Quaternion.identity);
    }

}
