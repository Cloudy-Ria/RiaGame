using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    /// <summary>
    /// Базовый класс для всех состояний в State Machine
    /// </summary>
    public abstract class State : MonoBehaviour
    {
        [SerializeField] private List<Transition> _transitions;

        public Transform Player { get; private set; }

        public void Enter()
        {
            if (enabled == false)
            {
                enabled = true;

                foreach (var transition in _transitions)
                {
                    transition.enabled = true;
                }
            }
        }

        public void Exit()
        {
            if (enabled)
            {
                foreach (var transition in _transitions)
                    transition.enabled = false;
            }

            enabled = false;
        }

        public State GetNextState()
        {
            foreach (var transition in _transitions)
            {
                if (transition.NeedTransit)
                    return transition.TargetStateProperty;
            }

            return null;
        }

        public void SetPlayerTransform(Transform playerTransform) => Player = playerTransform;
    }
}

