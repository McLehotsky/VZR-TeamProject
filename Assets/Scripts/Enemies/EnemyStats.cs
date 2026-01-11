using UnityEngine;

public class EnemyStats : MonoBehaviour, IDamageable
{
    public int health = 100;
    Animator animator; // Hit reactions

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Enemy dostal hit: " + damage + " | HP: " + health);

        if (health <= 0)
        {
            // Die animation
            // animator.Play("Death");
        }
        else
        {
            // Hit animation
            // animator.Play("Damage_01");
        }
    }
}