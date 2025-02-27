using Auto_turret.Code.ServiceLocatorSystem;
using UnityEngine;

namespace Auto_turret.Code.GameRoot
{
    public class LoadingScreen : MonoBehaviour, IServiceLocator
    {
        [SerializeField] private GameObject _loadingScreenCanvas;

        private void Awake() => Hide();

        public void Show() => _loadingScreenCanvas.SetActive(true);

        public void Hide() => _loadingScreenCanvas.SetActive(false);  
    }
}