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
        doorAnimation.SetInteger("doorStop", 1);
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
            doorAnimation.SetInteger("doorStop", 1);

            if (GameManager.Instance.enemiesRemaining <= 0)
            {
                Debug.Log("The box has been hit!");
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
        Debug.Log(isLocked);
        if (other.CompareTag("Player") && !isLocked)
        {
            if (isInput && doorAnimation.GetInteger("doorStop") == 1)
            {
                GameManager.Instance.playerScript.playerAnim.SetBool("isInteract", true);
                Debug.Log(isInput);
                doorAnimation.SetBool("isPlayer", true);
                noTrigCollider.SetActive(false);
                GameManager.Instance.playerScript.playerAnim.SetBool("isInteract", false);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorAnimation.SetInteger("doorStop", 0);
            GameManager.Instance.interactPrompt.GetComponentInChildren<TMP_Text>().enabled = false;
            lockedDoor.GetComponentInChildren<TMP_Text>().enabled = false;
            doorAnimation.SetBool("isPlayer", false);
            noTrigCollider.SetActive(true);
        }
    }
}

