using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Doors : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] public Animator doorAnimation;
    [SerializeField] public TMP_Text lockedDoor;
    [SerializeField] public TMP_Text unlockedDoor;

    //Just to keep track of locked doors
    [SerializeField] public bool isLocked;

    private void Start()
    {
        isLocked = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") /*&& GameManager.Instance.enemiesRemaining <= 0*/)
        {
            Debug.Log("The box has been hit!");
            //This will only access the door in the very very beginning 
            GameManager.Instance.DoorMenus();
        }
        else
        {
            isLocked = true;
            lockedDoor.gameObject.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !GameManager.Instance.startDoorCollider)
        {
            doorAnimation.Play("DoorClose", 0, 0.0f);
            gameObject.GetComponent<BoxCollider>().enabled = false;
            gameObject.GetComponentInParent<BoxCollider>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            gameObject.GetComponentInParent<BoxCollider>().enabled = false;
        }
    }
}
