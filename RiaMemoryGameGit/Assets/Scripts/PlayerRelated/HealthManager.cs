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

    //public GameObject gameOverPanel; // панель с game over
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
    public void ReduceHealth(int damage)
    {
        if (iFramesTimer >= iFramesMax)
        {
            iFramesTimer = 0;
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Убеждаемся, что здоровье не меньше 0
            UpdateHealthUI();

            if (currentHealth <= 0)
            {
                playerController.Die();

                // if (gameOverPanel != null)
                // {
                // gameOverPanel.SetActive(true); // Показать панель Game Over
                // }

                // Здесь можно добавить логику для смерти (например, перезагрузка уровня)
                // Debug.Log("Game Over!");
            }
            else
            {
                playerController.TakeDamage();
            }
        }
    }

    // Метод для восстановления здоровья (если нужно)
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Убеждаемся, что здоровье не больше maxHealth
        UpdateHealthUI();
    }
    
    // Метод для обновления UI
    public void UpdateHealthUI()
    {
        HealthUI.updateHealth(currentHealth, maxHealth);
    }


}
