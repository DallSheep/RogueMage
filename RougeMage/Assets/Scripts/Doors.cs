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

    //Just to keep track of locked doors
    [SerializeField] public bool isLocked;

    private void Start()
    {
        isLocked = true;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player") /*&& GameManager.Instance.enemiesRemaining <= 0*/)
    //    {
    //        Debug.Log("The box has been hit!");
    //        //This will only access the door in the very very beginning 
    //        if(gameObject.GetComponentInChildren<BoxCollider>().CompareTag("Start Collider"))
    //        {
    //            GameManager.Instance.statePause();
    //            GameManager.Instance.menuActive = GameManager.Instance.menuDungeon;
    //            GameManager.Instance.menuDungeon.SetActive(true);
    //        }
    //        else
    //        {
    //            GameManager.Instance.playerScript.enabled = true;
    //            isLocked = false;
    //            unlockedDoor.gameObject.SetActive(true);

    //            //This is for making the door interactable and letting you walk through by destroying the collider
    //            if (Input.GetButtonDown("Interact") && gameObject.GetComponentInChildren<BoxCollider>().CompareTag("Door Collider"))
    //            {
    //                doorAnimation.Play("DoorOpen", 0, 0.0f);
    //                Destroy(gameObject.GetComponentInChildren<BoxCollider>());
    //            }
    //        }
    //    }
    //    else
    //    {
    //        isLocked = true;
    //        lockedDoor.gameObject.SetActive(true);
    //    }
    //}

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorAnimation.Play("DoorClose", 0, 0.0f);
            //gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
