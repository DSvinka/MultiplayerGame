using System;
using System.Collections.Generic;
using Code.Utils;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Code.Managers
{
    public class CharactersManager: MonoBehaviour
    {
        public event Action<List<CharacterResult>> OnGetCharacters;
        public event Action<string, Dictionary<string, int>> OnGetCharacterData;
        
        public event Action<string> OnCreateCharacter;

        [Header("Managers")]
        [SerializeField] private CharacterDataManager _characterDataManager;

        public static string CharacterId { get; private set; }
        public static string CharacterName { get; private set; }

        public void ChoiceCharacter(CharacterResult character)
        {
            CharacterId = character.CharacterId;
            CharacterName = character.CharacterName;
            
            _characterDataManager.FetchCharacterData();
        }

        #region GetCharacters

        public void GetCharacters()
        {
            PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest()
            {
                
            }, OnGetCharactersSuccess, OnGetCharactersFailed);
        }

        private void OnGetCharactersSuccess(ListUsersCharactersResult result)
        {
            DLogger.Debug(GetType(), nameof(OnGetCharactersSuccess), 
                $"Get Characters Success! Characters Count: {result.Characters.Count}");
            OnGetCharacters?.Invoke(result.Characters);
        }

        private void OnGetCharactersFailed(PlayFabError error)
        {
            var errorMessage = error.GenerateErrorReport();
            DLogger.Error(GetType(), nameof(OnGetCharactersFailed),
                $"Get Characters Failed! Error: {errorMessage}");
        }

        #endregion

        #region GetCharacterStatistics

        public void GetCharacterData(string characterId)
        {
            PlayFabClientAPI.GetCharacterData(new GetCharacterDataRequest()
            {
                CharacterId = characterId
            }, OnGetCharacterDataSuccess, OnGetCharacterDataError);   
        }

        private void OnGetCharacterDataSuccess(GetCharacterDataResult result)
        {
            DLogger.Debug(GetType(), nameof(OnGetCharacterDataSuccess), 
                $"Get Character Data Success");
            OnGetCharacterData?.Invoke(result.CharacterId, result.Data);
        }

        private void OnGetCharacterDataError(PlayFabError error)
        {
            var errorMessage = error.GenerateErrorReport();
            DLogger.Error(GetType(), nameof(OnGetCharacterDataError), 
                $"Get Character Data Error: {errorMessage}");
        }

        #endregion

        #region CreateCharacter

        public void CreateCharacter(string characterName)
        {
            PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest()
            {
                CharacterName = characterName,
                ItemId = "character_item_id"
            }, OnCreateCharacterSuccess, OnCreateCharacterError);
        }

        private void OnCreateCharacterSuccess(GrantCharacterToUserResult result)
        {
            DLogger.Debug(GetType(), nameof(OnGetCharacterDataSuccess), 
                $"Create Character Success");
            OnCreateCharacter?.Invoke(result.CharacterId);
        }
        
        private void OnCreateCharacterError(PlayFabError error)
        {
            var errorMessage = error.GenerateErrorReport();
            DLogger.Error(GetType(), nameof(OnCreateCharacterError), 
                $"Create Character Error: {errorMessage}");
        }
        
        #endregion
    }
}