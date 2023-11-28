using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : MonoBehaviour
{
    [Header("----- Stats -----")]
    [SerializeField] int healthAmount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            Mathf.Clamp(player.Hp += healthAmount, 0, player.HPOrig);
        }
    }
}
