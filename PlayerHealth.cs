using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Image healthFill; // Make sure this is linked!

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI(); // Call this at start
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log("Health is now: " + currentHealth); // Debug message
        UpdateHealthUI(); // This MUST be called

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
{
    if (healthFill != null)
    {
        // Instead of fillAmount, we'll scale the bar
        float healthPercent = currentHealth / maxHealth;
        
        // Get the current scale
        Vector3 newScale = healthFill.transform.localScale;
        
        // Change only the X scale (width)
        newScale.x = healthPercent;
        
        // Apply the new scale
        healthFill.transform.localScale = newScale;
        
        Debug.Log("Health Bar Scaled to: " + healthPercent);
    }
}

    void Die()
    {
        Debug.Log("Player Died!");
        Invoke("RestartGame", 1f);
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}