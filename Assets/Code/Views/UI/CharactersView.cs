using System;
using System.Collections.Generic;
using Code.Lesson;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace Code.Views.UI
{
    public class CharactersView: MonoBehaviour
    {
        public event Action<string> OnCharacterCreateSubmit;
        
        [SerializeField] private GameObject _createCharacterPanel;
        
        [SerializeField] private InputField _characterNameInput;
        [SerializeField] private Button _createCharacterButton;

        [SerializeField] private List<SlotCharacterView> _slotCharacterViews;

        public List<SlotCharacterView> SlotCharacterViews => _slotCharacterViews;

        #region Unity Callbacks

        private void Start()
        {
            _createCharacterButton.onClick.AddListener(OnCreateCharacterButtonClick);
        }

        private void OnDestroy()
        {
            _createCharacterButton.onClick.RemoveAllListeners();
        }

        #endregion

        public void SetCreateCharacterPanelActive(bool active)
        {
            _createCharacterPanel.SetActive(active);
        }

        private void OnCreateCharacterButtonClick()
        {
            OnCharacterCreateSubmit?.Invoke(_characterNameInput.text);
        }
    }
}