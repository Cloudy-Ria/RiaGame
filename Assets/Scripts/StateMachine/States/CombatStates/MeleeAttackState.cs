using UnityEngine;

namespace StateMachine
{
    /// <summary>
    /// Состояние ближней атаки - враг атакует игрока
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class MeleeAttackState : State
    {
        [SerializeField] private Transform _raycastOrigin;
        [SerializeField] private float _raycastLength;
        [SerializeField] private float _raycastOriginOffsetX = 0f;

        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        private Transform _raycastOriginStartPosition;
        private Vector2 _raycastOriginOffsetXPosition;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            if (_raycastOrigin != null)
            {
                _raycastOriginStartPosition = _raycastOrigin;
                _raycastOriginOffsetXPosition = new Vector2(_raycastOriginStartPosition.position.x + _raycastOriginOffsetX, _raycastOriginStartPosition.position.y);
            }
        }

        private void OnEnable()
        {
            SetRaycastOriginPosition();
            if (_animator != null)
            {
                _animator.SetTrigger("Attack");
            }
        }

        private void OnDisable()
        {
            if (_animator != null)
            {
                _animator.ResetTrigger("Attack");
            }
        }

        /// <summary>
        /// Метод вызывается из анимации атаки (Animation Event)
        /// </summary>
        public void Attack()
        {
            if (Player == null || _raycastOrigin == null)
                return;

            FlipToPlayer();

            Vector2 raycastDirection = _spriteRenderer.flipX ? Vector2.left : Vector2.right;

            RaycastHit2D hit = Physics2D.Raycast(_raycastOrigin.position, raycastDirection, _raycastLength);

            DrawRaycast(_raycastOrigin.position, raycastDirection);

            if (hit.collider != null)
            {
                PlayerController player = hit.collider.gameObject.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage();
                }
            }
        }

        private void FlipToPlayer()
        {
            Vector2 directionToPlayer = Player.position - transform.position;
            directionToPlayer.Normalize();

            if (directionToPlayer.x < 0)
            {
                _spriteRenderer.flipX = true;
                if (_raycastOrigin != null)
                {
                    _raycastOrigin.position = _raycastOriginOffsetXPosition;
                }
            }
            else if (directionToPlayer.x > 0)
            {
                _spriteRenderer.flipX = false;
                if (_raycastOrigin != null)
                {
                    _raycastOrigin = _raycastOriginStartPosition;
                }
            }
        }

        private void SetRaycastOriginPosition()
        {
            if (_raycastOrigin == null || _spriteRenderer == null)
                return;

            if ((!_spriteRenderer.flipX && _raycastOrigin.localPosition.x <= 0) || (_spriteRenderer.flipX && _raycastOrigin.localPosition.x >= 0))
            {
                FlipXRaycastOrigin();
            }
        }

        private void FlipXRaycastOrigin()
        {
            if (_raycastOrigin == null)
                return;

            Vector2 position = _raycastOrigin.localPosition;
            position.x *= -1;
            _raycastOrigin.localPosition = position;
        }

        private void DrawRaycast(Vector2 raycastOrigin, Vector2 raycastDirection) => Debug.DrawRay(raycastOrigin, raycastDirection, Color.green);
    }
}

