using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    [Header("Target & Movement")]
    public Transform player;
    public float speed = 3f;
    
    [Header("Combat")]
    public float health = 100f;
    public float attackRange = 1.8f;  // Distance to start hitting
    public float attackDamage = 10f;
    public float attackCooldown = 1.5f; // Time between hits
    private float nextAttackTime = 0f;

    [Header("Visuals")]
    public Color flashColor = Color.red;

    private Rigidbody rb;
    private Renderer myRenderer;
    private Color originalColor;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        myRenderer = GetComponent<Renderer>();
        if (myRenderer != null) originalColor = myRenderer.material.color;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        // 1. Calculate Distance
        Vector3 direction = player.position - transform.position;
        direction.y = 0; // Keep movement flat
        float distance = direction.magnitude;

        // 2. Move towards player if outside attack range
        if (distance > attackRange)
        {
            rb.linearVelocity = direction.normalized * speed;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 10f * Time.fixedDeltaTime);
        }
        else
        {
            rb.linearVelocity = Vector3.zero; // Stop when close enough
        }

        // 3. Attack Logic (If close enough and cooldown is ready)
        if (distance <= attackRange && Time.time >= nextAttackTime)
        {
            // Find the PlayerHealth script on the player
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
                Debug.Log("Enemy HIT the Player!");
            }
            
            // Reset cooldown
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        
        // Update UI (Searches children to find the Health Bar script)
        EnemyHealthBar ui = GetComponentInChildren<EnemyHealthBar>();
        if (ui != null) ui.UpdateHealth(health);
        
        FlashDamage();
        if (health <= 0) Die();
    }

    void FlashDamage()
    {
        if (myRenderer != null)
        {
            myRenderer.material.color = flashColor;
            Invoke("ResetColor", 0.1f);
        }
    }

    void ResetColor()
    {
        if (myRenderer != null) myRenderer.material.color = originalColor;
    }

    void Die()
    {
        StartCoroutine(DieAnimation());
    }

    IEnumerator DieAnimation()
    {
        myRenderer.material.color = Color.red;
        transform.localScale = Vector3.one * 1.2f;
        yield return new WaitForSeconds(0.1f);

        float duration = 1.0f;
        float time = 0;
        Vector3 startScale = transform.localScale;

        while (time < duration)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, time / duration);
            yield return null;
        }
        Destroy(gameObject);
    }
}