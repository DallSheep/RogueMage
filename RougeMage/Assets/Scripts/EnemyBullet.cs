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

    [Header("===== Status Effect components =====")]
    public AcidStatusEffect acidStatusEffect;
    public FireStatusEffect fireStatusEffect;
    public GameObject statusEffect;

    void Start()
    {
        statusEffect = GameObject.FindWithTag("Status Effect");
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, destroyTime);
    }

    void OnTriggerEnter(Collider other)
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
                /*                if (gameObject.CompareTag("Acid Spitball"))
                                {
                                    damageable.takeDamage(damage);
                                    player.updatePlayerHealthUI();

                                    acidStatusEffect = player.GetComponent<AcidStatusEffect>();

                                    if (acidStatusEffect == null)
                                    {
                                        player.AddComponent<AcidStatusEffect>();
                                        //statusEffect.GetComponent<StatusEffect>().StartDamage();
                                    }
                                }
                                else if (gameObject.CompareTag("Flameball"))
                                {
                                    damageable.takeDamage(damage);
                                    player.updatePlayerHealthUI();

                                    fireStatusEffect = player.GetComponent<FireStatusEffect>();

                                    if (fireStatusEffect == null)
                                    {
                                        player.AddComponent<FireStatusEffect>();
                                        statusEffect.GetComponent<StatusEffect>().StartDamage();
                                    }
                                }
                                else
                                {
                                    damageable.takeDamage(damage);
                                    player.updatePlayerHealthUI();
                                }*/
                damageable.takeDamage(damage);
                player.updatePlayerHealthUI();
            }
        }
        Instantiate(hitEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
