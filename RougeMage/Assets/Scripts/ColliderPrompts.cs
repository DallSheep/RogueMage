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
            if(gameObject.CompareTag("Block Collider"))
            {
                GameManager.Instance.isPrompt = true;
            }
            else
                GameManager.Instance.isPrompt = false;
            GameManager.Instance.CharPrompts();
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
