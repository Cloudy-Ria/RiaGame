using UnityEngine;

namespace StateMachine
{
    /// <summary>
    /// Вспомогательный класс для сброса триггеров аниматора
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class ResetTrigger : MonoBehaviour
    {
        [SerializeField] private string _triggerName;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void ResetTriggerName()
        {
            if (_animator != null && !string.IsNullOrEmpty(_triggerName))
            {
                _animator.ResetTrigger(_triggerName);
            }
        }
    }
}

