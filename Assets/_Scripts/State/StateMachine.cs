using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Scripts.State
{
    public class StateMachine {
        private readonly List<Transition> _anyStateTransitions = new(); 
        private readonly Dictionary<State, List<Transition>> _stateToTransitions = new(); 
 
        public State CurrentState { get; private set; } 
        private List<Transition> _currentTransitions;

        // Use this while setting up your state machine
        public void AddTransition(Transition newTransition) 
        { 
            if (newTransition.Origin == null) 
            { 
                _anyStateTransitions.Add(newTransition); 
                return; 
            } 
 
            if (!_stateToTransitions.ContainsKey(newTransition.Origin)) 
            { 
                _stateToTransitions.Add(newTransition.Origin, new List<Transition>()); 
            } 
 
            _stateToTransitions[newTransition.Origin].Add(newTransition); 
        } 
 
        public void SetState(State state) 
        { 
            CurrentState = state; 
 
            CurrentState.OnEnter(); 
            _currentTransitions = new List<Transition>(); 
            _currentTransitions.AddRange(_anyStateTransitions); 
 
            if (!_stateToTransitions.ContainsKey(CurrentState)) return; 
 
            _currentTransitions.AddRange(_stateToTransitions[CurrentState]); 
        } 
 
        public void Tick() 
        { 
            if (CurrentState == null) return; 

            var transitionsThatSatisfy = _currentTransitions.Where(transition => transition.Predicate()).ToArray(); 
 
            if (transitionsThatSatisfy.Length == 0) 
            {
                CurrentState.OnTick();
                return;
            }
 
            if (transitionsThatSatisfy.Length > 1) 
            { 
                Debug.LogError("More than one transition satisfies"); 
                return;
            } 
 
            ChangeState(transitionsThatSatisfy[0]); 
        } 
 
        private void ChangeState(Transition transition) 
        { 
            CurrentState?.OnExit(); 
            SetState(transition.Target); 
        }
    }
}