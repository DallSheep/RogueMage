using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaDrop : MonoBehaviour
{
    [Header("----- Stats -----")]
    [SerializeField] int staminaAmount;

    private void OnTriggerEnter(Collider other)
    {
        if(other.isTrigger)
        {
            return;
        }

        if(other.CompareTag("Player"))
        {

        }
    }
}
