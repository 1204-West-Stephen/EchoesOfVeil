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
    private Transform player;

    [Header("Cameras")]
    private Camera mainCamera;
    public Camera jumpscareCamera;

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

    [Header("Jumpscare")]
    public float jumpscareDistance = 1.5f;
    public float jumpscareDuration = 2.3f;
    public Transform respawnPoint;

    private float chaseTimer;
    private bool screamPlayed;
    private bool isJumpscareActive;
    private Coroutine wanderCoroutine;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (!playerObj)
        {
            Debug.LogError("Player object not found!");
            enabled = false;
            return;
        }

        player = playerObj.transform;

        mainCamera = playerObj.GetComponent<Camera>();
        if (!mainCamera)
        {
            mainCamera = Camera.main;
        }

        if (!mainCamera)
        {
            Debug.LogError("Main camera not found!");
        }

        jumpscareCamera.enabled = false;

        currentState = EnemyState.Wander;
        wanderCoroutine = StartCoroutine(WanderRoutine());
    }

    void Update()
    {
        if (isJumpscareActive) return;

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

        Vector3 direction = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, direction);

        if (angle < viewAngle / 2f &&
            !Physics.Raycast(transform.position + Vector3.up, direction, distance, obstacleMask))
        {
            StartChase();
        }
    }

    // -------------------- WANDER --------------------
    IEnumerator WanderRoutine()
    {
        while (currentState == EnemyState.Wander)
        {
            Vector3 randomDir = Random.insideUnitSphere * wanderRadius + transform.position;
            Vector3 biasedDir = Vector3.Lerp(randomDir, player.position, playerBias);

            if (NavMesh.SamplePosition(biasedDir, out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
            {
                agent.speed = wanderSpeed;
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
            StopCoroutine(wanderCoroutine); // Only stop wander coroutine
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);

        if (!screamPlayed)
        {
            animator.SetTrigger("Scream");
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

    void CheckJumpscareDistance()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= jumpscareDistance)
        {
            StartCoroutine(JumpscareRoutine());
        }
    }

    bool CanSeePlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > hearingRadius) return false;

        Vector3 direction = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, direction);

        return angle < viewAngle / 2f &&
               !Physics.Raycast(transform.position + Vector3.up, direction, distance, obstacleMask);
    }

    void ReturnToWander()
    {
        currentState = EnemyState.Wander;
        agent.speed = wanderSpeed;
        animator.SetBool("isRunning", false);
        wanderCoroutine = StartCoroutine(WanderRoutine());
    }

    // -------------------- JUMPSCARE --------------------
    IEnumerator JumpscareRoutine()
    {
        if (isJumpscareActive) yield break;

        isJumpscareActive = true;
        currentState = EnemyState.Jumpscare;

        agent.isStopped = true;
        animator.SetTrigger("Jumpscare");

        PlayerControls pc = player.GetComponent<PlayerControls>();
        PlayerCamera cam = player.GetComponent<PlayerCamera>();

        if (pc) pc.DisableControls();
        if (cam) cam.controlLock();

        jumpscareCamera.enabled = true;

        yield return new WaitForSeconds(jumpscareDuration);

        if (respawnPoint)
            player.position = respawnPoint.position;

        jumpscareCamera.enabled = false;

        if (pc) pc.EnableControls();
        if (cam) cam.controlUnlock();

        agent.isStopped = false;

        isJumpscareActive = false;

        // Reset chase variables so enemy can attack again
        chaseTimer = initialChaseTime;
        screamPlayed = false;

        ReturnToWander();

        // Optional: force immediate detection after respawn
        DetectPlayer();
    }
}
