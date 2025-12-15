using UnityEngine;

namespace StateMachine
{
    /// <summary>
    /// Состояние преследования - враг движется к игроку
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class PursuitState : State
    {
        private SpriteRenderer _spriteRenderer;
        [SerializeField] public float _speed = 2f;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (Player != null)
            {
                PursueTarget();
            }
        }

        private void PursueTarget()
        {
            Vector2 direction = (Player.position - transform.position).normalized;
            transform.position += (Vector3)direction * _speed * Time.deltaTime;

            _spriteRenderer.flipX = direction.x < 0;
        }
    }
}

