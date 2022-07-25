using System;
using System.Collections.Generic;
using Code.Managers;
using Code.Models;
using Code.Shared.Constants;
using Code.Views.UI;
using Code.Views.UI.Lobby;
using Photon.Realtime;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Controllers
{
    public class LobbyController: MonoBehaviour
    {
        // TODO: Перенести префабы и настройки по умолчанию в ScriptableObject (Data)
        [Header("Prefabs")]
        [SerializeField] private RoomView _roomPrefab;
        
        [Header("Views")] 
        [SerializeField] private LobbyWindowView _view;

        [Header("Managers")] 
        [SerializeField] private ConnectionManager _connectionManager;
        [SerializeField] private LobbyManager _lobbyManager;
        [SerializeField] private RoomManager _roomManager;

        private Dictionary<string, RoomView> _roomViews;
        private RoomView _userRoomView;

        #region Unity Events

        private void Start()
        {
            _roomViews = new Dictionary<string, RoomView>();

            _connectionManager.OnConnect += OnConnected;
            
            _lobbyManager.OnRoomListUpdated += OnRoomListUpdated;
            _view.OnCreateRoom += OnCreateRoomSubmit;
            _view.OnJoinRoom += OnJoinRoomSubmit;
        }

        private void OnDestroy()
        {
            _connectionManager.OnConnect -= OnConnected;
            
            _lobbyManager.OnRoomListUpdated -= OnRoomListUpdated;
            _view.OnCreateRoom -= OnCreateRoomSubmit;
            _view.OnJoinRoom -= OnJoinRoomSubmit;

            _roomViews.Clear();
        }

        #endregion

        /// <summary>
        /// Создает и настраивает в UI интерфейсе указанную комнату.
        /// </summary>
        /// <param name="roomInfo">Информация о комнате</param>
        /// <returns>Созданный UI объект комнаты</returns>
        private RoomView CreateRoomView(RoomInfo roomInfo)
        {
            var roomView = Instantiate(_roomPrefab, _view.RoomsListContainer);
            roomView.Setup(
                roomInfo.CustomProperties[PhotonRoomKeys.ROOM_NAME_KEY].ToString(), 
                roomInfo.Name, 
                roomInfo.PlayerCount, roomInfo.MaxPlayers);
            return roomView;
        }

        /// <summary>
        /// Удаляет из UI интерфейса и списка указанную комнату
        /// </summary>
        /// <param name="roomCode">Код комнаты</param>
        private void DeleteRoomView(string roomCode)
        {
            Destroy(_roomViews[roomCode].gameObject);
            _roomViews.Remove(roomCode);
        }

        private void OnConnected()
        {
            _lobbyManager.JoinLobby();
        }
        
        private void OnJoinRoomSubmit(string roomCode)
        {
            _roomManager.JoinRoom(roomCode);
        }

        private void OnCreateRoomSubmit(EditRoomModel roomModel)
        {
            _roomManager.CreateRoom(roomModel);
        }

        private void OnRoomListUpdated(List<RoomInfo> roomInfos)
        {
            if (roomInfos.Count == 0 && _roomViews.Count == 0)
                return;
            
            if (roomInfos.Count == 0 && _roomViews.Count != 0)
            {
                foreach (var roomView in _roomViews.Values)
                {
                    Destroy(roomView.gameObject);
                }
                _roomViews.Clear();
                
                return;
            }
            
            var aliveRoomCodes = new List<string>();
            foreach (var roomInfo in roomInfos)
            {
                var roomContain = _roomViews.ContainsKey(roomInfo.Name);
                if (!roomInfo.IsVisible || !roomInfo.IsOpen || roomInfo.RemovedFromList)
                {
                    if (roomContain)
                    {
                        DeleteRoomView(roomInfo.Name);
                    }

                    continue;
                }
                
                aliveRoomCodes.Add(roomInfo.Name);

                if (roomContain)
                {
                    var roomView = _roomViews[roomInfo.Name];
                    roomView.Setup(
                        roomInfo.CustomProperties[PhotonRoomKeys.ROOM_NAME_KEY].ToString(), 
                        roomInfo.Name, 
                        roomInfo.PlayerCount, roomInfo.MaxPlayers  
                    );
                }
                else
                {
                    _roomViews[roomInfo.Name] = CreateRoomView(roomInfo);
                }
            }

            var roomKeysToDelete = new List<string>();
            foreach (var room in _roomViews)
            {
                if (!aliveRoomCodes.Contains(room.Key))
                {
                    Destroy(room.Value.gameObject);
                    roomKeysToDelete.Add(room.Key);
                }
            }

            if (roomKeysToDelete.Count != 0)
            {
                foreach (var roomKey in roomKeysToDelete)
                {
                    _roomViews.Remove(roomKey);
                }
            }
        }
    }
}