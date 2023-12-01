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
    [SerializeField] Transform shootPos;
    [SerializeField] public GameObject soulOrb;
    [SerializeField] public GameObject selectMage;
    [SerializeField] public GameObject finalMage;
    [SerializeField] public GameObject model;
    [SerializeField] public GameObject root;
    [SerializeField] TMP_Text goldCounter;

    [Header("----- Player Stats -----")]
    [Range(1, 8)][SerializeField] int playerSpeed;
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

    [SerializeField] GameObject bullet;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
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
    bool manaRegen;
    private float shootRateOrig;
    private Camera camOrig;
    public int HPOrig;
    public float manaOrig;
    public int staminaOrig;
    int selectedGun;

    int isStarted;

    Vector3 newPlayerY;



    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(mousePos);
        DontDestroyOnLoad(cam);
        //sets mana to full from start
        currMana = maxMana;
        manaOrig = currMana;
        

        isCharSlected = false;
        manaOrig = mana;
        staminaOrig = stamina;
        //goldOrig = gold;
        HPOrig = Hp;
        staminaOrig = stamina;
        setSpellStats(defaultSpell);
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
            if (SceneManager.GetActiveScene().name == "Dungeon_Scene" && isStarted == 1)
            {
                spawnPlayer();
                isStarted = 0;
            }
            movement();
            cameraMovement();

            if(Input.GetButton("Fire2") && !isShooting)
            {
                StartCoroutine(specialAttack());
            }

            //updatePlayerManaUI();
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

        if (currMana >= manaCost)
        {
            currMana -= manaCost;
            Ray ray = new Ray(transform.position, transform.forward);
            Vector3 targetPoint;

            targetPoint = ray.GetPoint(50);

            Vector3 shootDir = targetPoint - shootPos.position;

            if (bullet != null)
            {
                GameObject currBullet = Instantiate(bullet, shootPos.position, Quaternion.identity);
                currBullet.transform.forward = shootDir.normalized;
            }

            yield return new WaitForSeconds(cooldown);
        }
        isShooting = false;
    }


    IEnumerator Dash()
    {
        if (stamina > 0)
        {
            //using 'Jump' because is is already bound to space
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

        bullet = spell.bullet;
        bullet.GetComponent<Bullet>().damage = spell.damage;
        bullet.GetComponent<Bullet>().SetDestroyTime(spell.distance);
        bullet.GetComponent<Bullet>().setHitEffect(spell.hitEffect);
        manaCost = spell.manaCost;
        cooldown = spell.cooldown;

    }

    public void takeDamage(int amount)
    {
        Hp -= amount;
        updatePlayerHealthUI();
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
        currMana = manaOrig;
        stamina = staminaOrig;
        Debug.Log(gold);
        updatePlayerHealthUI();
        updatePlayerManaUI();
        updatePlayerStaminaUI();
        updatePlayerGoldUI(0);
        camOrig = cam;
        controller.enabled = false;
        transform.position = GameManager.Instance.playerSpawnPos.transform.position;
        controller.enabled = true;
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
        goldCounter.text = gold.ToString("0");
        Debug.Log(goldCounter);
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
        }
    }
}