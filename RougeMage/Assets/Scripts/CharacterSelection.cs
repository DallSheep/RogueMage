using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] public GameObject fireMage;
    [SerializeField] public GameObject waterMage;
    [SerializeField] public GameObject lightningMage;
    [SerializeField] public GameObject earthMage;

    public bool isInteractPressed;
    public bool isCharSelection;

    private void Start()
    {
        isInteractPressed = false;
        isCharSelection = true;
    }

    private void Update()
    {
        isInteractPressed = Input.GetButton("Interact");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (gameObject.tag)
            {
                case "Fire Mage":
                    GameManager.Instance.playerScript.selectMage = fireMage;
                    break;
                case "Water Mage":
                    GameManager.Instance.playerScript.selectMage = waterMage;
                    break;
                case "Lightning Mage":
                    GameManager.Instance.playerScript.selectMage = lightningMage;
                    break;
                case "Earth Mage":
                    GameManager.Instance.playerScript.selectMage = earthMage;
                    break;
            }
            GameManager.Instance.interactPrompt.GetComponentInChildren<TMP_Text>().enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isInteractPressed)
            {
                Debug.Log("pressed");
                GameManager.Instance.playerScript.ChangeModel();

                switch (GameManager.Instance.playerScript.selectMage.tag)
                {
                    case "Fire Mage":
                        GameManager.Instance.playerScript.finalMage.tag = fireMage.tag;
                        break;
                    case "Water Mage":
                        GameManager.Instance.playerScript.finalMage.tag = waterMage.tag;
                        break;
                    case "Lightning Mage":
                        GameManager.Instance.playerScript.finalMage.tag = lightningMage.tag;
                        break;
                    case "Earth Mage":
                        GameManager.Instance.playerScript.finalMage.tag = earthMage.tag;
                        break;
                }
            }
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.interactPrompt.GetComponentInChildren<TMP_Text>().enabled = false;
        }
    }
}
