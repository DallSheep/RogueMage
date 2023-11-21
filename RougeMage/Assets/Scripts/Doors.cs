using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Doors : MonoBehaviour
{
    [SerializeField] Animator doorAnimation;
    [SerializeField] TMP_Text lockedDoor;
    [SerializeField] TMP_Text unlockedDoor;

    bool isLocked;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorAnimation.Play("DoorOpen", 0, 0.0f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorAnimation.Play("DoorClose", 0, 0.0f);
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
