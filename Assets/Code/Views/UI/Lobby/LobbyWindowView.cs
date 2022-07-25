using System;
using Code.Models;
using Code.Utils;
using Code.Views.UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Views.UI.Lobby
{
    public class LobbyWindowView: RoomEditBase
    {
        #region Public Events

        public event Action<EditRoomModel> OnCreateRoom;
        public event Action<string> OnJoinRoom; 

        #endregion

        [Header("UI Rooms List")] 
        [SerializeField] private Transform _roomsListContainer;

        [Header("UI Join Room")] 
        [SerializeField] private InputField _roomCodeInput;
        [SerializeField] private Button _roomJoinButton;

        public Transform RoomsListContainer => _roomsListContainer;
        
        #region RoomEditBase Methods

        protected override void SubscribeToUI()
        {
            base.SubscribeToUI();
            
            _roomJoinButton.onClick.AddListener(OnJoinRoomClick);
        }

        protected override void UnsubscribeFromUI()
        {
            base.UnsubscribeFromUI();
            
            _roomJoinButton.onClick.RemoveAllListeners();
        }

        protected override void RoomSubmit()
        {
            if (!InputValided)
                return;
            
            var createRoomModel = new EditRoomModel()
            {
                RoomName = RoomName,
                RoomMaxPlayers = RoomMaxPlayers,
                RoomVisible = RoomVisible,
            };
            OnCreateRoom?.Invoke(createRoomModel);
        }

        #endregion

        private void OnJoinRoomClick()
        {
            OnJoinRoom?.Invoke(_roomCodeInput.text);
        }
    }
}