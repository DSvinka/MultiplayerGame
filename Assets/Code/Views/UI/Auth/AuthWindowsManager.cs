using UnityEngine;
using UnityEngine.UI;

namespace Code.Views.UI.Auth
{
    public class AuthWindowsManager : MonoBehaviour
    {
        [Header("Кнопки")]
        [SerializeField] private Button _loginButton;
        [SerializeField] private Button _registerButton;

        [Header("Канвас")]
        [SerializeField] private Canvas _authCanvas;
        [SerializeField] private Canvas _loginCanvas;
        [SerializeField] private Canvas _registerCanvas;

        private void Start()
        {
            OpenAuthWindow();
            
            _loginButton.onClick.AddListener(OpenLoginWindow);
            _registerButton.onClick.AddListener(OpenRegisterWindow);
        }

        private void OnDestroy()
        {
            _loginButton.onClick.RemoveAllListeners();
            _registerButton.onClick.RemoveAllListeners();
        }

        public void OpenAuthWindow()
        {
            _authCanvas.gameObject.SetActive(true);
            _loginCanvas.gameObject.SetActive(false);
            _registerCanvas.gameObject.SetActive(false);
        }

        public void OpenLoginWindow()
        {
            _authCanvas.gameObject.SetActive(false);
            _loginCanvas.gameObject.SetActive(true);
            _registerCanvas.gameObject.SetActive(false);
        }

        public void OpenRegisterWindow()
        {
            _authCanvas.gameObject.SetActive(false);
            _loginCanvas.gameObject.SetActive(false);
            _registerCanvas.gameObject.SetActive(true);
        }
    }
}
