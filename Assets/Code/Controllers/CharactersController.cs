using System;
using System.Collections.Generic;
using System.Linq;
using Code.Lesson;
using Code.Managers;
using Code.Shared.Constants;
using Code.Utils;
using Code.Views.UI;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.Controllers
{
    public class CharactersController: MonoBehaviour
    {
        [Header("Views")]
        [SerializeField] private CharactersView _charactersView;

        [Header("Managers")]
        [SerializeField] private CharactersManager _charactersManager;

        private Dictionary<SlotCharacterView, CharacterResult> _characters;

        #region Unity Callbacks

        private void Start()
        {
            _characters = new Dictionary<SlotCharacterView, CharacterResult>();
            
            foreach (var slotCharacterView in _charactersView.SlotCharacterViews)
            {
                slotCharacterView.OnCreateCharacterSubmit += OnOpenCreateCharacterSubmit;
                slotCharacterView.OnChoiceCharacterSubmit += OnChoiceCharacterSubmit;
            }
            
            _charactersView.OnCharacterCreateSubmit += OnCreateCharacterSubmit;
            _charactersManager.OnGetCharacterData += OnGetCharacterData;
            _charactersManager.OnGetCharacters += ShowCharactersInSlots;
            _charactersManager.OnCreateCharacter += OnCreateCharacter;
            
            _charactersManager.GetCharacters();
        }
        
        private void OnDestroy()
        {
            _characters.Clear();
            
            _charactersView.OnCharacterCreateSubmit -= OnCreateCharacterSubmit;
            _charactersManager.OnGetCharacterData += OnGetCharacterData;
            _charactersManager.OnGetCharacters -= ShowCharactersInSlots;
            
            foreach (var slotCharacterView in _charactersView.SlotCharacterViews)
            {
                slotCharacterView.OnCreateCharacterSubmit -= OnOpenCreateCharacterSubmit;
                slotCharacterView.OnChoiceCharacterSubmit += OnChoiceCharacterSubmit;
            }
        }

        #endregion

        private void OnCreateCharacter(string s)
        {
            _charactersManager.GetCharacters();
        }

        private void OnGetCharacterData(string characterId, Dictionary<string, int> data)
        {
            var character = _characters.First(x => x.Value.CharacterId.Equals(characterId));

            if (!data.TryGetValue("Level", out var level))
                level = 0;
            
            if (!data.TryGetValue("Damage", out var damage))
                damage = 5;

            if (!data.TryGetValue("Health", out var health))
                health = 100;
            
            character.Key.ShowInfo(character.Value.CharacterName, level, damage, health);
        }

        private void OnChoiceCharacterSubmit(SlotCharacterView slotCharacterView)
        {
            _charactersManager.ChoiceCharacter(_characters[slotCharacterView]);
            SceneManager.LoadScene((int) EScenesIndexes.Lobby);
        }
        
        private void OnCreateCharacterSubmit(string characterName)
        {
            _charactersView.SetCreateCharacterPanelActive(false);
            _charactersManager.CreateCharacter(characterName);
        }

        private void ShowCharactersInSlots(List<CharacterResult> characters)
        {
            foreach (var characterSlotView in _charactersView.SlotCharacterViews)
            {
                characterSlotView.ShowEmpty();
            }
            
            if (characters.Count > 0 && characters.Count <= _charactersView.SlotCharacterViews.Count)
            {
                for (var i = 0; i < characters.Count - 1; i++)
                {
                    _characters.Add(_charactersView.SlotCharacterViews[i], characters[i]);
                    _charactersManager.GetCharacterData(characters[i].CharacterId);
                }
            }
            else
            {
                DLogger.Error(GetType(), nameof(ShowCharactersInSlots), 
                    "Add more characters slots! " +
                    $"(current: {_charactersView.SlotCharacterViews.Count}, require: {characters.Count})");
            }
        }
        
        private void OnOpenCreateCharacterSubmit()
        {
            _charactersView.SetCreateCharacterPanelActive(true);
        }
    }
}