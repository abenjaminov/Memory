using _Scripts.Game;
using _Scripts.ScriptableObjects.Setup;

namespace _Scripts.State
{
    public class LoadLevelState: State
    {
        private LevelController _levelController;
        private LevelInfo _levelInfo;

        public bool IsLevelLoaded;
        
        public LoadLevelState(LevelController levelController)
        {
            _levelController = levelController;
        }
        
        public void SetLevelToLoad(LevelInfo levelInfo)
        {
            _levelInfo = levelInfo;
        }
        
        public override void OnEnter()
        {
            _levelController.SetupLevel(_levelInfo);

            IsLevelLoaded = true;
        }

        public override void OnExit()
        {
            IsLevelLoaded = false;
        }
    }
}