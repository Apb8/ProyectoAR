using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class HealthController : MonoBehaviour
{
    [SerializeField] private int maxHealth = 500;
    [SerializeField] private int currentHealth;
    [SerializeField] private int damageAmount = 10;

    [SerializeField] private Image healthBarImage;
    public delegate void OnHealthDepleted();
    public event OnHealthDepleted onHealthDepleted;
    public Score_logic score;
    private void Start()
    {
        currentHealth = maxHealth;
        score = FindObjectOfType<Score_logic>();
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
            Die();

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

    private void Die()
    {
        score.score = 0;
        score.JackFrostID = 0;
        score.JackoLantern = 0;
        score.BlackFrostID = 0;
        SceneManager.LoadScene(1);

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