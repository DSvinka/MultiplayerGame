using Code.Models;
using Code.Shared.Constants.UI;
using Code.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Views.UI.Base
{
    public abstract class RoomEditBase: MonoBehaviour
    {
        [Header("UI Indicators")] 
        [SerializeField] private TMP_Text _statusMessageText;
        
        [Header("UI Edit Room")]
        [SerializeField] private InputField _roomNameInput;
        [SerializeField] private InputField _roomMaxPlayersInput;
        [SerializeField] private Toggle _roomVisibleToggle;
        [SerializeField] private Button _roomSubmitButton;

        // По какой-то причине, при ручном изменении значений полей (через код), ивент OnValueChanged не срабатывает.
        // По этому приходится использовать этот вариант получения данных.
        protected string RoomName => _roomNameInput.text;
        protected byte RoomMaxPlayers => byte.Parse(_roomMaxPlayersInput.text);
        protected bool RoomVisible => _roomVisibleToggle.isOn;

        protected bool StatusMessageVisible;
        protected bool InputValided;
        
        private const float TextFadeAnimationDuration = 0.5f;
        private Sequence _sequence;
        
        private void Start()
        {
            SubscribeToUI();
        }

        private void OnDestroy()
        {
            UnsubscribeFromUI();
        }

        /// <summary>
        /// Устанавливает возможность редактировать комнату
        /// </summary>
        /// <param name="interactable">Можно ли редаткировать комнату</param>
        public virtual void SetRoomEditInteractable(bool interactable)
        {
            _roomNameInput.interactable = interactable;
            _roomMaxPlayersInput.interactable = interactable;
            _roomVisibleToggle.interactable = interactable;
            _roomSubmitButton.interactable = interactable;
        }
        
        /// <summary>
        /// Устанавливает значения по умолчанию у Input полей комнаты
        /// </summary>
        /// <param name="editRoomModel">Параметры комнаты</param>
        public virtual void SetupDefaultValues(EditRoomModel editRoomModel)
        {
            _roomNameInput.text = editRoomModel.RoomName;
            _roomMaxPlayersInput.text = editRoomModel.RoomMaxPlayers.ToString();
            _roomVisibleToggle.isOn = editRoomModel.RoomVisible;
        }

        /// <summary>
        /// Редактирует сообщение об успехе/ошибке/статусе
        /// </summary>
        /// <param name="text">Текст сообщения</param>
        /// <param name="color">Цвет сообщения</param>
        /// <param name="lifeTime">Время жизни сообщения</param>
        public virtual void ChangeStatusMessage(string text, Color color, int lifeTime)
        {
            _statusMessageText.text = text;
            _statusMessageText.color = new Color(color.r, color.g, color.b, 0f);
            _statusMessageText.gameObject.SetActive(true);
            
            _sequence = DOTween.Sequence();
            _sequence.Append(_statusMessageText.DOFade(1, TextFadeAnimationDuration));
            _sequence.AppendInterval(lifeTime);
            _sequence.Append(_statusMessageText.DOFade(0, TextFadeAnimationDuration));
            _sequence.Play();
        }
        
        /// <summary>
        /// Редактирует сообщение об успехе/ошибке/статусе
        /// </summary>
        /// <param name="text">Текст сообщения</param>
        /// <param name="color">Цвет сообщения</param>
        /// <param name="visible">Отображение сообщения</param>
        public virtual void ChangeStatusMessage(string text, Color color, bool visible)
        {
            _sequence?.Kill();
            
            _statusMessageText.text = text;
            _statusMessageText.color = color;
            _statusMessageText.gameObject.SetActive(visible);

            StatusMessageVisible = visible;
        }

        /// <summary>
        /// Вызывается в Start, служит для подписки на UI элементы (InputField, Button)
        /// </summary>
        protected virtual void SubscribeToUI()
        {
            _roomMaxPlayersInput.onValueChanged.AddListener(UpdateRoomMaxPlayers);
            _roomSubmitButton.onClick.AddListener(RoomSubmit);
        }

        /// <summary>
        /// Вызывается в OnDestroy, служит для отписки от UI элементов (InputField, Button)
        /// </summary>
        protected virtual void UnsubscribeFromUI()
        {
            _roomMaxPlayersInput.onValueChanged.RemoveAllListeners();
            _roomSubmitButton.onClick.AddListener(RoomSubmit);
        }

        protected virtual void RoomSubmit()
        {
            
        }

        private void UpdateRoomMaxPlayers(string roomMaxPlayers)
        {
            if (!byte.TryParse(roomMaxPlayers, out var roomMaxPlayersByte))
            {
                DLogger.Debug(GetType(), nameof(UpdateRoomMaxPlayers), 
                    "User Input Error: \"MaxPlayer Count\" field must contain only numbers");

                ChangeStatusMessage(
                    string.Format(InputMessages.UserInputErrorText, "Max Players"),
                    InputMessages.UserInputErrorColor, true
                );

                InputValided = false;
                return;
            }
            
            if (StatusMessageVisible)
                ChangeStatusMessage("", Color.black, false);
            
            InputValided = true;
        }
    }
}