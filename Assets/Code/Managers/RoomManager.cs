using System;
using Code.Interfaces.Managers;
using Code.Models;
using Code.Shared.Constants;
using Code.Utils;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using PlayFab.ClientModels;
using Random = UnityEngine.Random;

namespace Code.Managers
{
    public class RoomManager: MonoBehaviourPunCallbacks, IRoomManager
    {
        #region Public Events

        public event Action<Room> OnRoomJoin;
        public event Action OnRoomLeave;
        
        public event Action<Player> OnPlayerJoin;
        public event Action<Player> OnPlayerLeave;
        
        public event Action<Player> OnMasterClientSwitch;

        #endregion

        public void JoinRoom(string code)
        {
            if (PhotonNetwork.InRoom)
            {
                DLogger.Warning(GetType(), nameof(JoinRoom),
                    "The client is already in the room! " +
                    "Disconnecting a client from the old room and connecting him to the new room...");
                
                LeaveRoom();
            }
            
            PhotonNetwork.JoinRoom(code);
        }
        
        public void LeaveRoom()
        {
            if (!PhotonNetwork.InRoom)
            {
                DLogger.Warning(GetType(), nameof(LeaveRoom),
                    "The client is not in the room!");    
                return;
            }
            
            PhotonNetwork.LeaveRoom();
        }
        
        public void JoinRandomRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }
        
        public void EditRoom(EditRoomModel editRoomModel = null, bool isOpen = true)
        {
            if (!PhotonNetwork.InRoom)
            {
                DLogger.Error(GetType(), nameof(EditRoom),
                    "The client is not in the room!");    
                return;
            }

            if (!PhotonNetwork.CurrentRoom.MasterClientId.Equals(PhotonNetwork.LocalPlayer.ActorNumber))
            {
                DLogger.Error(GetType(), nameof(EditRoom),
                    "The client is not the master of the room!");    
                return;
            }
            
            if (editRoomModel != null)
            {
                PhotonNetwork.CurrentRoom.SetCustomProperties(
                    new Hashtable() { { PhotonRoomKeys.ROOM_NAME_KEY, editRoomModel.RoomName } });
                PhotonNetwork.CurrentRoom.IsVisible = editRoomModel.RoomVisible;
                PhotonNetwork.CurrentRoom.MaxPlayers = editRoomModel.RoomMaxPlayers;
            }

            PhotonNetwork.CurrentRoom.IsOpen = isOpen;
        }

        public string CreateRoom(EditRoomModel editRoomModel)
        {
            var roomCode = Random.Range(1000, 9999).ToString();
            var roomOptions = new RoomOptions()
            {
                MaxPlayers = editRoomModel.RoomMaxPlayers,
                IsVisible = editRoomModel.RoomVisible,
                CustomRoomPropertiesForLobby = new [] {PhotonRoomKeys.ROOM_NAME_KEY},
                CustomRoomProperties = new Hashtable() {{PhotonRoomKeys.ROOM_NAME_KEY, editRoomModel.RoomName}}
            };
            
            PhotonNetwork.CreateRoom(roomCode, roomOptions);
            return roomCode;
        }
        
        #region Players Callbacks

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            DLogger.Debug(GetType(), nameof(OnPlayerEnteredRoom), 
                $"Master Client Switched to {newMasterClient.NickName} [{newMasterClient.ActorNumber}] joined to room");
            
            OnMasterClientSwitch?.Invoke(newMasterClient);
        }

        public override void OnPlayerEnteredRoom(Player player)
        {
            DLogger.Debug(GetType(), nameof(OnPlayerEnteredRoom), 
                $"Player {player.NickName} [{player.ActorNumber}] joined to room");
            
            OnPlayerJoin?.Invoke(player);
        }

        public override void OnPlayerLeftRoom(Player player)
        {
            DLogger.Debug(GetType(), nameof(OnPlayerEnteredRoom), 
                $"Player {player.NickName} [{player.ActorNumber}] leaved from room");
            
            OnPlayerLeave?.Invoke(player);
        }

        #endregion

        #region Rooms Callbacks

        public override void OnJoinedRoom()
        {
            DLogger.Debug(GetType(), nameof(CreateRoom), 
                $"Client joined to room with code: {PhotonNetwork.CurrentRoom.Name}");
            
            OnRoomJoin?.Invoke(PhotonNetwork.CurrentRoom);
        }
        
        public override void OnLeftRoom()
        {
            DLogger.Debug(GetType(), nameof(CreateRoom), 
                $"Client leaved from room");
            
            OnRoomLeave?.Invoke();
        }

        public override void OnCreatedRoom()
        {
            DLogger.Debug(GetType(), nameof(CreateRoom), 
                $"Room created with code: {PhotonNetwork.CurrentRoom.Name}");
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            DLogger.Error(GetType(), nameof(OnCreateRoomFailed), 
                $"[{returnCode}] Create Room Failed: {message}");
        }
        
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            DLogger.Error(GetType(), nameof(OnJoinRoomFailed), 
                $"[{returnCode}] Join Room Failed: {message}");
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            DLogger.Error(GetType(), nameof(OnJoinRandomFailed), 
                $"[{returnCode}] Join Room Random Failed: {message}");
        }

        #endregion
    }
}