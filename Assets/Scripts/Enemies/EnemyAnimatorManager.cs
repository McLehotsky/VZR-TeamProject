using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimatorManager : MonoBehaviour
{
    Animator animator;
    NavMeshAgent navAgent;
    EnemyManager enemyManager;

    int verticalID;

    // Index cache for animation layers
    int layer2H_ID;
    int layerPolearm_ID;
    int layerGripRight_ID;
    int layerGripLeft_ID;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        enemyManager = GetComponent<EnemyManager>();

        // Setting indexes by name from Animator
        layer2H_ID = animator.GetLayerIndex("Layer_Stance_2H");
        layerPolearm_ID = animator.GetLayerIndex("Layer_Stance_Polearm");
        layerGripRight_ID = animator.GetLayerIndex("Layer_Grip_Right");
        layerGripLeft_ID = animator.GetLayerIndex("Layer_Grip_Left");

        verticalID = Animator.StringToHash("Vertical");
    }

    public void UpdateAnimator()
    {
        // 1. Zistíme rýchlosť z NavMesh Agenta
        float velocity = navAgent.velocity.magnitude;

        // 2. Mapovanie rýchlosti na animáciu (0, 0.5, 1)
        float snappedVertical = 0;

        if (velocity > 0.1f && velocity < 2f) // Chôdza
        {
            snappedVertical = 0.5f;
        }
        else if (velocity > 2f) // Beh (Chase)
        {
            snappedVertical = 1f;
        }

        // Posielame do Animatoru (používame rovnaký controller ako Player!)
        // Horizontal nepotrebujeme, AI sa otáča celým telom
        animator.SetFloat(verticalID, snappedVertical, 0.1f, Time.deltaTime);
    }

    // Handlling animation layers for holding items/weapons
    public void SetWeaponType(int weaponID)
    {
        animator.SetInteger("WeaponType", weaponID);

        // RESET of animation layers (Weight = 0)
        animator.SetLayerWeight(layer2H_ID, 0);
        animator.SetLayerWeight(layerPolearm_ID, 0);
        animator.SetLayerWeight(layerGripRight_ID, 0);
        animator.SetLayerWeight(layerGripLeft_ID, 0);

        // SWITCH LOGIC
        switch (weaponID)
        {
            case 1: // 1H Weapon = Hand Grip Only
                animator.SetLayerWeight(layerGripRight_ID, 1);
                break;

            case 2: // 2H Weapon = (Shoulder Stance + Hand Grip)
                animator.SetLayerWeight(layer2H_ID, 1);
                animator.SetLayerWeight(layerGripRight_ID, 1);
                break;

            case 3: // Polearm (Both Arms Stance + Both Hands Grip)
                animator.SetLayerWeight(layerPolearm_ID, 1);
                animator.SetLayerWeight(layerGripRight_ID, 1);
                animator.SetLayerWeight(layerGripLeft_ID, 1);
                break;

                // Case 0 (Unarmed) No need to implement here because its already implemented when all layers are RESET
        }
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        animator.SetBool("isInteracting", isInteracting);
        animator.CrossFade(targetAnim, 0.2f);

        // Zastavíme pohyb počas útoku
        navAgent.enabled = false; // Vypneme agenta, aby nestrkal do hráča
    }

    // Volané každý frame (alebo LateUpdate) na synchronizáciu
    public void OnAnimatorMove()
    {
        // Synchronizujeme bool z Animatora späť do Managera
        // (Rovnako ako u Playera, aby sme vedeli kedy útok skončil)
        enemyManager.isInteracting = animator.GetBool("isInteracting");

        // Keď útok skončí (isInteracting false), zapneme znova Agenta
        if (!enemyManager.isInteracting)
        {
            navAgent.enabled = true;
        }
    }
}