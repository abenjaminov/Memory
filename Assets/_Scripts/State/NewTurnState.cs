using _Scripts.ScriptableObjects.Variables;

namespace _Scripts.State
{
    public class NewTurnState: State
    {
        private BooleanVariable _isInputActive;
        
        public NewTurnState(BooleanVariable isInputActive)
        {
            _isInputActive = isInputActive;
        }
        
        public override void OnEnter()
        {
            _isInputActive.Value = true;
        }
    }
}