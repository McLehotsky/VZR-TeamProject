using UnityEngine;
using System.Collections.Generic;

public class DamageCollider : MonoBehaviour
{
    // This script handles the damage collider for melee attacks.
    public int currentDamage;

    // To keep track of already hit enemies in the current swing
    List<GameObject> hitEnemies = new List<GameObject>();
    Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();
        col.isTrigger = true;
        col.enabled = false;
    }

    public void EnableDamageCollider()
    {
        hitEnemies.Clear(); // Clear the list at the start of each swing
        col.enabled = true;
    }

    public void DisableDamageCollider()
    {
        col.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1. Check if the collider belongs to an enemy
        if (other.CompareTag("Enemy"))
        {
            // 2. Check if already hit
            if (!hitEnemies.Contains(other.gameObject))
            {
                // 3. Apply damage
                IDamageable enemy = other.GetComponent<IDamageable>();

                if (enemy != null)
                {
                    enemy.TakeDamage(currentDamage);
                    hitEnemies.Add(other.gameObject); // Zapamätaj si ho
                }
            }
        }
    }
}