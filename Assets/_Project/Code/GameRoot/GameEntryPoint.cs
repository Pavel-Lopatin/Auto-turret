using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using Auto_turret.Code.ServiceLocatorSystem;
using Auto_turret.Code.Services;

namespace Auto_turret.Code.GameRoot
{
    public class GameEntryPoint
    {
        private static GameEntryPoint _instance;
        private LevelManager _levelManager;
        private LoadingScreen _loadingScreen;
        private Coroutines _coroutines;

        // boot point
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void RunGame()
        {
            _instance = new GameEntryPoint();
            _instance.SelectStartScene();
        }

        private GameEntryPoint()
        {
            ServiceLocator.Init();

            _coroutines = new GameObject("Coroutines").AddComponent<Coroutines>();
            Object.DontDestroyOnLoad(_coroutines);

            var loadingScreenPrefab = Resources.Load<LoadingScreen>("LoadingScreen");
            _loadingScreen = Object.Instantiate(loadingScreenPrefab);
            Object.DontDestroyOnLoad(_loadingScreen);

            var levelManagerPrefab = Resources.Load<LevelManager>("LevelManager");
            _levelManager = Object.Instantiate(levelManagerPrefab);
            _levelManager.Init();
            Object.DontDestroyOnLoad(_levelManager);
        }

        private void SelectStartScene()
        {
#if UNITY_EDITOR
            var sceneIndex = SceneManager.GetActiveScene().buildIndex;

            if (sceneIndex == Scenes.MAIN_MENU)
            {
                _coroutines.StartCoroutine(LoadAndStartMainMenu());
                return;
            }

            if (sceneIndex > Scenes.MAIN_MENU)
            {
                _coroutines.StartCoroutine(LoadAndStartGameplay(sceneIndex));
                return;
            }

            if (sceneIndex != Scenes.BOOT)
            {
                return;
            }
#endif

            _coroutines.StartCoroutine(LoadAndStartMainMenu());
        }

        private IEnumerator LoadAndStartMainMenu()
        {
            _loadingScreen.Show();

            yield return LoadScene(Scenes.BOOT);
            yield return LoadScene(Scenes.MAIN_MENU);

            yield return new WaitForSeconds(1f);

            _loadingScreen.Hide();
        }

        private IEnumerator LoadAndStartGameplay(int buildIndex)
        {
            _loadingScreen.Show();

            yield return LoadScene(Scenes.BOOT);
            yield return LoadScene(buildIndex);

            yield return new WaitForSeconds(1f);

            _loadingScreen.Hide();
        }

        private IEnumerator LoadScene(int sceneIndex)
        {
            yield return SceneManager.LoadSceneAsync(sceneIndex);
        }
    }
}