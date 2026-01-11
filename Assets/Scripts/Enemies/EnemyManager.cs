using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic; // Needed for Lists

public class EnemyManager : MonoBehaviour
{
    EnemyAnimatorManager animatorManager;
    EnemyWeaponSlotManager weaponSlotManager;
    EnemyStats enemyStats;
    NavMeshAgent navAgent;
    EnemyAnimatorHook enemyAnimationHook;

    [Header("Target Info")]
    public Transform currentTarget; // The Player
    public float distanceFromTarget;

    [Header("AI Settings")]
    public float detectionRadius = 10f;  // Range to start chasing
    public float attackRange = 1.5f;     // Range to stop and attack
    public float stopChaseDistance = 15f;// Range to give up

    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    public int currentPatrolIndex = 0;

    [Header("Combat Settings")]
    public WeaponItem currentWeapon; // Drag your Weapon Scriptable Object here!
    public float attackCooldown = 2f;
    float currentRecoveryTime = 0;

    [Header("States")]
    public bool isInteracting;
    public enum AIState { Patrol, Chase, Attack }
    public AIState currentState;

    public bool isDead;

    private void Awake()
    {
        animatorManager = GetComponent<EnemyAnimatorManager>();
        weaponSlotManager = GetComponent<EnemyWeaponSlotManager>();
        enemyStats = GetComponent<EnemyStats>();
        navAgent = GetComponent<NavMeshAgent>();
        enemyAnimationHook = GetComponentInChildren<EnemyAnimatorHook>();

        // Auto-find player by Tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            currentTarget = player.transform;
        }
    }

    private void Start()
    {
        // Load the weapon visually and set up logic (Collider damage, Animator Layers)
        if (currentWeapon != null)
        {
            // false = Right Hand (assuming primary weapon is right handed)
            weaponSlotManager.LoadWeaponOnSlot(currentWeapon, false);
        }

        // Initialize NavMesh agent speed based on patrol state
        navAgent.speed = 1.5f;
    }

    private void Update()
    {
        if (isDead)
            return;

        // Handle Cooldown Timer
        if (currentRecoveryTime > 0)
        {
            currentRecoveryTime -= Time.deltaTime;
        }

        // Sync Interacting Bool from Animator (via hook or direct reference)
        // Assuming Animator is on a child object
        isInteracting = GetComponentInChildren<Animator>().GetBool("isInteracting");

        // If we are attacking or being hit, stop movement logic
        if (isInteracting)
        {
            navAgent.enabled = false; // Stop NavMesh movement
            navAgent.velocity = Vector3.zero; // Kill velocity
            return;
        }
        else
        {
            navAgent.enabled = true;
        }

        // Run State Machine Logic
        HandleStateMachine();

        // Update movement animations
        animatorManager.UpdateAnimator();
    }

    void HandleStateMachine()
    {
        if (currentTarget == null) return;

        distanceFromTarget = Vector3.Distance(transform.position, currentTarget.position);

        switch (currentState)
        {
            case AIState.Patrol:
                // Logic: Move between points
                navAgent.speed = 1.5f; // Walk Speed (Anim 0.5)

                // If patrol points exist, move to them
                if (patrolPoints.Length > 0)
                {
                    if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance + 0.5f)
                    {
                        currentPatrolIndex++;
                        if (currentPatrolIndex >= patrolPoints.Length) currentPatrolIndex = 0;
                        navAgent.SetDestination(patrolPoints[currentPatrolIndex].position);
                    }
                }

                // Switch to Chase if player is close
                if (distanceFromTarget <= detectionRadius)
                {
                    currentState = AIState.Chase;
                }
                break;

            case AIState.Chase:
                // Logic: Run towards player
                navAgent.speed = 2.5f; // Run Speed (Anim 1.0)
                navAgent.SetDestination(currentTarget.position);

                // Switch to Attack if close enough
                if (distanceFromTarget <= attackRange)
                {
                    currentState = AIState.Attack;
                }

                // Give up if player is too far
                if (distanceFromTarget > stopChaseDistance)
                {
                    currentState = AIState.Patrol;
                    if (patrolPoints.Length > 0)
                        navAgent.SetDestination(patrolPoints[currentPatrolIndex].position);
                }
                break;

            case AIState.Attack:
                // Logic: Stop and Attack
                navAgent.velocity = Vector3.zero; // Hard stop

                // Rotate towards target manually (since Agent is stopped)
                Vector3 direction = currentTarget.position - transform.position;
                direction.y = 0;
                if (direction != Vector3.zero)
                {
                    Quaternion targetRot = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 5f * Time.deltaTime);
                }

                // Attack if cooldown is ready
                if (currentRecoveryTime <= 0)
                {
                    AttackTarget();
                }

                // If target moved away, go back to Chase
                if (distanceFromTarget > attackRange)
                {
                    currentState = AIState.Chase;
                }
                break;
        }
    }

    public void HandleRecovery()
    {
        // 1. Resetujeme stav do Chase alebo Combat
        // (Aby nezostal zaseknutý v Attack logike)
        currentState = AIState.Chase;

        // 2. Zastavíme NavMesh Agenta okamžite
        navAgent.enabled = false;

        // 3. Vypneme zbraň, ak práve útočil (Interruption)
        if (enemyAnimationHook != null)
        {
            enemyAnimationHook.CloseRightWeaponCollider();
        }
    }

    void AttackTarget()
    {
        if (currentWeapon == null) return;

        // Build a temporary list of available attacks from the Weapon Item
        List<string> availableAttacks = new List<string>();

        if (!string.IsNullOrEmpty(currentWeapon.Light_Attack_1))
            availableAttacks.Add(currentWeapon.Light_Attack_1);

        if (!string.IsNullOrEmpty(currentWeapon.Light_Attack_2))
            availableAttacks.Add(currentWeapon.Light_Attack_2);

        if (!string.IsNullOrEmpty(currentWeapon.Heavy_Attack_1))
            availableAttacks.Add(currentWeapon.Heavy_Attack_1);

        // Pick a random attack
        if (availableAttacks.Count > 0)
        {
            int randomIdx = Random.Range(0, availableAttacks.Count);
            string attackAnimation = availableAttacks[randomIdx];

            // Play the animation
            animatorManager.PlayTargetAnimation(attackAnimation, true);

            // Set cooldown
            currentRecoveryTime = attackCooldown;
        }
    }
}