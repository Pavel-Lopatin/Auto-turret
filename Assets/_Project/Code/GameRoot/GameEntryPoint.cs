using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using Auto_turret.Code.ServiceLocatorSystem;
using Auto_turret.Code.Services;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

namespace Auto_turret.Code.GameRoot
{
    public class GameEntryPoint
    {
        private static GameEntryPoint _instance;
        private Coroutines _coroutines;
        private LoadingScreen _loadingScreen;
        private PauseManager _pauseManager;
        private LevelManager _levelManager;


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RunGame()
        {
            _instance = new GameEntryPoint();
            _instance.SelectStartScene();
        }

        private GameEntryPoint()
        {
            ServiceLocator.Init();

            var eventSystem = new GameObject("EventSystem").AddComponent<EventSystem>();
            eventSystem.gameObject.AddComponent<InputSystemUIInputModule>();
            Object.DontDestroyOnLoad(eventSystem); 

            _coroutines = new GameObject("Coroutines").AddComponent<Coroutines>();
            Object.DontDestroyOnLoad(_coroutines);
            ServiceLocator.Instance.Register(_coroutines);

            var loadingScreenPrefab = Resources.Load<LoadingScreen>("LoadingScreen");
            _loadingScreen = Object.Instantiate(loadingScreenPrefab);
            Object.DontDestroyOnLoad(_loadingScreen);
            ServiceLocator.Instance.Register(_loadingScreen);

            var pauseManagerPrefab = Resources.Load<PauseManager>("PauseManager");
            _pauseManager = Object.Instantiate(pauseManagerPrefab);
            Object.DontDestroyOnLoad(_pauseManager);
            ServiceLocator.Instance.Register(_pauseManager);

            var levelManagerPrefab = Resources.Load<LevelManager>("LevelManager");
            _levelManager = Object.Instantiate(levelManagerPrefab);
            Object.DontDestroyOnLoad(_levelManager);
            ServiceLocator.Instance.Register(_levelManager);
            _levelManager.Init();
        }

        private void SelectStartScene()
        {
            // TO DO 
            // it is possible to add other scenes

            _coroutines.StartCoroutine(LoadGameplay());
        }

        private IEnumerator LoadGameplay()
        {
            _loadingScreen.Show();

            yield return LoadScene(Scenes.BOOT);
            yield return LoadScene(Scenes.MAIN_SCENE);
        }

        private IEnumerator LoadScene(int sceneIndex)
        {
            yield return SceneManager.LoadSceneAsync(sceneIndex);
        }
    }
}