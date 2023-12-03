using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockExplosion : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] ParticleSystem explosionEffect;


    void Start()
    {
        
    }

    IEnumerator destroy()
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, explosionEffect.transform.rotation);
        }
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        IDamage damageable = other.GetComponent<IDamage>();

        if (damageable != null)
        {
            damageable.takeDamage(damage);
        }

        Destroy(gameObject);
    }
}
