using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] AudioSource aud;

    [Header("----- Player Stats -----")]
    [Range(1, 8)][SerializeField] int playerSpeed;
    [Range(1, 4)][SerializeField] int sprintMod;
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

    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audSteps;
    [Range(0, 1)][SerializeField] float audStepsVol;
    [SerializeField] AudioClip[] audDamage;
    [Range(0, 1)][SerializeField] float audDamageVol;
    [SerializeField] AudioClip[] audJump;
    [Range(0, 1)][SerializeField] float audJumpVol;

    private Vector3 move;
    private float horizontalMovement;
    private float verticalMovement;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    //private int jumpedTimes;
    bool isShooting;
    bool isPlayingSteps;
    bool isSprinting;
    int HPOrig;
    int selectedGun;

    private void Start()
    {
        HPOrig = Hp;
        spawnPlayer();
    }

    void Update()
    {
        if (!GameManager.Instance.isPaused)
            movement();

        if (gunList.Count > 0)
        {
            selectGun();

            if (Input.GetButton("Shoot") && !isShooting)
                StartCoroutine(shoot());
        }

    }

    void movement()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);

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

        move = (transform.forward * Input.GetAxisRaw("Vertical") + 
                transform.right * Input.GetAxisRaw("Horizontal"));

        //transform.Translate(move.normalized * playerSpeed * Time.deltaTime, Space.World);

        controller.Move(move * Time.deltaTime * playerSpeed);

        //Disabling the jump. Not sure if we are implementing this or not -Dami

        //if (Input.GetButtonDown("Jump") && jumpedTimes < jumpsMax)
        //{
        //    aud.PlayOneShot(audDamage[Random.Range(0, audDamage.Length)], audDamageVol);
        //    playerVelocity.y = jumpHeight;
        //    jumpedTimes++;
        //}

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
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
        controller.enabled = false;
        Hp = HPOrig;
        updatePlayerUI();
        transform.position = GameManager.Instance.playerSpawnPos.transform.position;
        controller.enabled = true;
    }

    public void updatePlayerUI()
    {
        //GameManager.Instance.playerHPBar.fillAmount = (float)Hp / HPOrig;
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
}