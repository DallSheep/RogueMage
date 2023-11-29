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
        Debug.Log("OnTriggerEnter");
        
        if (tag != "Skeleton Temp Bullet")
        {
            Debug.Log("BeforeHitEffect");
            //Instantiate(hitEffect, gameObject.transform.position, hitEffect.transform.rotation);
            Debug.Log("HitEffect");
        }
        
        Debug.Log("AfterSkeleton");
        if (other.isTrigger)
        {
            Debug.Log("other");
            return;
        }
        Debug.Log("BeforeIDamage");
        IDamage damageable = other.GetComponent<IDamage>();
        Debug.Log("AfterIDamage");
        if(damageable != null)
        {
            //if (other.CompareTag("Player"))
            //{
                //damageable.takeDamage(damage);
            //}
            damageable.takeDamage(damage);
            Debug.Log("TakesDamage");
        }
        Destroy(gameObject);
        Debug.Log("Destory");
    }

    public void SetDestroyTime(int time)
    {
        destroyTime = time;
    }

    public void setHitEffect(ParticleSystem spellHitEffect)
    {
        hitEffect = spellHitEffect;
    }
}
