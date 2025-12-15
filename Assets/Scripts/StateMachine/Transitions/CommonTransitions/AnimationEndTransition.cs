using UnityEngine;

namespace StateMachine
{
    /// <summary>
    /// Переход когда анимация заканчивается
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class AnimationEndTransition : Transition
    {
        [SerializeField] private string _animationName;
        [SerializeField] private float _normalizedTime = 0.9f;

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_animator != null && !string.IsNullOrEmpty(_animationName))
            {
                AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
                
                if (stateInfo.IsName(_animationName) && stateInfo.normalizedTime >= _normalizedTime)
                {
                    NeedTransit = true;
                }
            }
        }
    }
}

