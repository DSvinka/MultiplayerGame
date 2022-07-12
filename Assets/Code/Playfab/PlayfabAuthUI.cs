using Code.Shared.Constants.UI;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Playfab
{
    public class PlayfabAuthUI : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Button _loginAsGuestButton;
        [SerializeField] private TMP_Text _resultText;
        
        [Header("Objects")]
        [SerializeField] private PlayfabAuth _playfabAuth;

        private void Start()
        {
            _loginAsGuestButton.onClick.AddListener(OnLoginAsGuestButton);
            _playfabAuth.OnLoginSuccess += AuthSuccess;
            _playfabAuth.OnLoginFailed += AuthFailed;

            ChangeResultText(display: false);
            ChangeLoginButton(false);
        }

        private void OnDestroy()
        {
            _loginAsGuestButton.onClick.RemoveAllListeners();
            _playfabAuth.OnLoginSuccess -= AuthSuccess;
            _playfabAuth.OnLoginFailed -= AuthFailed;
        }

        /// <summary>
        /// Редактирование сообщения о результате авторизации
        /// </summary>
        /// <param name="text">Текст сообщения</param>
        /// <param name="color">Цвет текста</param>
        /// <param name="display">Отображение текста</param>
        private void ChangeResultText(string text = "", Color color = default, bool display = true)
        {
            _resultText.text = text;
            _resultText.color = color;
            _resultText.gameObject.SetActive(display);
        }
        
        /// <summary>
        /// Редактирование кнопки
        /// </summary>
        /// <param name="disabled">Выключена ли кнопка</param>
        private void ChangeLoginButton(bool disabled)
        {
            _loginAsGuestButton.interactable = !disabled;
        }


        private void OnLoginAsGuestButton()
        {
            _playfabAuth.LoginAsGuest();
        }

        private void AuthSuccess(LoginResult loginResult)
        {
            ChangeResultText(PlayfabMessages.LoginSuccessText, PlayfabMessages.LoginSuccessColor);
            ChangeLoginButton(true);
        }

        private void AuthFailed(PlayFabError playFabError)
        {
            ChangeResultText(PlayfabMessages.LoginFailedText, PlayfabMessages.LoginFailedColor);
            ChangeLoginButton(false);
        }
    }
}
