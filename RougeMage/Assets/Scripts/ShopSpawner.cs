using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSpawner : MonoBehaviour
{
    [SerializeField] GameObject shopMage;
    [SerializeField] GameObject shopInScene;
    [SerializeField] Transform[] spawnPos;

    int randomNum;

    private void Start()
    {
        shopInScene = null;
        randomNum = UnityEngine.Random.Range(0, spawnPos.Length);
    }

    private void Update()
    {
        if (GameManager.Instance.enemiesRemaining != 0)
        {
            shopMage.SetActive(false);
        }
        else
            shopMage.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && shopInScene == null)
        {
            Instantiate(shopMage, spawnPos[randomNum].position, spawnPos[randomNum].rotation);
            shopInScene = shopMage;
        }
    }
}
