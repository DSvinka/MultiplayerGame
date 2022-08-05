using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Lesson
{
    public class SlotCharacterView : MonoBehaviour
    {
        public event Action OnCreateCharacterSubmit;
        public event Action<SlotCharacterView> OnChoiceCharacterSubmit;
        
        [Header("Buttons")]
        [SerializeField] private Button _createCharacterButton;
        [SerializeField] private Button _choiceCharacterButton;
        
        [Header("Slots")]
        [SerializeField] private GameObject _emptySlot;
        [SerializeField] private GameObject _characterSlot;

        [Header("Texts")]
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _damageText;
        [SerializeField] private TMP_Text _healthText;

        #region Unity Callbacks

        private void Start()
        {
            _createCharacterButton.onClick.AddListener(OnCreateCharacterButtonClick);
            _choiceCharacterButton.onClick.AddListener(OnChoiceCharacterButtonClick);
        }

        private void OnDestroy()
        {
            _createCharacterButton.onClick.RemoveAllListeners();
            _choiceCharacterButton.onClick.RemoveAllListeners();
        }

        #endregion

        private void OnChoiceCharacterButtonClick()
        {
            OnChoiceCharacterSubmit?.Invoke(this);
        }
        
        private void OnCreateCharacterButtonClick()
        {
            OnCreateCharacterSubmit?.Invoke();
        }

        public void ShowInfo(string characterName, int level, int damage, int health)
        {
            _nameText.text = characterName;
            _levelText.text = "Level: " + level.ToString();
            _damageText.text = "Damage: " + damage.ToString();
            _healthText.text = "Health: " + health.ToString();
            
            _emptySlot.SetActive(false);
            _characterSlot.SetActive(true);
        }

        public void ShowEmpty()
        {
            _emptySlot.SetActive(true);
            _characterSlot.SetActive(false);
        }
    }
}
