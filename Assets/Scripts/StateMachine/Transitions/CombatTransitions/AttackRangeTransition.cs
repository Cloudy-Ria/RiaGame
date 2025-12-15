using UnityEngine;

namespace StateMachine
{
    /// <summary>
    /// Переход когда игрок находится в зоне атаки
    /// </summary>
    public class AttackRangeTransition : Transition
    {
        [SerializeField] private float _attackRange = 1.5f;
        [SerializeField] private LayerMask _targetLayer;

        private void Update()
        {
            if (Player == null)
                return;

            float distanceToPlayer = Vector2.Distance(transform.position, Player.position);

            if (distanceToPlayer <= _attackRange)
            {
                NeedTransit = true;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, _attackRange);
        }
    }
}

