using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Hardware;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] Rigidbody rb;
    [SerializeField] AudioSource aud;
    [SerializeField] Camera cam;
    [SerializeField] GameObject mousePos;
    [SerializeField] public GameObject soulOrb;
    [SerializeField] public GameObject selectMage;
    [SerializeField] public GameObject model;
    [SerializeField] public GameObject root;

    [Header("----- Player Stats -----")]
    [Range(1, 8)][SerializeField] int playerSpeed;
    [Range(1, 4)][SerializeField] int sprintMod;
    [Range(3, 10)][SerializeField] int dashMod;
    [SerializeField] float dashCooldown;
    [Range(8, 30)][SerializeField] float jumpHeight;
    [Range(1, 4)][SerializeField] int jumpsMax;
    [Range(-10, -40)][SerializeField] float gravityValue;
    [SerializeField] int Hp;

    [Header("----- Gun Stats -----")]
    [SerializeField] List<GunStats> gunList = new List<GunStats>();
    [SerializeField] GameObject gunmodel;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;
    [SerializeField] public GameObject weapon;

    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audSteps;
    [Range(0, 1)][SerializeField] float audStepsVol;
    [SerializeField] AudioClip[] audDamage;
    [Range(0, 1)][SerializeField] float audDamageVol;
    [SerializeField] AudioClip[] audJump;
    [Range(0, 1)][SerializeField] float audJumpVol;

    [Header("----- Animation -----")]
    [SerializeField] Animator playerAnim;


    public Vector3 move;
    private float horizontalMovement;
    private float verticalMovement;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    //private int jumpedTimes;
    bool isShooting;
    bool isPlayingSteps;
    bool isSprinting;
    bool isDashing;
    int HPOrig;
    int selectedGun;

    Vector3 newPlayerY;



    private void Start()
    {
        HPOrig = Hp;
        spawnPlayer();    }

    void Update()
    {
        if (!GameManager.Instance.isPaused)
        {
            movement();
            cameraMovement();
        }

        if (gunList.Count > 0)
        {
            selectGun();

            if (Input.GetButton("Shoot") && !isShooting)
                StartCoroutine(shoot());
        }
        newPlayerY = new Vector3(transform.position.x, 0, transform.position.z);
    }

    void movement()
    {
        //Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);

        sprint();

        if(!isDashing)
        {
            StartCoroutine(Dash());
        }

        if (groundedPlayer && move.normalized.magnitude > 0.3 && !isPlayingSteps)
        {
            StartCoroutine(playSteps());
        }

        groundedPlayer = controller.isGrounded;

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            //jumpedTimes = 0;
        }

        move.z = Input.GetAxis("Vertical");
        move.x = Input.GetAxis("Horizontal");

        controller.Move(move * Time.deltaTime * playerSpeed);

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        Debug.Log(move);

        if (move.x > 0 || move.z > 0 || move.x < 0 || move.z < 0)
        {
            Debug.Log(playerAnim.GetBool("isMoving"));
            playerAnim.SetBool("isMoving", true);
        }
        else
        {
            Debug.Log(playerAnim.GetBool("isMoving"));
            playerAnim.SetBool("isMoving", false);
        }
    }
 

    void cameraMovement()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;


        //finds vector between the player and the mouse position
        Vector3 relative =  mousePos.transform.position - transform.forward;
        //looks at mouse position
        transform.LookAt(relative, Vector3.up);

        
    }

    IEnumerator Dash()
    {
        //using 'Jump' because is is already bound to space
        if (Input.GetButtonDown("Jump"))
        {
            isDashing = true;
            //tracking original speed
            int ps = playerSpeed;
            playerSpeed *= dashMod;

            yield return new WaitForSeconds(0.2f);
            playerSpeed = ps;

            yield return new WaitForSeconds(dashCooldown);
            isDashing = false;
        }
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            isSprinting = true;
            playerSpeed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            isSprinting = false;
            playerSpeed /= sprintMod;
        }
    }

    IEnumerator playSteps()
    {

        isPlayingSteps = true;

        if (!isSprinting)
            yield return new WaitForSeconds(0.5f);
        else if (isSprinting)
            yield return new WaitForSeconds(0.3f);
        isPlayingSteps = false;
    }

    IEnumerator shoot()
    {
        if (gunList[selectedGun].ammoCurr > 0)
        {
            isShooting = true;

            gunList[selectedGun].ammoCurr--;

            aud.PlayOneShot(gunList[selectedGun].shootSound, gunList[selectedGun].shootSoundVol);


            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
            {

                Instantiate(gunList[selectedGun].hitEffect, hit.point, gunList[selectedGun].hitEffect.transform.rotation);
                IDamage damagable = hit.collider.GetComponent<IDamage>();

                if (hit.transform != transform && damagable != null)
                {
                    damagable.takeDamage(shootDamage);
                }
            }
            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
    }
    public void takeDamage(int amount)
    {
        Hp -= amount;
        updatePlayerUI();
        aud.PlayOneShot(audDamage[Random.Range(0, audDamage.Length)], audDamageVol);
        StartCoroutine(GameManager.Instance.playerFlashDamage());

        if (Hp <= 0)
        {
            GameManager.Instance.youLose();
        }
    }

    public void spawnPlayer()
    {
        //controller.enabled = false;
        Hp = HPOrig;
        updatePlayerUI();
        transform.position = GameManager.Instance.playerSpawnPos.transform.position;
        controller.enabled = true;
    }

    public void updatePlayerUI()
    {
        GameManager.Instance.playerHPBar.fillAmount = (float)Hp / HPOrig;
    }

    void selectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunList.Count - 1)
        {
            selectedGun++;
            changeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
        {
            selectedGun--;
            changeGun();
        }
    }

    void changeGun()
    {
        shootDamage = gunList[selectedGun].shootDamage;
        shootDist = gunList[selectedGun].shootDist;
        shootRate = gunList[selectedGun].shootRate;


        gunmodel.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGun].model.GetComponent<MeshFilter>().sharedMesh;
        gunmodel.GetComponent<MeshRenderer>().sharedMaterial = gunList[selectedGun].model.GetComponent<MeshRenderer>().sharedMaterial;
        isShooting = false;
    }

    public void getGunStats(GunStats gun)
    {
        gunList.Add(gun);

        shootDamage = gun.shootDamage;
        shootDist = gun.shootDist;
        shootRate = gun.shootRate;


        gunmodel.GetComponent<MeshFilter>().sharedMesh = gun.model.GetComponent<MeshFilter>().sharedMesh;
        gunmodel.GetComponent<MeshRenderer>().sharedMaterial = gun.model.GetComponent<MeshRenderer>().sharedMaterial;

        selectedGun = gunList.Count - 1;
    }

    public void ChangeModel()
    {
        //GameObject thisModel = Instantiate(selectMage, newPlayerY, transform.rotation);
        //thisModel.GetComponent<SphereCollider>().enabled = false;
        //thisModel.transform.parent = transform;
        //selectMage = thisModel;
        

        soulOrb.SetActive(false);
        root.SetActive(true);
        model.SetActive(true);

         model.GetComponent<SkinnedMeshRenderer>().rootBone = 
            selectMage.GetComponentInChildren<SkinnedMeshRenderer>().rootBone;

        model.GetComponent<SkinnedMeshRenderer>().sharedMesh =
            selectMage.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;

        model.GetComponent<SkinnedMeshRenderer>().material =
            selectMage.GetComponentInChildren<SkinnedMeshRenderer>().material;
        
        transform.position = newPlayerY;

    }
}