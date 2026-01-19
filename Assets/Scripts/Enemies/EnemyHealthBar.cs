using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider slider;       
    public Vector3 offset;       

    private Transform target;    

    private float maxHealth;

    // Инициализация полоски
    public void Initialize(Transform enemyTransform, float maxHealth)
    {
        target = enemyTransform;
        this.maxHealth = maxHealth;

        slider.maxValue = maxHealth;
        slider.value = maxHealth;

        slider.gameObject.SetActive(true);
    }

    // Обновление значения здоровья
    public void SetHealth(float health)
    {
        slider.value = health;
    }

    private void Update()
    {
        if (target != null)
        {

            Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position + offset);
            slider.transform.position = screenPos;
        }
    }
}
