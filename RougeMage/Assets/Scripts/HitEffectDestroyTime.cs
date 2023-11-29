using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectDestroyTime : MonoBehaviour
{
    [Range(0, 3)][SerializeField] float destroyTime;
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
