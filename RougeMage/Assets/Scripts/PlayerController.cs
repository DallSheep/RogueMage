using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;

    [Header("----- Player Stats -----")]
    [Range(1, 10)] public int HP;
    [Range(2, 8)][SerializeField] float playerSpeed;
    [Range(8, 30)][SerializeField] float jumpHeight;
    [Range(3, 4)][SerializeField] int sprintMod;
    [Range(1, 4)][SerializeField] int jumpsMax;
    [Range(-10, -40)][SerializeField] float gravityValue;

    [Header("----- Gun Stats -----")]
    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate; //Changed to float so we can have faster gunfire - Dami
    [SerializeField] int reloadTime;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject bullet2;

    private Vector3 move;
    private Vector3 playerVelocity;
    bool isPaused;
    bool isShooting;
    bool isPlayingSteps;
    public bool isSprinting;
    private bool groundedPlayer;
    private int jumpTimes;
    public int HPOrig;
    

    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;
        SpawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.isPaused)
        {
            Movement();
        }
    }

    public void takeDamage(int amount)
    {
        
    }

    private void SpawnPlayer()
    {
       
    }
    void Movement()
    {

        if (Input.GetButtonDown("Sprint"))
        {
            Debug.Log(playerSpeed);
            Debug.Log(sprintMod);
            isSprinting = true;
            playerSpeed *= sprintMod;
            Debug.Log(playerSpeed);
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            Debug.Log(playerSpeed);
            Debug.Log(sprintMod);
            isSprinting = false;
            playerSpeed /= sprintMod;
            Debug.Log(playerSpeed);
        }


        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist);

        if (Input.GetButton("Shoot") && !isShooting)
        {
            //StartCoroutine(shoot());
        }

        groundedPlayer = controller.isGrounded;

        if (groundedPlayer && move.normalized.magnitude > 0.3f && !isPlayingSteps)
        {
            //StartCoroutine(playSteps());
        }

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            jumpTimes = 0;
        }

        move = Input.GetAxis("Horizontal") * transform.right +
               Input.GetAxis("Vertical") * transform.forward;
        controller.Move(move * Time.deltaTime * playerSpeed);


        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && jumpTimes < jumpsMax)
        {
            //aud.PlayOneShot(audJump[Random.Range(0, audJump.Length)], audJumpVol);
            playerVelocity.y = jumpHeight;
            jumpTimes++;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
