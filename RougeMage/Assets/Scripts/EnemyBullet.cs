using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class EnemyBullet : MonoBehaviour
{
    [Header("===== Components =====")]
    [SerializeField] Rigidbody rb;
    public PlayerController player;
    public IDamage damageable;

    [Header("===== Stats =====")]
    [SerializeField] public int damage;
    [SerializeField] public int speed;
    [SerializeField] public int destroyTime;
    [SerializeField] public ParticleSystem hitEffect;

    [Header("===== Status Effect =====")]
    [Range(0, 5)][SerializeField] public float damageCount;
    [Range(1, 3)][SerializeField] public float timeIntervalStatusEffect;
    [Range(0, 10)][SerializeField] public int statusEffectDamage;

    [Header("===== Status Effect UI =====")]
    public Image statusEffectFlameUI;

    bool isPlayerCurrAffected;
    int statusEffectCount = 0;

    void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        player = other.GetComponent<PlayerController>();

        if (other.isTrigger)
        {
            return;
        }
       
        damageable = other.GetComponent<IDamage>();
      
        if (damageable != null)
        {
            if (other.CompareTag("Player"))
            {
                if (gameObject.CompareTag("Acid Spitball") || gameObject.CompareTag("Flameball"))
                {
                    if (isPlayerCurrAffected)
                    {
                        damageable.takeDamage(damage);
                    }
                    else
                    {
                        damageable.takeDamage(damage);
                        StartCoroutine(StatusEffect());
                    }
                }
                else
                {
                    damageable.takeDamage(damage);
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

    public IEnumerator StatusEffect()
    {
        isPlayerCurrAffected = true;
        while (isPlayerCurrAffected)
        {
            if (statusEffectCount == damageCount || player.Hp == 1)
            {
                isPlayerCurrAffected = false;
                statusEffectCount = 0;
            }

            statusEffectCount++;

            Mathf.Clamp(player.Hp - statusEffectDamage, 0, player.HPOrig);
            player.updatePlayerHealthUI();

            Debug.Log("Status Effect");

            yield return new WaitForSeconds(timeIntervalStatusEffect);
        }
    }
}
