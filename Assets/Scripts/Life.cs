using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [SerializeField] private int maxHealth = 500;
    [SerializeField] private int currentHealth;
    [SerializeField] private int damageAmount = 10;

    [SerializeField] private Image healthBarImage;
    public delegate void OnHealthDepleted();
    public event OnHealthDepleted onHealthDepleted;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Persona_Atack"))
        {
            TakeDamage(damageAmount);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("perdiste manco");


            if (onHealthDepleted != null)
            {
                onHealthDepleted();
            }
        }

        UpdateHealthBar();

        Debug.Log("Vida restante " + currentHealth);
    }

    private void ReduceHealth(int amount)
    {
        TakeDamage(amount); 
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

    public bool IsAlive()
    {
        return currentHealth > 0;
    }
}