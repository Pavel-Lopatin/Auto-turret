using Auto_turret.Code.GameRoot;
using Auto_turret.Code.ServiceLocatorSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Auto_turret.Code.Services
{
    public class LevelManager : MonoBehaviour, IServiceLocator
    {
        [SerializeField] private float _loadingTime;

        private LoadingScreen _loadingScreen;
        private Coroutines _coroutines;
        private PauseManager _pauseManager;

        private bool _inRestartingProcess = false;

        public void Init()
        {
            _loadingScreen = ServiceLocator.Instance.Get<LoadingScreen>();
            _coroutines = ServiceLocator.Instance.Get<Coroutines>();
            _pauseManager = ServiceLocator.Instance.Get<PauseManager>();

            _pauseManager.Init();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"Scene {scene.name} loaded in mode {mode}");

#if UNITY_EDITOR
            // exclude mode 4 after start game in editor
            // https://discussions.unity.com/t/solved-problem-with-unitys-scene-management/624943
            if (mode > LoadSceneMode.Additive)
            {
                Debug.Log("Checked wrong scene mode");
                return;
            }
#endif

            if (scene.buildIndex == Scenes.MAIN_SCENE)
                _coroutines.StartCoroutine(LoadingGameplayCoroutine());
        }

        private IEnumerator LoadingGameplayCoroutine()
        {
            yield return new WaitForSeconds(_loadingTime);
            StartGameplay();
        }

        private void StartGameplay()
        {
            _loadingScreen.Hide();
            _pauseManager.TurnOn();
            _inRestartingProcess = false;

            Debug.Log("Gameplay started!");
        }

        private void RestartGameplay()
        {
            _loadingScreen.Show();
            _pauseManager.TurnOff();
            _coroutines.StartCoroutine(ReloadGameplayScene());
            Debug.Log("Gameplay finished!");
        }

        private IEnumerator ReloadGameplayScene()
        {
            _inRestartingProcess = true;
            yield return LoadScene(Scenes.BOOT);
            yield return LoadScene(Scenes.MAIN_SCENE);
        }

        private IEnumerator LoadScene(int sceneIndex)
        {
            yield return SceneManager.LoadSceneAsync(sceneIndex);
        }

        public void TryToRestartGameplay()
        {
            if (!_inRestartingProcess)
                RestartGameplay();
        }

        public void ExitGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }


    }
}
