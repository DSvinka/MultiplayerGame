using Code.Managers;
using Code.Shared.Constants;
using Code.Utils;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Code.Controllers
{
    public class AccountController : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private Button _logoutButton;
        
        [Header("Managers")]
        [SerializeField] private AuthManager _authManager;

        #region Unity Events
        
        private void Start()
        {
            var request = new GetAccountInfoRequest();
            PlayFabClientAPI.GetAccountInfo(request, OnGetAccount, OnGetAccountError);
            
            _logoutButton.onClick.AddListener(OnLogoutClick);
        }

        private void OnDestroy()
        {
            _logoutButton.onClick.RemoveAllListeners();
        }
        
        #endregion

        private void OnLogoutClick()
        {
            _authManager.Logout();
            SceneManager.LoadScene((int) EScenesIndexes.Auth);
        }

        private void OnGetAccount(GetAccountInfoResult result)
        {
            _titleText.text = $"User: {result.AccountInfo.PlayFabId}\n" +
                              $"Created At: {result.AccountInfo.Created}\n" +
                              $"First Login: {result.AccountInfo.TitleInfo.FirstLogin}\n" +
                              $"Is Banned: {result.AccountInfo.TitleInfo.isBanned}";
        }

        private void OnGetAccountError(PlayFabError playFabError)
        {
            var errorMessage = playFabError.GenerateErrorReport();
            DLogger.Error(GetType(), nameof(OnGetAccountError), 
                $"GetAccount Failed: {errorMessage}");
        }

    }
}
