using System;
using Code.Shared.Constants;
using Code.Utils;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Code
{
    public class LauncherManager: MonoBehaviourPunCallbacks
    {
        #region Ивенты
        
        public event Action OnConnect;
        public event Action<DisconnectCause> OnDisconnect;
        
        public event Action OnJoinToLobby;
        public event Action OnJoinToRoom;
        
        #endregion
        
        public bool IsConnected => PhotonNetwork.IsConnected;

        /// <summary>
        /// Если подключение к серверам отсутствует, подключает пользователя к серверам Photon, используя настройки из файла PhotonServerSettings
        /// </summary>
        public void Connect()
        {
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = Application.version;
            }
        }
        
        /// <summary>
        /// Если подключение к серверам присутствует, отключает пользователя от серверов Photon
        /// </summary>
        public void Disconnect()
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.Disconnect();
            }
        }

        #region Unity Методы

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = PhotonConstants.AutomaticallySyncScene;
        }

        private void Start()
        {
            Connect();
        }

        #endregion

        #region Photon Методы
        
        public override void OnDisconnected(DisconnectCause cause)
        {
            DLogger.Debug(nameof(LauncherManager), nameof(OnDisconnected), 
                "Disconnected from Photon!");
            
            OnDisconnect?.Invoke(cause);
        }

        public override void OnConnectedToMaster()
        {
            DLogger.Debug(nameof(LauncherManager), nameof(OnConnectedToMaster), 
                "Connected to Master!");
            
            OnConnect?.Invoke();
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            DLogger.Debug(nameof(LauncherManager), nameof(OnJoinedLobby), 
                $"Joined to Lobby! [InLobby={PhotonNetwork.InLobby}]");
            
            OnJoinToLobby?.Invoke();

            var roomOptions = new RoomOptions()
            {
                MaxPlayers = PhotonConstants.DefaultRoomMaxPlayer, IsVisible = PhotonConstants.DefaultRoomVisible,
            };
            PhotonNetwork.JoinOrCreateRoom(PhotonConstants.DefaultRoomName, roomOptions, TypedLobby.Default);
        }

        public override void OnJoinedRoom()
        {
            DLogger.Debug(nameof(LauncherManager), nameof(OnJoinedRoom), 
                $"Joined to Room! [InRoom={PhotonNetwork.InRoom}]");
            
            OnJoinToRoom?.Invoke();
        }
        
        #endregion
    }
}