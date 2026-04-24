using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Patrol, Chase, Jumpscare }
    public EnemyState currentState;

    private NavMeshAgent agent;
    private Animator animator;
    private Transform player;

    [Header("Patrol Points")]
    public Transform[] patrolPoints;
    private int patrolIndex;

    [Header("Detection")]
    public float hearingRadius = 10f;
    public float viewAngle = 60f;
    public LayerMask obstacleMask;

    [Header("Movement")]
    public float patrolSpeed = 1.5f;
    public float chaseSpeed = 3.5f;
    public float patrolPause = 0.5f;

    [Header("Chase")]
    public float chaseTime = 6f;
    public AudioClip scream;
    private float chaseTimer;
    public float chaseDetectionRadius = 5f;

    [Header("Lurk")]
    public float lurkInterval = 4f;
    public float lurkChance = 0.35f;
    private float lurkTimer;

    [Header("Jumpscare")]
    public float jumpscareDistance = 1.5f;
    public float jumpscareDuration = 2.3f;
    public Transform respawnPoint;
    public Transform enemyResetPoint;
    private Inventory playerInventory;
    private BookShelf[] allShelves;

    public Camera jumpscareCamera;
    private Camera playerCamera;

    public Animator jumpscareAnimator;
    public AudioSource jumpscareSource;
    public AudioClip jumpscareClip;

    private bool isBusy;
    private bool isJumpscareActive;
    private bool canJumpscare = true;

    private float decisionTimer = 15f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCamera = Camera.main;

        playerInventory = player.GetComponent<Inventory>();
        allShelves = FindObjectsByType<BookShelf>(FindObjectsSortMode.None);

        agent.autoBraking = true;
        agent.stoppingDistance = 1.2f;
        agent.angularSpeed = 240f;
        agent.acceleration = 8f;

        currentState = EnemyState.Patrol;

        StartCoroutine(PatrolLoop());
    }

    void Awake()
    {
        if (jumpscareCamera)
            jumpscareCamera.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isJumpscareActive) return;
        if (isBusy) return;

        if (currentState == EnemyState.Patrol)
        {
            DetectPlayer();
            HandleLurk();
            HandlePatrolSwitch();
        }
        else if (currentState == EnemyState.Chase)
        {
            Chase();
        }
    }

    // ---------------- PATROL ----------------

    IEnumerator PatrolLoop()
    {
        while (true)
        {
            if (currentState != EnemyState.Patrol || isBusy)
            {
                yield return null;
                continue;
            }

            Vector3 target = GetPatrolTarget();

            agent.speed = patrolSpeed;
            agent.SetDestination(target);

            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);

            while (currentState == EnemyState.Patrol && !isBusy)
            {
                if (!agent.pathPending &&
                    agent.remainingDistance <= agent.stoppingDistance + 0.2f)
                    break;

                yield return null;
            }

            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;

            yield return new WaitForSeconds(patrolPause);
        }
    }

    Vector3 GetPatrolTarget()
    {
        Vector3 basePoint = patrolPoints[patrolIndex].position;
        Vector2 offset = Random.insideUnitCircle * 2f;
        Vector3 candidate = basePoint + new Vector3(offset.x, 0, offset.y);

        if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            return hit.position;

        return basePoint;
    }

    void HandlePatrolSwitch()
    {
        decisionTimer -= Time.deltaTime;
        if (decisionTimer > 0f) return;

        decisionTimer = 15f;

        if (Random.value < 0.3f)
            SwitchPatrolPoint();
    }

    void SwitchPatrolPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return;

        float roll = Random.value;

        Vector3 target;

        // ---------------- 60% NORMAL PATROL ----------------
        if (roll < 0.6f)
        {
            int newIndex = patrolIndex;

            for (int i = 0; i < 5; i++)
            {
                newIndex = Random.Range(0, patrolPoints.Length);
                if (newIndex != patrolIndex) break;
            }

            patrolIndex = newIndex;
            target = GetPatrolTarget();
        }
        else
        {
            // ---------------- 40% SPECIAL BEHAVIOR ----------------
            float subRoll = Random.value;

            if (subRoll < 0.85f)
            {
                // 60% of 40% investigate player area
                target = GetInvestigationPoint();
            }
            else
            {
                // 40% of 40%  just pick another patrol point
                int newIndex = Random.Range(0, patrolPoints.Length);
                patrolIndex = newIndex;
                target = GetPatrolTarget();
            }
        }

        agent.ResetPath();
        agent.SetDestination(target);
    }

    Vector3 GetInvestigationPoint()
    {
        // small random offset around player so it feels like searching, not locking on
        Vector2 offset = Random.insideUnitCircle * 3f;
        Vector3 candidate = player.position + new Vector3(offset.x, 0, offset.y);

        if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            return hit.position;

        return player.position;
    }

    // ---------------- DETECTION ----------------

    void DetectPlayer()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist > hearingRadius) return;

        Vector3 dir = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, dir);

        bool inVision =
            angle < viewAngle / 2f &&
            !Physics.Raycast(transform.position + Vector3.up, dir, dist, obstacleMask);

        bool closeOverride = dist < chaseDetectionRadius;

        if (inVision || closeOverride)
        {
            StartCoroutine(ScreamThenChase());
        }
    }

    IEnumerator ScreamThenChase()
    {
        if (isBusy) yield break;

        isBusy = true;

        agent.isStopped = true;
        agent.ResetPath();

        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);

        animator.Play("screamFix", 0, 0f);

        AudioSource.PlayClipAtPoint(scream, transform.position, 1f);

        yield return new WaitForSeconds(1f);

        StartChase();

        isBusy = false;
    }

    // ---------------- LURK ----------------

    void HandleLurk()
    {
        if (isBusy) return;

        lurkTimer -= Time.deltaTime;
        if (lurkTimer > 0f) return;

        lurkTimer = lurkInterval;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist < chaseDetectionRadius) return;

        if (Random.value < lurkChance)
        {
            StartCoroutine(LurkCoroutine());
        }
    }

    IEnumerator LurkCoroutine()
    {
        if (isBusy) yield break;

        isBusy = true;

        agent.isStopped = true;
        agent.ResetPath();

        animator.SetBool("isRunning", false);
        animator.SetBool("isWalking", false);

        animator.Play("searchingFIx", 0, 0f);

        yield return new WaitForSeconds(8f);

        isBusy = false;
    }

    // ---------------- CHASE ----------------

    void StartChase()
    {
        currentState = EnemyState.Chase;
        chaseTimer = chaseTime;

        agent.speed = chaseSpeed;

        animator.SetBool("isRunning", true);
        animator.SetBool("isWalking", false);
    }

    void Chase()
    {
        agent.SetDestination(player.position);

        chaseTimer -= Time.deltaTime;

        if (canJumpscare &&
            Vector3.Distance(transform.position, player.position) <= jumpscareDistance)
        {
            StartCoroutine(JumpscareRoutine());
            return;
        }

        if (chaseTimer <= 0f && !CanSeePlayer())
        {
            ReturnToPatrol();
        }
    }

    // ---------------- JUMPSCARE ----------------

    IEnumerator JumpscareRoutine()
    {
        if (isJumpscareActive) yield break;

        isJumpscareActive = true;

        agent.isStopped = true;
        agent.ResetPath();

        if (jumpscareAnimator != null)
            jumpscareAnimator.Play("JumpScare 1", 0, 0f);

        if (jumpscareSource && jumpscareClip)
            jumpscareSource.PlayOneShot(jumpscareClip);

        if (playerCamera != null)
            playerCamera.gameObject.SetActive(false);

        if (jumpscareCamera != null)
            jumpscareCamera.gameObject.SetActive(true);

        yield return new WaitForSeconds(jumpscareDuration);

        // ---------------- SAFE GAME LOGIC ----------------
        if (player != null && respawnPoint != null)
            player.position = respawnPoint.position;

        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            Transform randomPoint = patrolPoints[Random.Range(0, patrolPoints.Length)];

            if (randomPoint != null &&
                NavMesh.SamplePosition(randomPoint.position, out NavMeshHit hit, 3f, NavMesh.AllAreas))
            {
                agent.Warp(hit.position);
                patrolIndex = System.Array.IndexOf(patrolPoints, randomPoint);
            }
        }

        // ---------------- INVENTORY (SAFE, NO BREAKS) ----------------
        if (playerInventory != null)
        {
            ItemData heldItem = playerInventory.GetSelectedItem();

            if (heldItem != null)
            {
                Debug.Log("BOOK FOUND ON DEATH: " + heldItem.name);

                playerInventory.RemoveSelectedItem();

                if (allShelves != null)
                {
                    foreach (BookShelf shelf in allShelves)
                    {
                        if (shelf == null) continue;

                        if (shelf.GetStartingBook() == heldItem)
                        {
                            Debug.Log("RETURNING BOOK TO SHELF: " + heldItem.name);
                            shelf.ResetIfMatches(heldItem);
                            break;
                        }
                    }
                }
            }
        }

        // ---------------- ALWAYS RUN (CRITICAL) ----------------
        if (jumpscareCamera != null)
            jumpscareCamera.gameObject.SetActive(false);

        if (playerCamera != null)
            playerCamera.gameObject.SetActive(true);

        currentState = EnemyState.Patrol;
        agent.isStopped = false;

        isBusy = false;
        isJumpscareActive = false;
    }

    void ReturnToPatrol()
    {
        currentState = EnemyState.Patrol;
        agent.ResetPath();
    }

    // ---------------- HELPERS ----------------

    bool CanSeePlayer()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist > hearingRadius) return false;

        Vector3 dir = (player.position - transform.position).normalized;

        return !Physics.Raycast(transform.position + Vector3.up, dir, dist, obstacleMask);
    }
}