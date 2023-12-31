using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Doors : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] public Animator doorAnimation;
    [SerializeField] public GameObject lockedDoor;
    [SerializeField] GameObject noTrigCollider;

    //Just to keep track of locked doors
    [SerializeField] public bool isLocked;
    public bool isInput;

    private void Start()
    {
        isLocked = true;
        noTrigCollider = GameObject.FindWithTag("Door Collider");
        lockedDoor = GameObject.FindWithTag("Locked Door");
    }

    private void Update()
    {
        isInput = Input.GetButton("Interact");
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            if (GameManager.Instance.enemiesRemaining <= 0)
            {
                //This will only access the door in the very very beginning 
                GameManager.Instance.interactPrompt.GetComponentInChildren<TMP_Text>().enabled = true;
                isLocked = false;
            }
            else
            {
                isLocked = true;
                lockedDoor.GetComponentInChildren<TMP_Text>().enabled = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isLocked)
        {
            if (isInput && GameManager.Instance.enemiesRemaining == 0)
            {
                doorAnimation.SetBool("isPlayer", true);
                noTrigCollider.SetActive(false);
            }

        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.interactPrompt.GetComponentInChildren<TMP_Text>().enabled = false;
            lockedDoor.GetComponentInChildren<TMP_Text>().enabled = false;
            doorAnimation.SetBool("isPlayer", false);
        }
    }
}

