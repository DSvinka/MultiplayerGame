using System;

namespace Code.Interfaces
{
    public interface IAuthLogout
    {
        event Action OnLogout;
        
        /// <summary>
        /// Забывает аккаунт пользователя (выход из аккаунта)
        /// </summary>
        public void Logout();
    }
}