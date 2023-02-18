using _Scripts.ScriptableObjects.Variables;

namespace _Scripts.State
{
    public class LevelTransitionState : State
    {
        private BooleanVariable _isInputActive;
        
        public LevelTransitionState(BooleanVariable isInputActive)
        {
            _isInputActive = isInputActive;
        }

        public override void OnEnter()
        {
            _isInputActive.Value = false;
        }
    }
}