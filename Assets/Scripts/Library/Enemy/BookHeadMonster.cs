using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Wander, Chase, Jumpscare }
    public EnemyState currentState;

    [Header("References")]
    public NavMeshAgent agent;
    public Animator animator;
    public Transform player;

    [Header("Detection")]
    public float hearingRadius = 15f;
    public float viewAngle = 90f;
    public LayerMask obstacleMask;

    [Header("Wandering")]
    public float wanderRadius = 20f;
    public float wanderDelay = 3f;
    public float playerBias = 0.4f;

    [Header("Chase")]
    public float chaseSpeed = 6f;
    public float wanderSpeed = 2f;
    public float initialChaseTime = 5f;
    public float chaseExtension = 2f;

    [Header("Jumpscare")]
    public string jumpscareSceneName = "JumpscareScene";
    public string currentSceneName = "Library";

    float chaseTimer;
    bool screamPlayed;
    bool isJumpscareActive;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        currentState = EnemyState.Wander;
        StartCoroutine(WanderRoutine());
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

        if (angle < viewAngle / 2f)
        {
            if (!Physics.Raycast(transform.position + Vector3.up, direction, distance, obstacleMask))
            {
                StartChase();
            }
        }
    }

    // -------------------- WANDER --------------------
    IEnumerator WanderRoutine()
    {
        while (currentState == EnemyState.Wander)
        {
            Vector3 randomDir = Random.insideUnitSphere * wanderRadius;
            randomDir += transform.position;

            Vector3 biasedDir = Vector3.Lerp(randomDir, player.position, playerBias);

            if (NavMesh.SamplePosition(biasedDir, out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
            {
                agent.speed = wanderSpeed;
                agent.SetDestination(hit.position);
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);
            }

            if (Random.value < 0.2f)
            {
                animator.SetTrigger("Lurk");
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
        StopAllCoroutines();
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
        {
            chaseTimer += chaseExtension;
        }

        if (chaseTimer <= 0f && !CanSeePlayer())
        {
            ReturnToWander();
        }
    }

    bool CanSeePlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > hearingRadius) return false;

        Vector3 direction = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, direction);

        if (angle < viewAngle / 2f)
        {
            if (!Physics.Raycast(transform.position + Vector3.up, direction, distance, obstacleMask))
            {
                return true;
            }
        }
        return false;
    }

    void ReturnToWander()
    {
        currentState = EnemyState.Wander;
        agent.speed = wanderSpeed;
        animator.SetBool("isRunning", false);
        StartCoroutine(WanderRoutine());
    }

    // -------------------- JUMPSCARE --------------------
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TriggerJumpscare();
        }
    }

    void TriggerJumpscare()
    {
        if (isJumpscareActive) return;

        isJumpscareActive = true;
        currentState = EnemyState.Jumpscare;
        agent.isStopped = true;

        SceneManager.LoadSceneAsync(jumpscareSceneName, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(currentSceneName, UnloadSceneOptions.None);
    }
}
