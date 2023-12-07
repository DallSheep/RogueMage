using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class EnemyBullet : MonoBehaviour
{
    [Header("===== Components =====")]
    [SerializeField] Rigidbody rb;
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
    AcidStatusEffect acidStatusScript;
    FireStatusEffect fireStatusScript;

    void Start()
    {
        statusEffect = GameObject.FindWithTag("Status Effect");
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, destroyTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
       
        damageable = other.GetComponent<IDamage>();
      
        if (damageable != null && other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("Acid Spitball"))
            {
                damageable.takeDamage(damage);
                GameManager.Instance.playerScript.updatePlayerHealthUI();

                if (GameManager.Instance.player.GetComponent<AcidStatusEffect>() == null)
                {
                    acidStatusScript = GameManager.Instance.playerScript.AddComponent<AcidStatusEffect>();
                    acidStatusScript.SetData();
                    statusEffect.GetComponent<StatusEffect>().StartDamage();
                }
            }
            else if (gameObject.CompareTag("Flameball"))
            {
                damageable.takeDamage(damage);
                GameManager.Instance.playerScript.updatePlayerHealthUI();

                if (GameManager.Instance.player.GetComponent<FireStatusEffect>() == null)
                {
                    fireStatusScript = GameManager.Instance.player.AddComponent<FireStatusEffect>();
                    fireStatusScript.SetData();
                    statusEffect.GetComponent<StatusEffect>().StartDamage();
                }
            }
            else
            {
                damageable.takeDamage(damage);
                GameManager.Instance.playerScript.updatePlayerHealthUI();
            }
        }

        Instantiate(hitEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
