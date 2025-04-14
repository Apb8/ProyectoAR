using UnityEngine;
using UnityEngine.UI; 

public class HealthController : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private int damageAmount = 10;

    [SerializeField] private Image healthBarImage; 

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }
    private void OnCollisionEnter (Collision collision)
    {
        if (collision.gameObject.CompareTag("Persona"))
        {
            ReduceHealth(damageAmount);
        }
    }
    private void ReduceHealth(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("¡Has perdido toda tu vida!");
        }

        UpdateHealthBar();

        Debug.Log("Vida restante: " + currentHealth);
    }
    private void UpdateHealthBar()
    {
        if (healthBarImage != null)
        {
            float fillAmount = (float)currentHealth / maxHealth;
            healthBarImage.fillAmount = fillAmount;
        }
    }
    public int GetHealth()
    {
        return currentHealth;
    }
}