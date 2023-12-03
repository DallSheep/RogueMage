using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBullet : MonoBehaviour
{
    [Header("===== Components =====")]
    [SerializeField] Rigidbody rb;

    [Header("===== Stats =====")]
    [SerializeField] public int damage;
    [SerializeField] public float speed;
    [SerializeField] public float destroyTime;
    [SerializeField] public ParticleSystem hitEffect;

    [Header("===== Status Effects =====")]
    [Range(0, 5)][SerializeField] float damageCount;
    [Range(1, 3)][SerializeField] float timeInvervalStatusEffect;
    [Range(0, 3)][SerializeField] int statusEffectDamage;

    bool isHurting;
    PlayerController player;
    int statusEffectCount;
    void Start()
    {
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
            Destroy(gameObject);
        }
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
