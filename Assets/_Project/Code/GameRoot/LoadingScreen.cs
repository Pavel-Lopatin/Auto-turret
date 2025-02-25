using UnityEngine;

namespace Auto_turret.Code.GameRoot
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingScreenCanvas;

        private void Awake() => Hide();

        public void Show() => _loadingScreenCanvas.SetActive(true);

        public void Hide() => _loadingScreenCanvas.SetActive(false);  
    }
}