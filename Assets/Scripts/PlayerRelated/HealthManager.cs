using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public int maxHealth = 3;      // Максимальное здоровье
    public int currentHealth = 3;    // Текущее здоровье
    [SerializeField] HealthHeartsBar HealthUI; //Полоска со здоровьем
    PlayerController playerController;
    private float iFramesTimer = Mathf.Infinity;
    [SerializeField] private float iFramesMax = 1f;

    void Start()
    {
        UpdateHealthUI();
        playerController = GetComponent<PlayerController>();
        
    }
    private void Update()
    {
        iFramesTimer += Time.deltaTime;
    }

    // Метод для получения урона
    public void ReduceHealth(int damage, GameObject source)
    {
        if (iFramesTimer >= iFramesMax)
        {
            iFramesTimer = 0;
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            UpdateHealthUI();

            if (currentHealth <= 0)
            {
                playerController.Die();
            }
            else
            {
                playerController.TakeDamage(source);
            }
        }
    }

    // Метод для восстановления здоровья
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }
    
    // Метод для обновления UI
    public void UpdateHealthUI()
    {
        HealthUI.updateHealth(currentHealth, maxHealth);
    }


}
