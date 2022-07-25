using System;
using Code.Interfaces.Managers;
using Code.Shared.Constants;
using Code.Utils;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Code.Managers
{
    public class AuthManager : MonoBehaviour, IAuthManager
    {
        #region Public Events

        public event Action OnLogout;
        
        public event Action<LoginResult> OnLoginSuccess;
        public event Action<PlayFabError> OnLoginFailed;

        public event Action<RegisterPlayFabUserResult> OnRegisterSuccess;
        public event Action<PlayFabError> OnRegisterFailed;

        #endregion

        public void Logout()
        {
            PlayFabClientAPI.ForgetAllCredentials();
            
            DLogger.Debug(GetType(), nameof(Logout), 
                "Logout Complete!");
            OnLogout?.Invoke();
        }

        #region Authentication

        public void Login(string username, string password)
        {
            var registerRequest = new LoginWithPlayFabRequest()
            {
                Username = username,
                Password = password,
            };
            
            PlayFabClientAPI.LoginWithPlayFab(registerRequest, OnLogin, OnLoginError);
        }

        public void LoginAsGuest()
        {
            if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            {
                PlayFabSettings.staticSettings.TitleId = PlayfabConstants.DefaultTitleId;
            }
            
            var needCreation = PlayerPrefs.HasKey(PlayerPrefsKeys.AuthGuestGuidKey);
            var id = PlayerPrefs.GetString(PlayerPrefsKeys.AuthGuestGuidKey, Guid.NewGuid().ToString());

            var loginRequest = new LoginWithCustomIDRequest()
            {
                CustomId = id,
                CreateAccount = !needCreation
            };
            
            PlayFabClientAPI.LoginWithCustomID(loginRequest,
                success =>
                {
                    PlayerPrefs.SetString(PlayerPrefsKeys.AuthGuestGuidKey, id);
                    OnLogin(success);
                }, OnLoginError);
        }
        
        private void OnLogin(LoginResult loginResult)
        {
            DLogger.Debug(GetType(), nameof(OnLogin), 
                "Login Complete!");
            OnLoginSuccess?.Invoke(loginResult);
        }

        private void OnLoginError(PlayFabError playFabError)
        {
            var errorMessage = playFabError.GenerateErrorReport();
            DLogger.Error(GetType(), nameof(OnLoginError), 
                $"Login Failed: {errorMessage}");
            OnLoginFailed?.Invoke(playFabError);
        }

        #endregion
        
        
        #region Registration

        public void Register(string username, string password, string email)
        {
            var registerRequest = new RegisterPlayFabUserRequest()
            {
                Email = email,
                Username = username, 
                Password = password,
            };
            
            PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegister, OnRegisterError);
        }

        
        private void OnRegister(RegisterPlayFabUserResult registerResult)
        {
            DLogger.Debug(GetType(), nameof(OnRegister), 
                $"Register Complete! [Username={registerResult.Username}]");
            OnRegisterSuccess?.Invoke(registerResult);
        }
        
        private void OnRegisterError(PlayFabError playFabError)
        {
            var errorMessage = playFabError.GenerateErrorReport();
            DLogger.Error(GetType(), nameof(OnRegisterError), 
                $"Register Failed: {errorMessage}");
            OnRegisterFailed?.Invoke(playFabError);
        }

        #endregion
    }
}
