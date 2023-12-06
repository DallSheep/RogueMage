using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCatapultBullet : MonoBehaviour
{
    [Header("===== Components =====")]
    [SerializeField] Rigidbody rb;

    [Header("===== Stats =====")]
    [SerializeField] public int damage;
    [SerializeField] public float speed;
    [SerializeField] public float destroyTime;
    [SerializeField] GameObject explosion;
    [SerializeField] public ParticleSystem hitEffect;

    [Header("===== Status Effect =====")]
    [Range(0, 5)][SerializeField] float damageCount;
    [Range(1, 3)][SerializeField] float timeIntervalStatusEffect;
    [Range(0, 3)][SerializeField] int StatusEffectDamage;

    bool isHurting;
    PlayerController player;
    int statusEffectCount;

    void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        IDamage damageable = other.GetComponent<IDamage>();

        if (damageable != null && !other.CompareTag("Player")) 
        {
            damageable.takeDamage(damage);
            Instantiate(explosion, transform.position, explosion.transform.rotation);
            Debug.Log("Explosion");
        }

        Destroy(gameObject);
    }

    public void SetDestroyTime(float time)
    {
        destroyTime = time;
    }

    public void setHitEffect(ParticleSystem spellHitEffect) 
    {
        hitEffect = spellHitEffect;
    }
}
