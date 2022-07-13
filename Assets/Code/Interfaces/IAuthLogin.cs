using System;
using PlayFab;
using PlayFab.ClientModels;

namespace Code.Interfaces
{
    public interface IAuthLogin
    {
        event Action<LoginResult> OnLoginSuccess;
        event Action<PlayFabError> OnLoginFailed;

        /// <summary>
        /// Авторизует пользователя в Playfab по логину и паролю
        /// </summary>
        public void Login(string username, string password);
        
        /// <summary>
        /// Авторизует пользователя в Playfab в качестве гостя
        /// </summary>
        public void LoginAsGuest();
    }
}