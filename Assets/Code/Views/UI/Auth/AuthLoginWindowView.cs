using Code.Controllers;
using Code.Managers;
using Code.Views.UI.Base;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Views.UI.Auth
{
    public class AuthLoginWindowView: AuthWindowBase
    {
        [Header("Кнопки")]
        [SerializeField] private Button _loginButton;
        [SerializeField] private Button _closeButton;

        [Header("Индикаторы")] 
        [SerializeField] private GameObject _loadingIndicator;
        [SerializeField] private Text _errorText;
        
        [Header("Менеджеры")]
        [SerializeField] private AuthManager _authManager;
        [SerializeField] private AuthWindowsManager _authWindowsManager;

        private void Login()
        {
            _errorText.gameObject.SetActive(false); 
            
            _loadingIndicator.SetActive(true);
            _authManager.Login(_username, _password);
        }

        protected override void SubscribeToUI()
        {
            base.SubscribeToUI();
            
            _loginButton.onClick.AddListener(Login);
            _closeButton.onClick.AddListener(_authWindowsManager.OpenAuthWindow);
            
            _authManager.OnLoginSuccess += OnLoginSuccess;
            _authManager.OnLoginFailed += OnLoginFailed;
        }
        
        protected override void UnsubscribeFromUI()
        {
            base.UnsubscribeFromUI();

            _loginButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();
            
            _authManager.OnLoginSuccess -= OnLoginSuccess;
            _authManager.OnLoginFailed -= OnLoginFailed;
        }
        
        private void OnLoginSuccess(LoginResult loginResult)
        {
            _loadingIndicator.SetActive(false);
            ChangeSceneToLobby();
        }
        
        private void OnLoginFailed(PlayFabError playFabError)
        {
            _loadingIndicator.SetActive(false);
            
            _errorText.gameObject.SetActive(true); 
            _errorText.text = playFabError.ErrorMessage;
        }
    }
}