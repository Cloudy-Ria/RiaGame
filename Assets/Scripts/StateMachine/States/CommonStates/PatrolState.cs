using UnityEngine;

namespace StateMachine
{
    /// <summary>
    /// Состояние патрулирования - враг движется между точками waypoints
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Transform))]
    public class PatrolState : State
    {
        [SerializeField] private MoveDirection _moveDirection;
        [SerializeField] private Transform[] _waypoints;
        [SerializeField] private float _speed = 2f;
        [SerializeField] private float _colliderOffsetX;

        private int waypointIndex = 0;
        private SpriteRenderer _spriteRenderer;
        private BoxCollider2D _collider;
        private Animator _animator;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<BoxCollider2D>();
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            if (_animator != null)
            {
                _animator.SetTrigger("Patrol");
            }
        }

        private void OnDisable()
        {
            if (_animator != null)
            {
                _animator.ResetTrigger("Patrol");
            }
        }


        private void Start()
        {
            // Проверяем, что все waypoints являются дочерними объектами
            ValidateWaypoints();
            
            if (_waypoints != null && _waypoints.Length > 0)
            {
                MoveToNextWaypoint();
            }
        }

        private void ValidateWaypoints()
        {
            if (_waypoints == null || _waypoints.Length == 0)
                return;

            for (int i = 0; i < _waypoints.Length; i++)
            {
                if (_waypoints[i] == null)
                    continue;

                if (!_waypoints[i].IsChildOf(transform))
                {
                    Debug.LogWarning($"Waypoint {i} ({_waypoints[i].name}) не является дочерним объектом врага. " +
                                   $"Переместите его в Hierarchy как дочерний объект врага для правильной работы патрулирования.");
                }
            }
        }

        private void Update()
        {
            if (_waypoints == null || _waypoints.Length == 0)
                return;

            if (ReachedCurrentWaypoint())
            {
                MoveToNextWaypoint();
            }
            else
            {
                Move();
            }
        }

        private void Move()
        {
            transform.position = Vector2.MoveTowards(transform.position, _waypoints[waypointIndex].position, _speed * Time.deltaTime);

            Vector2 direction = (_waypoints[waypointIndex].position - transform.position).normalized;

            switch (_moveDirection)
            {
                case MoveDirection.Horizontal:
                    _spriteRenderer.flipX = direction.x < 0;
                    _collider.offset = new Vector2(_spriteRenderer.flipX ? _colliderOffsetX : 0, _collider.offset.y);
                    break;
                case MoveDirection.Vertical:
                    _spriteRenderer.flipX = direction.y > 0;
                    _collider.offset = new Vector2(_spriteRenderer.flipX ? 0 : _colliderOffsetX, _collider.offset.y);
                    break;
                default:
                    break;
            }
        }

        private bool ReachedCurrentWaypoint()
        {
            return Vector2.Distance(transform.position, _waypoints[waypointIndex].position) < 0.5f;
        }

        private void MoveToNextWaypoint()
        {
            waypointIndex = (waypointIndex + 1) % _waypoints.Length;
        }
    }
}

