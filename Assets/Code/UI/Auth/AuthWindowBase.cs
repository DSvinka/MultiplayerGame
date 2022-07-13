using Code.Shared.Constants;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.UI.Auth
{
    public abstract class AuthWindowBase: MonoBehaviour
    {
        [Header("Поля ввода данных")]
        [SerializeField] private InputField _usernameInput;
        [SerializeField] private InputField _passwordInput;

        protected string _username;
        protected string _password;

        private void Start()
        {
            SubscribeToUI();
        }

        private void OnDestroy()
        {
            UnsubscribeFromUI();
        }

        /// <summary>
        /// Вызывается в Start, служит для подписки на UI элементы (InputField, Button)
        /// </summary>
        protected virtual void SubscribeToUI()
        {
            _usernameInput.onValueChanged.AddListener(UpdateUsername);
            _passwordInput.onValueChanged.AddListener(UpdatePassword);
        }

        /// <summary>
        /// Вызывается в OnDestroy, служит для отписки от UI элементов (InputField, Button)
        /// </summary>
        protected virtual void UnsubscribeFromUI()
        {
            _usernameInput.onValueChanged.RemoveAllListeners();
            _passwordInput.onValueChanged.RemoveAllListeners();
        }

        protected void ChangeSceneToGame()
        {
            SceneManager.LoadScene((int) EScenesIndexes.Game);
        }
        
        private void UpdateUsername(string username)
        {
            _username = username;
        }
        
        private void UpdatePassword(string password)
        {
            _password = password;
        }

    }
}