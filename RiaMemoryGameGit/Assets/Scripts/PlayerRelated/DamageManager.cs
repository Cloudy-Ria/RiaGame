using UnityEngine;

public class DamageManager : MonoBehaviour
{
    [SerializeField] public int damageAmount = 1; // Количество урона

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Проверяем, что столкнулись с игроком (должен быть тег "Player")
        {
            HealthManager healthManager = other.GetComponent<HealthManager>();
            if (healthManager != null)
            {
                healthManager.ReduceHealth(damageAmount);
            }
        }
    }
}
