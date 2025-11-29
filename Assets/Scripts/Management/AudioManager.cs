using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [Header("-------- Audio Source --------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;


    [Header("-------- Audio Clip --------")]
    public AudioClip background;
    public AudioClip dash;
    public AudioClip hit;
    public AudioClip pickup;
    public AudioClip walk;
    public AudioClip swordAttack;
    public AudioClip arrowShot;
    public AudioClip spellCast;
    public AudioClip checkPoint;
    public AudioClip destructible;
    public AudioClip bossDeath;
    public AudioClip bossThrow;
    public AudioClip bossSpawn;
    public AudioClip lose;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
