using Enemies;
using System.Threading;
using UnityEngine;

public class PlayerMeleeAttackHitbox : MonoBehaviour
{
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private PlayerController playerController;
    [SerializeField] float damage = 10f;
    [SerializeField] float jumpHeight = 30f;
    [SerializeField] float cooldown = 0.3f;
    private float timer = 1000f;

    private void Update()
    {
        timer += Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster") && timer>cooldown)
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            knockback(jumpHeight);
            timer = 0f;
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            playerAttack.DeactivatePogoHitbox();
        }

    }

    private void knockback(float _jumpHeight)
    {
        playerController.Knock(Vector2.up, _jumpHeight);
    }
}
