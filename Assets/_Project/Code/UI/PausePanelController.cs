using Auto_turret.Code.ServiceLocatorSystem;
using Auto_turret.Code.Services;
using UnityEngine;

namespace Auto_turret.Code.UI
{
    public class PausePanelController : MonoBehaviour 
    {
        public void ContinueGame()
        {
            ServiceLocator.Instance.Get<PauseManager>().TryToContinueGame();
        }

        public void RestartGame()
        {
            ServiceLocator.Instance.Get<LevelManager>().TryToRestartGameplay();
        }

        public void ExitGame()
        {
            ServiceLocator.Instance.Get<LevelManager>().ExitGame();
        }
    }
}


