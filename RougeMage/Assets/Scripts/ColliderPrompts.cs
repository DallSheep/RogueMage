using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class ColliderPrompts : MonoBehaviour
{
    [SerializeField] public Animator shopAnimation;

    public bool isEPressed;

    private void Start()
    {
   
    }

    private void Update()
    {
        isEPressed = Input.GetButton("Interact");
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
                    GameManager.Instance.statePause();
                    break;
                case "Shop Collider":
                    shopAnimation.SetBool("isPlayer", true);
                    break;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.tag == "Shop Collider")
            {
                GameManager.Instance.interactPrompt.GetComponentInChildren<TMP_Text>().enabled = true;

                if (isEPressed)
                {
                    GameManager.Instance.ShopScreen();
                }
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
                    GameManager.Instance.dungeonPrompt.GetComponent<Image>().enabled = false;
                    GameManager.Instance.buttonYes.GetComponent<Image>().enabled = false;
                    GameManager.Instance.buttonYes.GetComponentInChildren<TMP_Text>().enabled = false;
                    GameManager.Instance.buttonNo.GetComponent<Image>().enabled = false;
                    GameManager.Instance.buttonNo.GetComponentInChildren<TMP_Text>().enabled = false;
                    break;
                case "Shop Collider":
                    shopAnimation.SetBool("isPlayer", false);
                    GameManager.Instance.interactPrompt.GetComponentInChildren<TMP_Text>().enabled = false;
                    break;
            }
        }
    }
}
