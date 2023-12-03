using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkeletonSword : MonoBehaviour
{
    [Header("----- Sword Stats -----")]
    [SerializeField] int damage;

    bool isAttacking;
    IDamage damageable;

    void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        else if (other.CompareTag("Player"))
        {
            damageable = other.GetComponent<IDamage>();
            if (damageable != null)
            {
                if (other.CompareTag("Player") && !isAttacking)
                {
                    StartCoroutine(attacking());
                }
            }
        }
    }

    public IEnumerator attacking()
    {
        isAttacking = true;
        damageable.takeDamage(damage);
        yield return new WaitForSeconds(2);

        isAttacking = false;
    }

}
