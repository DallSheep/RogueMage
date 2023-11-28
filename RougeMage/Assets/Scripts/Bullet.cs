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

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Instantiate(hitEffect, gameObject.transform.position, hitEffect.transform.rotation);

        if (other.isTrigger)
        {
            return;
        }
        IDamage damageable = other.GetComponent<IDamage>();

        if(damageable != null && other.CompareTag("Player"))
        {
            damageable.takeDamage(damage);
        }

        Destroy(gameObject);
        //Destroy(hitEffect);
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
