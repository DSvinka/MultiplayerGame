using System;
using Code.Shared.Constants;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Code.Photon
{
    public class PhotonLauncher: MonoBehaviourPunCallbacks
    {
        public bool IsConnected => PhotonNetwork.IsConnected;
        
        public event Action OnConnect;
        public event Action<DisconnectCause> OnDisconnect;
        public event Action OnJoinToLobby;
        public event Action OnJoinToRoom;
        
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
        
        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = PhotonConstants.AutomaticallySyncScene;
        }

        private void Start()
        {
            Connect();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            DLogger.Debug(nameof(PhotonLauncher), nameof(OnDisconnected), 
                "Disconnected from Photon!");
            
            OnDisconnect?.Invoke(cause);
        }

        public override void OnConnectedToMaster()
        {
            DLogger.Debug(nameof(PhotonLauncher), nameof(OnConnectedToMaster), 
                "Connected to Master!");
            
            OnConnect?.Invoke();
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            DLogger.Debug(nameof(PhotonLauncher), nameof(OnJoinedLobby), 
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
            DLogger.Debug(nameof(PhotonLauncher), nameof(OnJoinedRoom), 
                $"Joined to Room! [InRoom={PhotonNetwork.InRoom}]");
            
            OnJoinToRoom?.Invoke();
        }
    }
}