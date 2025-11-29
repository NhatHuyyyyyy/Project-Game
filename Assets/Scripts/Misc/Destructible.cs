using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private GameObject deathVFX;
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<DamageSource>() || other.gameObject.GetComponent<Projectile>())
        {
            audioManager.PlaySFX(audioManager.destructible);
            GetComponent<PickUpSpawner>().DropItems();
            Instantiate(deathVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
