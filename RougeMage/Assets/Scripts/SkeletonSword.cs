using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSword : MonoBehaviour
{
    [Header("----- Sword Stats -----")]
    [SerializeField] int damage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        IDamage damageable = other.GetComponent<IDamage>();
        if(damageable != null)
        {
            damageable.takeDamage(damage);
        }
    }

}
