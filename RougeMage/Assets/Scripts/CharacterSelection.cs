using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] public GameObject fireMage;
    [SerializeField] public GameObject waterMage;
    [SerializeField] public GameObject lightningMage;
    [SerializeField] public GameObject earthMage;

    public bool charSelected;

    private void Start()
    {
        charSelected = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (gameObject.tag)
            {
                case "Fire Mage":
                    GameManager.Instance.selectMage = fireMage;
                    break;
                case "Water Mage":
                    GameManager.Instance.selectMage = waterMage;
                    break;
                case "Lightning Mage":
                    GameManager.Instance.selectMage = lightningMage;
                    break;
                case "Earth Mage":
                    GameManager.Instance.selectMage = earthMage;
                    break;
            }
            GameManager.Instance.CharacterEntry();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetButtonDown("Interact"))
            {
                Debug.Log("pressed");
                GameManager.Instance.ChangeModel();
            }
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.CharacterExit();
        }
    }
}
