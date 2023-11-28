using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkeletonSword : MonoBehaviour
{
    [Header("----- Sword Stats -----")]
    [SerializeField] int damage;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        else if (other.CompareTag("Player"))
        {
            IDamage damageable = other.GetComponent<IDamage>();
            if (damageable != null)
            {
                damageable.takeDamage(damage);
            }
        }
    }
}
