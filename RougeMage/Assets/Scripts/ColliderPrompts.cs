using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColliderPrompts : MonoBehaviour
{
   

    private void Start()
    {
   
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Got Here");

            switch (gameObject.tag)
            {
                case "Block Collider":
                    Debug.Log(gameObject.tag);
                    Debug.Log("Blocked Wall");
                    GameManager.Instance.blockedPrompt.GetComponentInChildren<TMP_Text>().enabled = true;
                    break;
                case "Character Trigger":
                    Debug.Log(gameObject.tag);
                    Debug.Log("Characters");
                    GameManager.Instance.charSelect.GetComponentInChildren<TMP_Text>().enabled = true;
                    break;
                case "Door Collider":
                    Debug.Log(gameObject.tag);
                    GameManager.Instance.dungeonPrompt.GetComponentInChildren<TMP_Text>().enabled = true;
                    GameManager.Instance.dungeonPrompt.GetComponent<Image>().enabled = true;
                    GameManager.Instance.buttonYes.GetComponent<Image>().enabled = true;
                    GameManager.Instance.buttonYes.GetComponentInChildren<TMP_Text>().enabled = true;
                    GameManager.Instance.buttonNo.GetComponent<Image>().enabled = true;
                    GameManager.Instance.buttonNo.GetComponentInChildren<TMP_Text>().enabled = true;
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (gameObject.tag)
            {
                case "Block Collider":
                    Debug.Log(gameObject.tag);
                    Debug.Log("Blocked Wall");
                    GameManager.Instance.blockedPrompt.GetComponentInChildren<TMP_Text>().enabled = false;
                    break;
                case "Character Trigger":
                    Debug.Log(gameObject.tag);
                    Debug.Log("Characters");
                    GameManager.Instance.charSelect.GetComponentInChildren<TMP_Text>().enabled = false;
                    break;
                case "Door Collider":
                    Debug.Log(gameObject.tag);
                    Debug.Log("Dungeon");
                    GameManager.Instance.dungeonPrompt.GetComponentInChildren<TMP_Text>().enabled = false;
                    GameManager.Instance.dungeonPrompt.GetComponentInChildren<Image>().enabled = false;
                    GameManager.Instance.dungeonPrompt.GetComponent<Image>().enabled = false;
                    GameManager.Instance.buttonYes.GetComponentInChildren<TMP_Text>().enabled = false;
                    GameManager.Instance.buttonNo.GetComponentInChildren<TMP_Text>().enabled = false;
                    break;
            }
        }
    }
}
