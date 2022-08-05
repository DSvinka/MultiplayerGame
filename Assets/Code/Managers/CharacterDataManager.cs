using System;
using System.Collections.Generic;
using Code.Models.UserData;
using Code.Utils;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Code.Managers
{
    public class CharacterDataManager: MonoBehaviour
    {
        private CharacterDataModel _characterDataModel;

        /// <summary>
        /// Для изменения данных на сервере используйте эту модель.
        /// При изменении её полей, автоматически изменения отправляются на сервер.
        /// Вам не нужно думать о запросах на сервере, модель сделает это за вас :D
        /// </summary>
        public CharacterDataModel CharacterDataModel => _characterDataModel;

        private void Awake()
        {
            _characterDataModel = new CharacterDataModel();
            _characterDataModel.OnDataUpdated += SetCharacterData;
        }

        private void OnDestroy()
        {
            _characterDataModel.OnDataUpdated -= SetCharacterData;
            _characterDataModel.Dispose();
        }

        private void SetCharacterData(string key, string value, bool sendChangeToServer)
        {
            if (!sendChangeToServer)
                return;
            
            var updateRequest = new UpdateCharacterDataRequest()
            {
                CharacterId = CharactersManager.CharacterId,
                Data = _characterDataModel.ToDict()
            };
            
            PlayFabClientAPI.UpdateCharacterData(updateRequest, OnUpdateSuccess, OnUpdateFailed);
        }

        public void FetchCharacterData()
        {
            var getRequest = new GetCharacterDataRequest()
            {
                PlayFabId = AuthManager.PlayFabId,
                CharacterId = CharactersManager.CharacterId,
            };
            
            PlayFabClientAPI.GetCharacterData(getRequest, OnFetchSuccess, OnFetchFailed);
        }
        
        #region SetCharacterData Callback
        
        private void OnUpdateSuccess(UpdateCharacterDataResult updateResult)
        {
            DLogger.Debug(GetType(), nameof(OnUpdateSuccess), 
                "Update Character Data - Success");
        }
        
        private void OnUpdateFailed(PlayFabError playFabError)
        {
            var errorMessage = playFabError.GenerateErrorReport();
            DLogger.Debug(GetType(), nameof(OnUpdateFailed), 
                $"Update Character Data - Failed: {errorMessage}");
        }

        #endregion
        
        #region FetchCharacterData Callback
        
        private void OnFetchSuccess(GetCharacterDataResult getResult)
        {
            DLogger.Debug(GetType(), nameof(OnFetchSuccess), 
                "Fetch Character Data - Success");
            if (getResult.Data.Count != 0)
                _characterDataModel.LoadFromDict(getResult.Data);
            else
            {
                _characterDataModel.Damage = 5;
                _characterDataModel.Health = 100;
                _characterDataModel.Level = 0;
            }
        }
        
        private void OnFetchFailed(PlayFabError playFabError)
        {
            var errorMessage = playFabError.GenerateErrorReport();
            DLogger.Debug(GetType(), nameof(OnFetchFailed), 
                $"Fetch Character Data - Failed: {errorMessage}");
        }
        
        #endregion
    }
}