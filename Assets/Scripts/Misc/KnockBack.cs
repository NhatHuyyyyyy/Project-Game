using System.Collections;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public bool GettingKnockedBack { get; private set; }

    [SerializeField] private float knockBackTime = .2f;

    private Rigidbody2D rb;
    private AudioManager audioManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void GetKnockedBack(Transform dameSource, float knockBackThrust)
    {
        audioManager.PlaySFX(audioManager.hit);
        GettingKnockedBack = true;
        Vector2 difference = (transform.position - dameSource.position).normalized * knockBackThrust * rb.mass;
        rb.AddForce(difference, ForceMode2D.Impulse);
        StartCoroutine(KnockRoutine());
    }

    private IEnumerator KnockRoutine()
    {
        yield return new WaitForSeconds(knockBackTime);
        rb.linearVelocity = Vector2.zero;
        GettingKnockedBack = false;
    }
}
