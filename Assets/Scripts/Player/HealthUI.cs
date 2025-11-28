using UnityEngine;
using TMPro;

public class HealthUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public TMP_Text healthText;

    void OnEnable()
    {
        // Intento de auto-asignación si está vacío
        if (playerHealth == null)
        {
            playerHealth = FindObjectOfType<PlayerHealth>();
            if (playerHealth != null) Debug.Log("HealthUI: PlayerHealth encontrado automáticamente.");
        }
        
        if (healthText == null)
        {
            healthText = GetComponent<TMP_Text>();
            if (healthText != null) Debug.Log("HealthUI: TMP_Text encontrado en el mismo objeto.");
        }

        // Suscripción al evento
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHealthText;
            UpdateHealthText(playerHealth.currentHealth);
        }
        else
        {
            Debug.LogError("HealthUI: PlayerHealth NO asignado. Arrastra el objeto Player al campo PlayerHealth en el Inspector.");
        }
    }

    void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthText;
        }
    }

    void UpdateHealthText(int currentHealth)
    {
        if (healthText != null)
        {
            healthText.text = currentHealth.ToString();
            // Debug.Log("HealthUI: Texto actualizado a " + currentHealth); // Descomentar para debug
        }
        else
        {
            Debug.LogError("HealthUI: HealthText (TMP) NO asignado. Arrastra el objeto de Texto al campo HealthText.");
        }
    }
}
