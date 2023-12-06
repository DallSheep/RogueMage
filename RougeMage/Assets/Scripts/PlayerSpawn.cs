using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public bool isSpawned = false;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance && isSpawned == false)
            if (GameManager.Instance.playerScript)
            {
                isSpawned = true;
                GameManager.Instance.playerScript.spawnPlayer();
            }
    }
}
