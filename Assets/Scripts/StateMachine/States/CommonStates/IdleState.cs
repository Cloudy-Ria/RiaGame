using UnityEngine;

namespace StateMachine
{
    /// <summary>
    /// Состояние бездействия - враг стоит на месте
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class IdleState : State
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            if (_animator != null)
            {
                _animator.SetTrigger("Idle");
            }
        }

        private void OnDisable()
        {
            if (_animator != null)
            {
                _animator.ResetTrigger("Idle");
            }
        }
    }
}

