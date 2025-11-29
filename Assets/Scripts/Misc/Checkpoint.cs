using UnityEditor;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth.Instance.UpdateSpawnPoint(transform.position);
            audioManager.PlaySFX(audioManager.checkPoint);
        }
    }
}
