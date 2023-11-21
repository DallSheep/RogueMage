using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Doors : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] Animator doorAnimation;
    [SerializeField] TMP_Text lockedDoor;
    [SerializeField] TMP_Text unlockedDoor;

    [SerializeField] public bool isLocked;

    private void Start()
    {
        isLocked = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")/*&& GameManager.Instance.enemiesRemaining <= 0*/)
        {
            isLocked = false;
            unlockedDoor.gameObject.SetActive(true);
            if (Input.GetButtonDown("Interact"))
            {
                doorAnimation.Play("DoorOpen", 0, 0.0f);
            }
        }
        else
        {
            isLocked = true;
            lockedDoor.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorAnimation.Play("DoorClose", 0, 0.0f);
            //gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
