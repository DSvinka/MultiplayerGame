using System;
using Code.Interfaces.Managers;
using Code.Shared.Constants;
using Code.Utils;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Code.Managers
{
    public class ConnectionManager: MonoBehaviourPunCallbacks, IConnectionManager
    {
        #region Public Events
        
        public event Action OnConnect;
        public event Action<DisconnectCause> OnDisconnect;

        #endregion

        #region Unity Events

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = PhotonConstants.AutomaticallySyncScene;
        }

        private void Start()
        {
            Connect();
        }

        #endregion
        
        public bool IsConnected => PhotonNetwork.IsConnected;
        
        public void Connect()
        {
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = Application.version;
            }
        }
      
        public void Disconnect()
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.Disconnect();
            }
        }

        #region Photon Events
        
        public override void OnDisconnected(DisconnectCause cause)
        {
            DLogger.Debug(GetType(), nameof(OnDisconnected), 
                "Disconnected from Photon!");
            
            OnDisconnect?.Invoke(cause);
        }

        public override void OnConnectedToMaster()
        {
            DLogger.Debug(GetType(), nameof(OnConnectedToMaster), 
                "Connected to Master!");
            
            OnConnect?.Invoke();
        }

        #endregion
    }
}