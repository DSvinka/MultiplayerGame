using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Auth
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
            _authCanvas.enabled = true;
            _loginCanvas.enabled = false;
            _registerCanvas.enabled = false;
        }

        public void OpenLoginWindow()
        {
            _authCanvas.enabled = false;
            _loginCanvas.enabled = true;
            _registerCanvas.enabled = false;
        }

        public void OpenRegisterWindow()
        {
            _authCanvas.enabled = false;
            _loginCanvas.enabled = false;
            _registerCanvas.enabled = true;
        }
    }
}
