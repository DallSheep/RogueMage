using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] Animator anim;
    [SerializeField] Collider damageCol;
    [SerializeField] AudioSource aud;

    [Header("----- Enemy Stats -----")]
    [Range(0, 10)][SerializeField] int HP;
    [Range(1, 20)][SerializeField] int playerFaceSpeed;
    [SerializeField] int viewCone;
    [SerializeField] int shootCone;
    [SerializeField] int roamDist;
    [SerializeField] int roamPauseTime;

    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audAttack;
    [Range(0, 1)][SerializeField] float audAttackVol;
    [SerializeField] AudioClip audDragonAwakens;
    [Range(0, 1)][SerializeField] float audDragonAwakensVol;
    [SerializeField] AudioClip[] audFootSteps;
    [Range(0, 1)][SerializeField] float audFootStepsVol;
    [SerializeField] float timeBetweenSteps;

    [Header("----- Gun Stats -----")]
    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    [Header("----- Sword Stuff -----")]
    [Range(1, 5)][SerializeField] int timeBetweenSwings;

    [Header("----- Drop on Death -----")]
    [SerializeField] List<GameObject> groundItems;

    Vector3 playerDir;
    Vector3 startingPos;
    bool isShooting;
    bool playerInRange;
    bool destinationChosen;
    bool isAttacking;
    public bool isPlayingSteps;
    float angleToPlayer;
    float stoppingDistOrig;

    void Start()
    {
        if(gameObject.CompareTag("Dragon Boss"))
        {
            aud.PlayOneShot(audDragonAwakens, audDragonAwakensVol);
        }
        GameManager.Instance.UpdateGameGoal(1);
        stoppingDistOrig = agent.stoppingDistance;
        startingPos = transform.position;
    }

    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            anim.SetFloat("Speed", agent.velocity.normalized.magnitude);

            if (playerInRange && !canSeePlayer())
            {
                StartCoroutine(roam());
            }
            else if (!playerInRange)
            {
                StartCoroutine(roam());
            }
        }
    }

    IEnumerator roam()
    {
        if (agent.remainingDistance < 0.05f && !destinationChosen)
        {
            destinationChosen = true;
            agent.stoppingDistance = 0;

            yield return new WaitForSeconds(roamPauseTime);

            Vector3 randomPos = Random.insideUnitSphere * roamDist;
            randomPos += startingPos;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, roamDist, 1);
            agent.SetDestination(hit.position);

            destinationChosen = false;
        }
    }

    bool canSeePlayer()
    {
        playerDir = GameManager.Instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);

        //Debug.DrawRay(headPos.position, playerDir);
        //Debug.Log(angleToPlayer);

        RaycastHit hit;

        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                agent.stoppingDistance = stoppingDistOrig;

                if (angleToPlayer <= viewCone)
                {

                    if (angleToPlayer <= shootCone && !isShooting && !isAttacking)
                    {
                        StartCoroutine(attack());
                        StartCoroutine(shoot());
                    }

                    if (agent.remainingDistance < agent.stoppingDistance)
                    {
                        faceTarget();
                    }

                    agent.SetDestination(GameManager.Instance.player.transform.position);

                    return true;
                }

            }
        }

        return false;
    }

    IEnumerator playSteps()
    {
        isPlayingSteps = true;

        aud.PlayOneShot(audFootSteps[Random.Range(0, audFootSteps.Length)], audFootStepsVol);

        yield return new WaitForSeconds(timeBetweenSteps);

        isPlayingSteps = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            agent.stoppingDistance = 0;
            playerInRange = false;
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        anim.SetTrigger("isShooting");

        if (!gameObject.CompareTag("Skeleton Enemy"))
        {
            aud.PlayOneShot(audAttack[Random.Range(0, audAttack.Length)], audAttackVol);
        }
        CreateBullet();

        //using transform.rotation will shoot the bullet wherever the enemy is pointing
        yield return new WaitForSeconds(shootRate);

        isShooting = false;
    }
    public void CreateBullet()
    {
        Instantiate(bullet, shootPos.position, transform.rotation);

    }

    IEnumerator attack()
    {
        isAttacking = true;
        anim.SetBool("isAttacking", true);
        if (gameObject.CompareTag("Skeleton Enemy"))
        {
            aud.PlayOneShot(audAttack[Random.Range(0, audAttack.Length)], audAttackVol);
        }

        yield return new WaitForSeconds(timeBetweenSwings);

        isAttacking = false;
        anim.SetBool("isAttacking", false);
    }

    public void takeDamage(int amount)
    {
        HP -= amount;

        if (HP <= 0)
        {
            damageCol.enabled = false;
            agent.enabled = false;
            GameManager.Instance.UpdateGameGoal(-1);
            Instantiate(groundItems[Random.Range(0, groundItems.Count)], transform.position, transform.rotation);
            anim.SetBool("isDead", true);
        }
        else
        {
            StartCoroutine(flashRed());
            agent.SetDestination(GameManager.Instance.player.transform.position);
        }
    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }
}
