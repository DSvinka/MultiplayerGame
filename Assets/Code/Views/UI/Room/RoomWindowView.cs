using System;
using Code.Models;
using Code.Views.UI.Base;
using Code.Views.UI.Lobby;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Views.UI.Room
{
    public class RoomWindowView: RoomEditBase
    {
        #region Public Events

        public event Action<EditRoomModel> OnEditRoomSubmit;
        public event Action OnStartGameSubmit;
        public event Action OnLeaveRoomSubmit; 

        #endregion

        [Header("UI Info")] 
        [SerializeField] private RoomView _roomView;

        [Header("UI Players List")] 
        [SerializeField] private Transform _playersListContainer;

        [Header("UI Actions")]
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _roomLeaveButton;

        public Transform PlayersListContainer => _playersListContainer;

        /// <summary>
        /// Обновляет отображаемую информацию о комнате
        /// </summary>
        /// <param name="roomName">Имя Комнаты</param>
        /// <param name="roomCode">Код Комнаты</param>
        /// <param name="currentPlayersCount">Текущее количество игроков</param>
        /// <param name="maxPlayersCount">Максимальное количество игроков</param>
        public void UpdateRoomInformation(string roomName, string roomCode, int currentPlayersCount, byte maxPlayersCount)
        {
            _roomView.Setup(roomName, roomCode, currentPlayersCount, maxPlayersCount);
        }
        
        /// <summary>
        /// Обновляет отображаемую информацию о комнате
        /// </summary>
        /// <param name="currentPlayersCount">Текущее количество игроков</param>
        /// <param name="maxPlayersCount">Максимальное количество игроков</param>
        public void UpdateRoomInformation(int currentPlayersCount, byte maxPlayersCount)
        {
            _roomView.UpdatePlayersCount(currentPlayersCount, maxPlayersCount);
        }
        
        #region RoomEditBase Methods

        protected override void SubscribeToUI()
        {
            base.SubscribeToUI();
            
            _startGameButton.onClick.AddListener(OnStartGameClick);
            _roomLeaveButton.onClick.AddListener(OnLeaveRoomClick);
        }

        protected override void UnsubscribeFromUI()
        {
            base.UnsubscribeFromUI();
            
            _startGameButton.onClick.RemoveAllListeners();
            _roomLeaveButton.onClick.RemoveAllListeners();
        }

        protected override void RoomSubmit()
        {
            var createRoomModel = new EditRoomModel()
            {
                RoomName = RoomName,
                RoomMaxPlayers = RoomMaxPlayers,
                RoomVisible = RoomVisible,
            };
            OnEditRoomSubmit?.Invoke(createRoomModel);
        }

        public override void SetRoomEditInteractable(bool interactable)
        {
            base.SetRoomEditInteractable(interactable);
            _startGameButton.interactable = interactable;
        }

        #endregion

        private void OnStartGameClick()
        {
            OnStartGameSubmit?.Invoke();
        }
        
        private void OnLeaveRoomClick()
        {
            OnLeaveRoomSubmit?.Invoke();
        }
    }
}