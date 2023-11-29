using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("===== Components =====")]
    [SerializeField] Rigidbody rb;

    [Header("===== Stats =====")]
    [SerializeField] public int damage;
    [SerializeField] public int speed;
    [SerializeField] public int destroyTime;
    [SerializeField] public ParticleSystem hitEffect;

    [Header("===== Status Effect =====")]
    [Range(0, 5)][SerializeField] float damageCount;
    [Range(1, 3)][SerializeField] float timeIntervalStatusEffect;
    [Range(0, 3)][SerializeField] int StatusEffectDamage;

    bool isHurting;
    PlayerController player;
    int statusEffectCount;


    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (tag != "Skeleton Temp Bullet")
        {
            Instantiate(hitEffect, gameObject.transform.position, hitEffect.transform.rotation);
        }

        if (other.isTrigger)
        {
            return;
        }

        IDamage damageable = other.GetComponent<IDamage>();

        if(damageable != null)
        {
            if (other.CompareTag("Player"))
            {
                damageable.takeDamage(damage);

                if (isHurting == false)
                {
                    StartCoroutine(StatusEffectInterval());
                }
            }
        }

        Destroy(gameObject);
    }

    public void SetDestroyTime(int time)
    {
        destroyTime = time;
    }

    public void setHitEffect(ParticleSystem spellHitEffect)
    {
        hitEffect = spellHitEffect;
    }

    IEnumerator StatusEffectInterval()
    {
        if (statusEffectCount < damageCount)
        {
            isHurting = true;

            statusEffectCount++;

            player = player.GetComponent<PlayerController>();

            Mathf.Clamp(player.Hp - StatusEffectDamage, 0, player.HPOrig);

            player.updatePlayerHealthUI();
        }

        yield return new WaitForSeconds(timeIntervalStatusEffect);

        isHurting = true;
    }
}
