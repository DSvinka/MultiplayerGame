using UnityEngine;
using UnityEngine.UI;

namespace Code.Controllers
{
    public class AuthWindowsController : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button _loginButton;
        [SerializeField] private Button _registerButton;

        [Header("Windows")]
        [SerializeField] private GameObject _authWindow;
        [SerializeField] private GameObject _loginWindow;
        [SerializeField] private GameObject _registerWindow;

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
            _authWindow.gameObject.SetActive(true);
            _loginWindow.gameObject.SetActive(false);
            _registerWindow.gameObject.SetActive(false);
        }

        public void OpenLoginWindow()
        {
            _authWindow.gameObject.SetActive(false);
            _loginWindow.gameObject.SetActive(true);
            _registerWindow.gameObject.SetActive(false);
        }

        public void OpenRegisterWindow()
        {
            _authWindow.gameObject.SetActive(false);
            _loginWindow.gameObject.SetActive(false);
            _registerWindow.gameObject.SetActive(true);
        }
    }
}
