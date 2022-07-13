using System;
using PlayFab;
using PlayFab.ClientModels;

namespace Code.Interfaces
{
    public interface IAuthRegister
    {
        event Action<RegisterPlayFabUserResult> OnRegisterSuccess;
        event Action<PlayFabError> OnRegisterFailed;

        /// <summary>
        /// Регистрирует пользователя в Playfab
        /// </summary>
        void Register(string username, string password, string email);
    }
}