using Auto_turret.Code.ServiceLocatorSystem;
using System;
using UnityEngine;

namespace Auto_turret.Code.Services
{
    public class PauseManager : MonoBehaviour, IServiceLocator
    {
        [SerializeField] private GameObject _pauseCanvas;

        [SerializeField] private bool _isActive;
        [SerializeField] private bool _stopDeltaTime;
        [SerializeField] private bool _detectEscapeButton = false;
        [SerializeField] private bool _isGamePaused;

        public event Action OnGamePaused;
        public event Action OnGameContinued;

        public void Init()
        {
            _isGamePaused = false;
            ShowPausePanel(_isGamePaused);
        }

        public void TurnOn()
        {
            _isActive = true;
            _detectEscapeButton = true;
            _isGamePaused = false;
            ShowPausePanel(_isGamePaused);
        }

        public void TurnOff()
        {
            _isActive = false;
            _detectEscapeButton = false;
            _isGamePaused = false;
            ShowPausePanel(_isGamePaused);
        }

        public void TryToContinueGame()
        {
            if (_isActive && _isGamePaused)
                ContinueGame();
        }

        private void PauseGame()
        {
            Debug.Log("Pause Manager started pause");
            _isGamePaused = true;
            OnGamePaused?.Invoke();
            ShowPausePanel(_isGamePaused);
        }

        public void ContinueGame()
        {
            Debug.Log("Pause Manager finished pause");
            _isGamePaused = false;
            OnGameContinued?.Invoke();
            ShowPausePanel(_isGamePaused);
        }

        private void ShowPausePanel(bool state)
        {
            _pauseCanvas.SetActive(state);
        }

        private void Update()
        {
            if (!_detectEscapeButton) return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!_isGamePaused) PauseGame();
                else ContinueGame();
            }
        }
    }
}