using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderPrompts : MonoBehaviour
{
   

    private void Start()
    {
   
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(gameObject.GetComponent<GameObject>().CompareTag("Block Collider"))
            {
                GameManager.Instance.blockedTrigger.SetActive(true);
            }
            else
            {
                GameManager.Instance.charTrigger.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.blockedTrigger.SetActive(false);
            GameManager.Instance.charTrigger.SetActive(false);
        }
    }
}
