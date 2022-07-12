using UnityEngine;

namespace Code.Shared.Constants.UI
{
    public static class PlayfabMessages
    {
        public const string LoginSuccessText = "Вы успешно авторизировались!";
        public static readonly Color LoginSuccessColor = Color.green;

        public const string LoginFailedText = "Не удалось авторизироваться...";
        public static readonly Color LoginFailedColor = Color.red;
    }
}