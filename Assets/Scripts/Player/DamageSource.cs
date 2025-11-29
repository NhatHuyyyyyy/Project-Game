using System.Runtime.CompilerServices;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    private int damage;

    private void Start()
    {
        MonoBehaviour currentActiveWeapon = ActiveWeapon.Instance.CurrentActiveWeapon;
        damage = (currentActiveWeapon as IWeapon).GetWeaponInfo().weaponDamage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        BossHealth bossHealth = collision.gameObject.GetComponent<BossHealth>();
        enemyHealth?.TakeDamage(damage);
        bossHealth?.TakeDamage(damage);
    }
}
