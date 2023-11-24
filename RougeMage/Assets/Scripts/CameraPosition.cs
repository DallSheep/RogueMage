using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] Vector3 height;

    // Update is called once per frame
    void Update()
    {
        //takes the exact position of the player and adds an offset for height on it
        transform.position = player.transform.position + height;
    }
}
