using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaDrop : MonoBehaviour
{
    [SerializeField] float manaAmount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {

        }
    }
}
