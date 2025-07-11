using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 3f;
    private float currentHealth;
    public GameObject[] indicadorSalud;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // Asegúrate que los enemigos tengan este tag
        {
            TakeDamage(maxHealth / 3f);
        }
    }

    private void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0f);
        foreach (GameObject icon in indicadorSalud)
        {
            icon.SetActive(false);
        }
        if (currentHealth > 0)
        {
            indicadorSalud[(int)(currentHealth - 1)].SetActive(true);
        }
        Debug.Log("Player hit! Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died.");
        // Aquí puedes desactivar al jugador, mostrar GameOver, etc.
    }
}

