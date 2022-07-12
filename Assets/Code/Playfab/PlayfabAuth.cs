using System;
using Code.Shared.Constants;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Code.Playfab
{
    public class PlayfabAuth : MonoBehaviour
    {
        public event Action<LoginResult> OnLoginSuccess;
        public event Action<PlayFabError> OnLoginFailed;
        
        /// <summary>
        /// Авторизует пользователя в Playfab в качестве гостя
        /// </summary>
        public void LoginAsGuest()
        {
            if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            {
                PlayFabSettings.staticSettings.TitleId = PlayfabConstants.DefaultTitleId;
            }

            var request = new LoginWithCustomIDRequest()
            {
                CustomId = PlayfabConstants.DefaultLoginCustomId,
                CreateAccount = true
            };
            
            PlayFabClientAPI.LoginWithCustomID(request, OnLogin, OnLoginError);
        }

        private void OnLogin(LoginResult loginResult)
        {
            DLogger.Debug(nameof(PlayfabAuth), nameof(OnLogin), "Login Complete!");
            OnLoginSuccess?.Invoke(loginResult);
        }

        private void OnLoginError(PlayFabError playFabError)
        {
            var errorMessage = playFabError.GenerateErrorReport();
            DLogger.Error(nameof(PlayfabAuth), nameof(OnLoginError), $"Login Error: {errorMessage}");
            OnLoginFailed?.Invoke(playFabError);
        }
    }
}
