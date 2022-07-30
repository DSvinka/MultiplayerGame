using System;
using System.Collections.Generic;
using Code.Models.UserData;
using Code.Utils;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Code.Managers
{
    public class UserDataManager: MonoBehaviour
    {
        [SerializeField] private AuthManager _authManager;
        
        private UserDataModel _userDataModel;

        /// <summary>
        /// Для изменения данных на сервере используйте эту модель.
        /// При изменении её полей, автоматически изменения отправляются на сервер.
        /// Вам не нужно думать о запросах на сервере, модель сделает это за вас :D
        /// </summary>
        public UserDataModel UserDataModel => _userDataModel;

        private void Awake()
        {
            _userDataModel = new UserDataModel();
            _userDataModel.OnDataUpdated += SetUserData;
        }

        private void Start()
        {
            _authManager.OnLoginSuccess += OnLogin;
        }

        private void OnDestroy()
        {
            _authManager.OnLoginSuccess -= OnLogin;
            
            _userDataModel.OnDataUpdated -= SetUserData;
            _userDataModel.Dispose();
        }
        
        private void OnLogin(LoginResult obj)
        {
            FetchUserData();
        }

        private void SetUserData(string key, string value, bool sendChangeToServer)
        {
            if (!sendChangeToServer)
                return;
            
            var updateUserDataRequest = new UpdateUserDataRequest()
            {
                Data = _userDataModel.ToDict()
            };
            
            PlayFabClientAPI.UpdateUserData(updateUserDataRequest, OnUpdateUserDataSuccess, OnUpdateUserDataFailed);
        }

        public void FetchUserData()
        {
            var getUserDataRequest = new GetUserDataRequest()
            {
                PlayFabId = AuthManager.PlayFabId
            };
            
            PlayFabClientAPI.GetUserData(getUserDataRequest, OnFetchUserDataSuccess, OnFetchUserDataFailed);
        }
        
        #region SetUserData Callback
        
        private void OnUpdateUserDataSuccess(UpdateUserDataResult updateUserDataResult)
        {
            DLogger.Debug(GetType(), nameof(OnUpdateUserDataSuccess), 
                "Update User Data - Success");
        }
        
        private void OnUpdateUserDataFailed(PlayFabError playFabError)
        {
            var errorMessage = playFabError.GenerateErrorReport();
            DLogger.Debug(GetType(), nameof(OnUpdateUserDataFailed), 
                $"Update User Data - Failed: {errorMessage}");
        }

        #endregion
        
        #region FetchUserData Callback
        
        private void OnFetchUserDataSuccess(GetUserDataResult getUserDataResult)
        {
            DLogger.Debug(GetType(), nameof(OnFetchUserDataSuccess), 
                "Fetch User Data - Success");
            _userDataModel.LoadFromDict(getUserDataResult.Data);
        }
        
        private void OnFetchUserDataFailed(PlayFabError playFabError)
        {
            var errorMessage = playFabError.GenerateErrorReport();
            DLogger.Debug(GetType(), nameof(OnFetchUserDataFailed), 
                $"Fetch User Data - Failed: {errorMessage}");
        }
        
        #endregion
    }
}