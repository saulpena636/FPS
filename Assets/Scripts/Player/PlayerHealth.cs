using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public event System.Action<int> OnHealthChanged;

    [Header("Audio")]
    public AudioClip damageSound;
    public AudioSource audioSource;

    void Start()
    {
        currentHealth = maxHealth;
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        OnHealthChanged?.Invoke(currentHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;
        
        OnHealthChanged?.Invoke(currentHealth);

        if (audioSource != null && damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
            Debug.Log("Player took damage. Playing damage sound.");
        }
        else
        {
            Debug.LogWarning("Player took damage but audioSource or damageSound is NULL.");
        }

        Debug.Log("Player took damage: " + amount + ". Current Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Died!");
        Time.timeScale = 0f;
        // Add death logic here (e.g., reload scene, show game over screen)
    }
}
