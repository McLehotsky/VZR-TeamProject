using UnityEngine;

public class EnemyStats : MonoBehaviour, IDamageable
{
    public int health = 100;

    EnemyAnimatorManager animatorManager;
    EnemyManager enemyManager; // <--- Potrebujeme referenciu na mozog
    Collider enemyCollider;    // Potrebujeme referenciu na hlavný collider (aby sme cez mŕtvolu mohli prejsť)

    private void Awake()
    {
        animatorManager = GetComponent<EnemyAnimatorManager>();
        enemyManager = GetComponent<EnemyManager>();
        enemyCollider = GetComponent<Collider>(); // Zvyčajne CapsuleCollider na hlavnom objekte
    }

    public void TakeDamage(int damage)
    {
        // Ak už je mŕtvy, ignoruj ďalšie zásahy
        if (enemyManager.isDead)
            return;

        health -= damage;
        // Debug.Log("Enemy dostal hit: " + damage + " | HP: " + health);

        if (health <= 0)
        {
            health = 0;
            HandleDeath(); // Zavoláme našu novú funkciu
        }
        else
        {
            enemyManager.HandleRecovery();
            // Hit animation
            animatorManager.PlayTargetAnimation("Damage_01", true);
        }
    }

    private void HandleDeath()
    {
        // 1. Nastavíme flag v Manageri (toto zastaví Update loop)
        enemyManager.isDead = true;

        // 2. Prehráme animáciu smrti
        animatorManager.PlayTargetAnimation("Death_01", true);

        // 3. Vypneme Collider, aby hráč do mŕtvoly nenarážal (mohol cez ňu prejsť)
        if (enemyCollider != null)
            enemyCollider.enabled = false;

        // 4. Naplánujeme zničenie objektu o 5 sekúnd
        Destroy(gameObject, 5f);
    }
}