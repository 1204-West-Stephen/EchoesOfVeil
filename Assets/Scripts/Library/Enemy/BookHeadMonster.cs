using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Wander, Chase, Jumpscare }
    public EnemyState currentState;

    [Header("References")]
    private NavMeshAgent agent;
    private Animator animator;
    public Animator jumpscareAnimator;
    private Transform player;
    private PlayerControls playerControls;

    [Header("Cameras")]
    public Camera jumpscareCamera;
    private Camera playerCamera;

    [Header("Detection")]
    public float hearingRadius = 8f;
    public float viewAngle = 50f;
    public LayerMask obstacleMask;

    [Header("Wandering")]
    public float wanderRadius = 40f;
    public float wanderDelay = 5f;
    public float playerBias = 0.3f;

    [Header("Chase")]
    public float chaseSpeed = 2f;
    public float wanderSpeed = 0.8f;
    public float initialChaseTime = 5f;
    public float chaseExtension = 2f;

    [Header("Scream")]
    public float screamStopTime = 1.0f;

    [Header("Jumpscare")]
    public float jumpscareDistance = 1.5f;
    public float jumpscareDuration = 2.3f;
    public Transform respawnPoint;

    [Header("Reset")]
    public Transform enemyResetPoint;
    public float postRespawnGraceTime = 1.2f;

    private float chaseTimer;
    private bool screamPlayed;
    private bool isJumpscareActive;
    private bool isFrozen;
    private bool canJumpscare = true;
    private Coroutine wanderCoroutine;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        animator.applyRootMotion = false;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.transform;
        playerControls = playerObj.GetComponent<PlayerControls>();

        playerCamera = playerObj.GetComponentInChildren<Camera>();

        currentState = EnemyState.Wander;
        wanderCoroutine = StartCoroutine(WanderRoutine());
    }

    void Awake()
    {
        if (jumpscareCamera)
            jumpscareCamera.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isFrozen || isJumpscareActive) return;

        switch (currentState)
        {
            case EnemyState.Wander:
                DetectPlayer();
                break;

            case EnemyState.Chase:
                ChasePlayer();
                CheckJumpscareDistance();
                break;
        }
    }

    // -------------------- DETECTION --------------------
    void DetectPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > hearingRadius) return;

        Vector3 dir = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, dir);

        if (angle < viewAngle / 2f &&
            !Physics.Raycast(transform.position + Vector3.up, dir, distance, obstacleMask))
        {
            StartChase();
        }
    }

    // -------------------- WANDER --------------------
    IEnumerator WanderRoutine()
    {
        while (currentState == EnemyState.Wander)
        {
            if (NavMesh.SamplePosition(Random.insideUnitSphere * wanderRadius + transform.position,
                out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
            {
                if (!agent.isOnNavMesh || agent.isStopped)
                    yield return null;

                agent.speed = wanderSpeed;

                if (agent.isOnNavMesh)
                    agent.SetDestination(hit.position);

                agent.SetDestination(hit.position);

                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);
            }

            yield return new WaitForSeconds(wanderDelay);
        }
    }

    // -------------------- CHASE --------------------
    void StartChase()
    {
        if (currentState == EnemyState.Chase) return;

        currentState = EnemyState.Chase;
        chaseTimer = initialChaseTime;
        screamPlayed = false;

        agent.speed = chaseSpeed;

        if (wanderCoroutine != null)
            StopCoroutine(wanderCoroutine);
    }

    void ChasePlayer()
    {
        if (isFrozen) return;

        if (!agent.isOnNavMesh || agent.isStopped) return;
       
        agent.SetDestination(player.position);

        if (!screamPlayed)
        {
            StartCoroutine(ScreamRoutine());
            screamPlayed = true;
        }

        animator.SetBool("isRunning", true);
        animator.SetBool("isWalking", false);

        chaseTimer -= Time.deltaTime;

        if (CanSeePlayer() && chaseTimer <= 0f)
            chaseTimer += chaseExtension;

        if (chaseTimer <= 0f && !CanSeePlayer())
            ReturnToWander();
    }

    // -------------------- SCREAM --------------------
    IEnumerator ScreamRoutine()
    {
        FreezeEnemy();

        animator.SetTrigger("Scream");

        yield return new WaitForSeconds(screamStopTime);

        UnfreezeEnemy();
    }

    void CheckJumpscareDistance()
    {
        if (!canJumpscare || isFrozen) return;

        if (Vector3.Distance(transform.position, player.position) <= jumpscareDistance)
            StartCoroutine(JumpscareRoutine());
    }

    bool CanSeePlayer()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist > hearingRadius) return false;

        Vector3 dir = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, dir);

        return angle < viewAngle / 2f &&
               !Physics.Raycast(transform.position + Vector3.up, dir, dist, obstacleMask);
    }

    void ReturnToWander()
    {
        currentState = EnemyState.Wander;

        if (wanderCoroutine != null)
            StopCoroutine(wanderCoroutine);

        wanderCoroutine = StartCoroutine(WanderRoutine());
    }

    // -------------------- JUMPSCARE / KILL --------------------
    IEnumerator JumpscareRoutine()
    {
        if (wanderCoroutine != null)
        {
            StopCoroutine(wanderCoroutine);
            wanderCoroutine = null;
        }

        if (isJumpscareActive) yield break;

        isJumpscareActive = true;
        canJumpscare = false;
        currentState = EnemyState.Jumpscare;

        jumpscareAnimator.Play("JumpScare", 0, 0f);
        jumpscareAnimator.Update(0f); // forces pose update THIS frame

        yield return null;

        jumpscareCamera.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(false);


        FreezeEnemy();

        if (playerControls != null)
            playerControls.DisableControls();

        jumpscareCamera.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(false);

        yield return new WaitForSeconds(jumpscareDuration);

        if (respawnPoint)
            player.position = respawnPoint.position;

        if (enemyResetPoint) { 
            if (NavMesh.SamplePosition(enemyResetPoint.position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                agent.Warp(hit.position);
            }
            else
            {
                Debug.LogError("Enemy reset point is NOT on NavMesh!");
            }
        }

        playerCamera.gameObject.SetActive(true);
        jumpscareCamera.gameObject.SetActive(false);

        if (playerControls != null)
            playerControls.EnableControls();

        UnfreezeEnemy();

        currentState = EnemyState.Wander;
        isJumpscareActive = false;

        wanderCoroutine = StartCoroutine(WanderRoutine());

        yield return new WaitForSeconds(postRespawnGraceTime);
        canJumpscare = true;
    }

    // -------------------- FREEZE HELPERS --------------------
    void FreezeEnemy()
    {
        isFrozen = true;

        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.updatePosition = false;
        agent.updateRotation = false;

        animator.speed = 0f;
    }

    void UnfreezeEnemy()
    {
        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.isStopped = false;

        animator.speed = 1f;

        agent.ResetPath();
        isFrozen = false;
    }
}
