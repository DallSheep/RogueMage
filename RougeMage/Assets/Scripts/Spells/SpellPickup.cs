using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellPickup : MonoBehaviour
{
    [SerializeField] SpellStats spell;
    [SerializeField] PlayerController pController;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject button;

    void Start()
    {
        
    }

    //Exists for the purposes of testing without the shop
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("TriggerEnter");
            //pController.setSpellStats(bullet);
            //GameManager.Instance.playerScript.setSpellStats(spell);
            Destroy(gameObject);
            Debug.Log("TriggerExit");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            button.SetActive(false);
        }
    }
}
