using UnityEngine;

namespace StateMachine
{
    /// <summary>
    /// Переход когда игрок выходит из зоны атаки
    /// </summary>
    public class OutOfAttackRangeTransition : Transition
    {
        [SerializeField] private float _attackRange = 1.5f;

        private void Update()
        {
            if (Player == null)
                return;

            float distanceToPlayer = Vector2.Distance(transform.position, Player.position);

            if (distanceToPlayer > _attackRange)
            {
                NeedTransit = true;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _attackRange);
        }
    }
}

