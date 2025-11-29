using UnityEngine;

public class Staff : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject magicLaser;
    [SerializeField] private Transform magicLaserSpawnPoint;

    private Animator anim;
    private AudioManager audioManager;

    readonly int ATTACK_HASH = Animator.StringToHash("Attack");

    private void Awake()
    {
        anim = GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }

    public void Attack()
    {
        anim.SetTrigger(ATTACK_HASH);
    }

    public void SpawnStaffProjectileAnimEvent()
    {
        audioManager.PlaySFX(audioManager.spellCast);
        GameObject newLaser = Instantiate(magicLaser, magicLaserSpawnPoint.position, transform.rotation);
        newLaser.GetComponent<MagicLaser>().UpdateLaserRange(weaponInfo.weaponRange);
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        if (mousePos.x < playerScreenPoint.x)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
