using UnityEngine;

namespace StateMachine
{
    /// <summary>
    /// Базовый класс для всех переходов между состояниями
    /// </summary>
    public abstract class Transition : MonoBehaviour
    {
        [SerializeField] protected State TargetState;

        public State TargetStateProperty => TargetState;
        public bool NeedTransit { get; protected set; }

        /// <summary>
        /// Получает Transform игрока из родительского State
        /// </summary>
        protected Transform Player
        {
            get
            {
                State parentState = GetComponentInParent<State>();
                return parentState != null ? parentState.Player : null;
            }
        }

        private void OnEnable() => NeedTransit = false;
    }
}

