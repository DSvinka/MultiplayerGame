using Code.Controllers;
using Code.Managers;
using Code.Views.UI.Base;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Views.UI.Auth
{
    public class AuthRegisterWindowView: AuthWindowBase
    {
        [SerializeField] private InputField _emailInput;

        [Header("Кнопки")]
        [SerializeField] private Button _registerButton;
        [SerializeField] private Button _closeButton;
        
        [Header("Индикаторы")] 
        [SerializeField] private GameObject _loadingIndicator;
        [SerializeField] private Text _errorText;
        
        [Header("Менеджеры")]
        [SerializeField] private AuthManager _authManager;
        [SerializeField] private AuthWindowsManager _authWindowsManager;
        
        private string _email;

        private void UpdateEmail(string email)
        {
            _email = email;
        }
        
        private void Register()
        {
            _errorText.gameObject.SetActive(false); 
            
            _loadingIndicator.SetActive(true);
            _authManager.Register(_username, _password, _email);
        }
        
        protected override void SubscribeToUI()
        {
            base.SubscribeToUI();
            
            _emailInput.onValueChanged.AddListener(UpdateEmail);
            
            _registerButton.onClick.AddListener(Register);
            _closeButton.onClick.AddListener(_authWindowsManager.OpenAuthWindow);
            
            _authManager.OnRegisterSuccess += OnRegisterSuccess;
            _authManager.OnRegisterFailed += OnRegisterFailed;
        }
        
        protected override void UnsubscribeFromUI()
        {
            base.UnsubscribeFromUI();
            
            _emailInput.onValueChanged.RemoveAllListeners();
            
            _registerButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();
            
            _authManager.OnRegisterSuccess -= OnRegisterSuccess;
            _authManager.OnRegisterFailed -= OnRegisterFailed;
        }
        
        private void OnRegisterSuccess(RegisterPlayFabUserResult registerResult)
        {
            _loadingIndicator.SetActive(false);
            ChangeSceneToLobby();
        }
        
        private void OnRegisterFailed(PlayFabError playFabError)
        {
            _loadingIndicator.SetActive(false);
            
            _errorText.gameObject.SetActive(true); 
            _errorText.text = playFabError.ErrorMessage;
        }
    }
}