using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{
    public string targetBool; // Meno premennej (napr. "IsInteracting")
    public bool status;       // Na aku hodnotu ju resetnut (napr. false)

    // Táto funkcia sa zavolá AUTOMATICKY, keď animácia skončí (alebo prejde do inej)
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(targetBool, status);
    }
}