using Auto_turret.Code.GameRoot;
using Auto_turret.Code.ServiceLocatorSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Auto_turret.Code.Services
{
    public class LevelManager : MonoBehaviour, IServiceLocator
    {
        private LoadingScreen _loadingScreen;
        private Coroutines _coroutines;

        public void Init()
        {
            ServiceLocator.Instance.Register(this);

            _loadingScreen = FindAnyObjectByType<LoadingScreen>();
            _coroutines = FindAnyObjectByType<Coroutines>();
        }

        /// <summary>
        /// This method checks which scene to load: Main Menu or next level, and loading it
        /// </summary>
        public void CheckSceneToLoad()
        {
            int totalSceneIndex = SceneManager.sceneCountInBuildSettings;
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            if (currentSceneIndex < totalSceneIndex)
            {
                currentSceneIndex += 1;
                _coroutines.StartCoroutine(LoadAndStartNextScene(currentSceneIndex));
            }
            else
            {
                _coroutines.StartCoroutine(LoadAndStartMainMenu());
            }
        }

        public void LoadFirstScene()
        {
            _coroutines.StartCoroutine(LoadAndStartFirstScene());
        }

        private IEnumerator LoadAndStartFirstScene()
        {
            _loadingScreen.Show();

            yield return LoadScene(Scenes.BOOT);
            yield return LoadScene(Scenes.LEVEL_1);

            yield return new WaitForSeconds(1f);

            _loadingScreen.Hide();
        }

        private IEnumerator LoadAndStartNextScene(int sceneIndex)
        {
            _loadingScreen.Show();

            yield return LoadScene(Scenes.BOOT);
            yield return LoadScene(sceneIndex);

            yield return new WaitForSeconds(1f);

            _loadingScreen.Hide();
        }

        private IEnumerator LoadAndStartMainMenu()
        {
            _loadingScreen.Show();

            yield return LoadScene(Scenes.BOOT);
            yield return LoadScene(Scenes.MAIN_MENU);

            yield return new WaitForSeconds(1f);

            _loadingScreen.Hide();
        }

        private IEnumerator LoadScene(int sceneIndex)
        {
            yield return SceneManager.LoadSceneAsync(sceneIndex);
        }
    }
}
