using System.Collections;
using Enemies.Interfaces;
using UnityEngine;

namespace Enemies
{
    /// <summary>
    /// Базовый класс врага - наносит урон при столкновении с игроком
    /// Требует наличия BoxCollider2D (добавляется автоматически через PatrolState)
    /// </summary>
    public class Enemy : MonoBehaviour, IDamaging
    {
        [SerializeField] private float _enableColliderDelay = 1.5f;

        private BoxCollider2D _collider;

        private void Awake() => _collider = GetComponent<BoxCollider2D>();

        public void ApplyDamage(HealthManager player) => player.ReduceHealth(1);

        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameObject player = collision.gameObject;
            if (collision.gameObject.CompareTag("Player"))
            {
                if (player.GetComponent<PlayerController>().moveable == 0) // Проверяем, может ли игрок двигаться
                {
                    ApplyDamage(player.GetComponent<HealthManager>());
                    StartCoroutine(TemporarilyDisableCollider());
                }
            }
        }

        private IEnumerator TemporarilyDisableCollider()
        {
            _collider.enabled = false;
            yield return new WaitForSeconds(_enableColliderDelay);
            _collider.enabled = true;
        }
    }
}

