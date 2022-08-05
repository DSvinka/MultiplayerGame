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

        [Header("Buttons")]
        [SerializeField] private Button _registerButton;
        [SerializeField] private Button _closeButton;
        
        [Header("Indicators")] 
        [SerializeField] private GameObject _loadingIndicator;
        [SerializeField] private Text _errorText;
        
        [Header("Managers")]
        [SerializeField] private AuthManager _authManager;
        
        [Header("Controllers")]
        [SerializeField] private AuthWindowsController _authWindowsController;
        
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
            _closeButton.onClick.AddListener(_authWindowsController.OpenAuthWindow);
            
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
            ChangeSceneToCharacters();
        }
        
        private void OnRegisterFailed(PlayFabError playFabError)
        {
            _loadingIndicator.SetActive(false);
            
            _errorText.gameObject.SetActive(true); 
            _errorText.text = playFabError.ErrorMessage;
        }
    }
}