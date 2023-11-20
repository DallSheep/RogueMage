using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance;

    [Header("-----player-----")]
    public GameObject player;
    public GameObject playerSpawnPos;
    public PlayerController playerScript;

    public bool isPaused;
    float timescaleOrig;

    public void Awake()
    {
        Instance = this;
        timescaleOrig = Time.timeScale;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
    }

    public void Start()
    {
        
    }

    public void Update()
    {
        
    }
}
