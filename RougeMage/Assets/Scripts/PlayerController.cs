using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] Rigidbody rb;
    [SerializeField] AudioSource aud;
    [SerializeField] Camera cam;
    [SerializeField] GameObject mousePos;
    [SerializeField] public Transform shootPos;
    [SerializeField] public GameObject soulOrb;
    [SerializeField] public GameObject selectMage;
    [SerializeField] public GameObject finalMage;
    [SerializeField] public GameObject model;
    [SerializeField] public GameObject root;

    [Header("----- Player Stats -----")]
    [Range(1, 10)][SerializeField] int playerSpeed;
    [Range(1, 4)][SerializeField] int sprintMod;
    [Range(3, 10)][SerializeField] int dashMod;
    [SerializeField] float dashCooldown;
    [Range(8, 30)][SerializeField] float jumpHeight;
    [Range(1, 4)][SerializeField] int jumpsMax;
    [Range(-10, -40)][SerializeField] float gravityValue;
    [SerializeField] public int Hp;
    [SerializeField] public float maxMana;
    [SerializeField] public float currMana; //Used just to see current mana, remove when done
    [SerializeField] public int stamina;
    [SerializeField] public int gold;
    [Range(0,5)] [SerializeField] float manaRegenSpeed;
    public bool isCharSlected;

    [Header("----- Spell Stats -----")]
    [SerializeField] List<SpellStats> SpellList = new List<SpellStats>();
    [SerializeField] SpellStats defaultSpell;

    [SerializeField] GameObject bulletMain;
    [SerializeField] GameObject bulletFire;
    [SerializeField] GameObject bulletWater;
    [SerializeField] GameObject bulletLightning;
    [SerializeField] GameObject bulletEarth;
    [SerializeField] int shootDamage;
    [SerializeField] float shootDist;
    [SerializeField] float shootRate;
    [SerializeField] float manaCost;
    [SerializeField] float cooldown;
    [SerializeField] public GameObject weapon;

    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audSteps;
    [Range(0, 1)][SerializeField] float audStepsVol;
    [SerializeField] AudioClip[] audDamage;
    [Range(0, 1)][SerializeField] float audDamageVol;
    [SerializeField] AudioClip[] audJump;
    [Range(0, 1)][SerializeField] float audJumpVol;

    [Header("----- Animation -----")]
    [SerializeField] public Animator playerAnim;


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
    bool manaRegen;
    private float shootRateOrig;
    private Camera camOrig;
    public int HPOrig;
    public float manaOrig;
    public int staminaOrig;
    int selectedGun;

    public int isStarted;

    Vector3 newPlayerY;

    private void Start()
    {
        DontDestroyOnLoad(mousePos);
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(cam);
        

        //sets mana to full from start
        currMana = maxMana;
        manaOrig = currMana;
        

        //isCharSlected = false;
        staminaOrig = stamina;
        //goldOrig = gold;
        HPOrig = Hp;
        staminaOrig = stamina;
        //setSpellStats(defaultSpell);
        shootRateOrig = shootRate;
        isStarted = 1;

        if (GameManager.Instance.playerSpawnPos != null)
        {
            spawnPlayer();
        }
    }

    void Update()
    {
        if (!GameManager.Instance.isPaused)
        {
            if (SceneManager.GetActiveScene().name != "Main Menu" && isStarted == 1)
            {
                spawnPlayer();
                isStarted = 0;
            }

            GameManager.Instance.playerSpawnPos = GameObject.FindWithTag("Respawn");

            transform.position = GameManager.Instance.playerSpawnPos.transform.position;

            movement();
            cameraMovement();

            if(Input.GetButton("Fire2") && !isShooting)
            {
                
                StartCoroutine(specialAttack());
            }

            if (Input.GetButton("Shoot") && !isShooting && isCharSlected)
            {
                StartCoroutine(baseAttack());
            }

            updatePlayerManaUI();
            updatePlayerHealthUI();
            //Regens mana when mana is not full
            if ((currMana < maxMana) && !manaRegen)
            {
                StartCoroutine(regenMana());
            }
        }

        
        newPlayerY = new Vector3(transform.position.x, 0, transform.position.z);
    }

    void movement()
    {
        //Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);

        if (stamina >= 100 && !isDashing)
        {
            Dash();
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

        if (move.x > 0 || move.z > 0 || move.x < 0 || move.z < 0)
        {
            playerAnim.SetBool("isMoving", true);
        }
        else
        {
            playerAnim.SetBool("isMoving", false);
        }
    }
 

    void cameraMovement()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;


        //finds vector between the player and the mouse position
        Vector3 relative =  mousePos.transform.position;

        relative.y = transform.position.y;
        //looks at mouse position
        transform.LookAt(relative);

        
    }

    IEnumerator regenMana()
    {
        manaRegen = true;
        //Lower the faster regen speed
        yield return new WaitForSeconds(manaRegenSpeed);

        currMana++;

        manaRegen = false;
    }

    IEnumerator specialAttack()
    {
        isShooting = true;
        playerAnim.SetBool("isAttacking", true);

        if (currMana >= manaCost)
        {
            currMana -= manaCost;
            Ray ray = new Ray(transform.position, transform.forward);
            Vector3 targetPoint;

            targetPoint = ray.GetPoint(50);

            Vector3 shootDir = targetPoint - shootPos.position;

            if (bulletMain != null)
            {
                GameObject currBullet = Instantiate(bulletMain, shootPos.position, Quaternion.identity);
                currBullet.transform.forward = shootDir.normalized;
            }

            yield return new WaitForSeconds(cooldown);
        }
        isShooting = false;
        playerAnim.SetBool("isAttacking", false);
    }

    IEnumerator baseAttack()
    {
        isShooting = true;
        playerAnim.SetBool("isAttacking", true);

        switch (finalMage.tag)
        {
            case "Fire Mage":
                bulletMain = bulletFire;
                break;
            case "Water Mage":
                bulletMain = bulletWater;
                break;
            case "Lightning Mage":
                bulletMain = bulletLightning;
                break;
            case "Earth Mage":
                bulletMain = bulletEarth;
                break;
        }

        if (currMana >= manaCost)
        {
            currMana -= manaCost;
            Ray ray = new Ray(transform.position, transform.forward);
            Vector3 targetPoint;

            targetPoint = ray.GetPoint(50);

            Vector3 shootDir = targetPoint - shootPos.position;

            if (bulletMain != null)
            {
                GameObject currBullet = Instantiate(bulletMain, shootPos.position, Quaternion.identity);
                currBullet.transform.forward = shootDir.normalized;
            }

            yield return new WaitForSeconds(cooldown);
        }
        isShooting = false;
        playerAnim.SetBool("isAttacking", false);
    }


    IEnumerator Dash()
    {
        if (stamina > 0)
        {
            if (Input.GetButtonDown("Sprint"))
            {
                isDashing = true;
                //tracking original speed
                int ps = playerSpeed;
                playerSpeed *= dashMod;
                stamina = stamina - 100;

                yield return new WaitForSeconds(0.2f);
                playerSpeed = ps;

                yield return new WaitForSeconds(dashCooldown);
                isDashing = false;
            }
        }
    }

    //void sprint()
    //{
    //    if (Input.GetButtonDown("Sprint"))
    //    {
    //        isSprinting = true;
    //        playerSpeed *= sprintMod;
    //    }
    //    else if (Input.GetButtonUp("Sprint"))
    //    {
    //        isSprinting = false;
    //        playerSpeed /= sprintMod;
    //    }
    //}

    IEnumerator playSteps()
    {

        isPlayingSteps = true;

        if (!isSprinting)
            yield return new WaitForSeconds(0.5f);
        else if (isSprinting)
            yield return new WaitForSeconds(0.3f);
        isPlayingSteps = false;
    }

    public void setSpellStats(SpellStats spell)
    {
        SpellList.Add(spell);

        bulletMain = spell.bullet;
        bulletMain.GetComponent<Bullet>().damage = spell.damage;
        bulletMain.GetComponent<Bullet>().SetDestroyTime(spell.distance);
        bulletMain.GetComponent<Bullet>().setHitEffect(spell.hitEffect);
        manaCost = spell.manaCost;
        cooldown = spell.cooldown;

    }

    public void takeDamage(int amount)
    {
        playerAnim.SetBool("isHit", true);
        Hp -= amount;
        updatePlayerHealthUI();
        //aud.PlayOneShot(audDamage[Random.Range(0, audDamage.Length)], audDamageVol);
        //StartCoroutine(GameManager.Instance.playerFlashDamage());

        if (Hp <= 0)
        {
            playerAnim.SetBool("isDead", true);
            GameManager.Instance.youLose();
        }
        playerAnim.SetBool("isHit", false);
    }

    public void spawnPlayer()
    {
        //controller.enabled = false;
        Hp = HPOrig;
        currMana = manaOrig;
        stamina = staminaOrig;
        Debug.Log(gold);
        updatePlayerHealthUI();
        updatePlayerManaUI();
        updatePlayerStaminaUI();
        //updatePlayerGoldUI(0);
        camOrig = cam;
        Debug.Log("in spawnPlayer");
        controller.enabled = false;
        Debug.Log("in spawnPlayer");
        transform.position = GameManager.Instance.playerSpawnPos.transform.position;
        Debug.Log("in spawnPlayer");
        controller.enabled = true;
        Debug.Log("out spawnPlayer");
        Time.timeScale = 1;
        Debug.Log(GameManager.Instance.timescaleOrig);
    }

    public void updatePlayerHealthUI()
    {
        GameManager.Instance.playerHPBar.fillAmount = (float)Hp / HPOrig;
    }

    public void updatePlayerManaUI()
    {
        GameManager.Instance.playerManaBar.fillAmount = (float)currMana / manaOrig;
    }

    public void updatePlayerStaminaUI()
    {
        GameManager.Instance.playerStaminaBar.fillAmount = (float)stamina / staminaOrig;
    }


    public void updatePlayerGoldUI(int amount)
    {
        gold += amount;
        Debug.Log(gold);
        GameManager.Instance.goldCount.GetComponent<TMP_Text>().text = gold.ToString("0");
        Debug.Log(GameManager.Instance.goldCount.GetComponentInChildren<TMP_Text>().text);
    }


    public void ChangeModel()
    {
        //GameObject thisModel = Instantiate(selectMage, newPlayerY, transform.rotation);
        //thisModel.GetComponent<SphereCollider>().enabled = false;
        //thisModel.transform.parent = transform;
        //selectMage = thisModel;

        isCharSlected = true;


        soulOrb.SetActive(false);
        root.SetActive(true);
        model.SetActive(true);

        root.GetComponentInChildren<MeshRenderer>().material = selectMage.GetComponentInChildren<MeshRenderer>().material;

         model.GetComponent<SkinnedMeshRenderer>().rootBone = 
            selectMage.GetComponentInChildren<SkinnedMeshRenderer>().rootBone;

        model.GetComponent<SkinnedMeshRenderer>().sharedMesh =
            selectMage.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;

        model.GetComponent<SkinnedMeshRenderer>().material =
            selectMage.GetComponentInChildren<SkinnedMeshRenderer>().material;
        
        transform.position = newPlayerY;
        Debug.Log(isCharSlected);
        if(isCharSlected)
        {
            GameManager.Instance.blockedTrigger.SetActive(false);
            GameManager.Instance.charTrigger.SetActive(false);
            GameManager.Instance.blockedWall.SetActive(false);
            GameManager.Instance.oldMage.SetActive(false);
        }
    }
}