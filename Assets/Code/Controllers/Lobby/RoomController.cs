using System.Collections.Generic;
using Code.Managers;
using Code.Models;
using Code.Shared.Constants;
using Code.Shared.Constants.UI;
using Code.Utils;
using Code.Views.UI.Lobby;
using Code.Views.UI.Room;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Code.Controllers.Lobby
{
    public class RoomController: MonoBehaviour
    {
        // TODO: Перенести префабы, спрайты и настройки по умолчанию в ScriptableObject (Data)
        [Header("Prefabs")]
        [SerializeField] private RoomPlayerView _roomPlayerPrefab;

        [Header("Sprites")]
        [SerializeField] private Sprite _roomOwnerSprite;
        [SerializeField] private Sprite _roomPlayerSprite;
        
        [Header("Views")] 
        [SerializeField] private RoomWindowView _view;

        [Header("Managers")]
        [SerializeField] private RoomManager _roomManager;
        [SerializeField] private LobbyManager _lobbyManager;

        private Dictionary<int, RoomPlayerView> _roomPlayerViews;
        
        private RoomView _userRoomView;
        private bool _waitingRoomChanges;
        private int _currentMasterClientId;

        private const int UpdateInfoInRoomInterval = 3;
        private Sequence _updateInfoInRoomSequence;
        
        #region Unity Events

        private void Start()
        {
            _roomPlayerViews = new Dictionary<int, RoomPlayerView>();

            _roomManager.OnRoomJoin += OnRoomJoin;
            _roomManager.OnRoomLeave += OnRoomLeave;
            
            _roomManager.OnPlayerJoin += OnPlayerJoin;
            _roomManager.OnPlayerLeave += OnPlayerLeave;

            _roomManager.OnMasterClientSwitch += OnMasterClientSwitch;
            
            _view.OnEditRoomSubmit += OnEditRoomSubmit;
            _view.OnLeaveRoomSubmit += OnLeaveRoomSubmit;
            _view.OnStartGameSubmit += OnStartGameSubmit;
        }

        private void OnDestroy()
        {
            _roomManager.OnRoomJoin -= OnRoomJoin;
            _roomManager.OnRoomLeave -= OnRoomLeave;
            
            _roomManager.OnPlayerJoin -= OnPlayerJoin;
            _roomManager.OnPlayerLeave -= OnPlayerLeave;

            _roomManager.OnMasterClientSwitch -= OnMasterClientSwitch;

            _view.OnEditRoomSubmit -= OnEditRoomSubmit;
            _view.OnLeaveRoomSubmit -= OnLeaveRoomSubmit;
            _view.OnStartGameSubmit -= OnStartGameSubmit;
            
            _updateInfoInRoomSequence?.Kill();
            _roomPlayerViews.Clear();
        }

        #endregion

        /// <summary>
        /// Создает и настраивает в UI интерфейсе отображение присоединившегося игрока.
        /// </summary>
        /// <param name="player">Информация о комнате</param>
        /// <returns>Созданный UI объект игрока</returns>
        private RoomPlayerView CreateRoomPlayerView(Player player)
        {
            var roomPlayerView = Instantiate(_roomPlayerPrefab, _view.PlayersListContainer);
            roomPlayerView.Setup(
                player.NickName,
                player.IsLocal ? _roomOwnerSprite : _roomPlayerSprite
            );
            return roomPlayerView;
        }

        /// <summary>
        /// Удаляет из UI интерфейса и списка указанного игрока
        /// </summary>
        /// <param name="actorNumber">Идентификатор игрока</param>
        private void DeleteRoomPlayerView(int actorNumber)
        {
            Destroy(_roomPlayerViews[actorNumber].gameObject);
            _roomPlayerViews.Remove(actorNumber);
        }

        /// <summary>
        /// Обновляет информацию о комнате в вьюшке.
        /// <para>Если клиент является владельцем комнаты, то обновление значений инпутов не запускается.*</para>
        /// <para>* Отключается изменением параметра <paramref name="ignoreMasterClientCheck"/></para>
        /// </summary>
        /// <param name="ignoreMasterClientCheck">Отключает проверку на владельца комнаты</param>
        private void UpdateInformationInRoomView(bool ignoreMasterClientCheck = false)
        {
            if (!PhotonNetwork.InRoom)
                return;
            
            var room = PhotonNetwork.CurrentRoom;

            _view.UpdateRoomInformation(
                room.CustomProperties[PhotonRoomKeys.ROOM_NAME_KEY].ToString(),
                room.Name,
                room.PlayerCount, room.MaxPlayers
            );
            
            if (ignoreMasterClientCheck || room.MasterClientId != PhotonNetwork.LocalPlayer.ActorNumber)
            {
                _view.SetupDefaultValues(new EditRoomModel()
                {
                    RoomName = room.CustomProperties[PhotonRoomKeys.ROOM_NAME_KEY].ToString(),
                    RoomVisible = room.IsVisible,
                    RoomMaxPlayers = room.MaxPlayers
                });
            }
        }
        
        private void UpdatePlayersCountInRoomView()
        {
            var room = PhotonNetwork.CurrentRoom;
            _view.UpdateRoomInformation(room.PlayerCount, room.MaxPlayers);
        }

        #region UI Callbacks

        private void OnLeaveRoomSubmit()
        {
            _roomManager.LeaveRoom();
        }

        private void OnEditRoomSubmit(EditRoomModel roomModel)
        {
            _roomManager.EditRoom(roomModel);
            _view.ChangeStatusMessage(
                PhotonMessages.RoomEditSuccessText, 
                PhotonMessages.RoomEditSuccessColor, 
                PhotonMessages.RoomEditSuccessLifeTime
            );
        }

        private void OnStartGameSubmit()
        {
            DLogger.Info(GetType(), nameof(OnStartGameSubmit), 
                "Game Started!");
            _roomManager.EditRoom(null, false);
            PhotonNetwork.LoadLevel((int) EScenesIndexes.Game);
        }

        #endregion

        #region Room Callbacks
        
        private void OnRoomJoin(Room room)
        {
            _currentMasterClientId = room.MasterClientId;
            if (room.MasterClientId == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                _view.SetRoomEditInteractable(true);
            }
            else
            {
                _view.SetRoomEditInteractable(false);
            }
                
            foreach (var player in room.Players.Values)
            {
                _roomPlayerViews.Add(player.ActorNumber, CreateRoomPlayerView(player));
            }

            UpdateInformationInRoomView(true);
            _updateInfoInRoomSequence = DOTween.Sequence();
            _updateInfoInRoomSequence.AppendCallback(() => UpdateInformationInRoomView());
            _updateInfoInRoomSequence.AppendInterval(UpdateInfoInRoomInterval);
            _updateInfoInRoomSequence.SetLoops(-1);
            _updateInfoInRoomSequence.Play();

        }
        
        private void OnRoomLeave()
        {
            foreach (var roomPlayer in _roomPlayerViews)
            {
                Destroy(roomPlayer.Value.gameObject);
            }
            _roomPlayerViews.Clear();
            _updateInfoInRoomSequence?.Kill();
        }

        private void OnPlayerJoin(Player player)
        {
            _roomPlayerViews.Add(player.ActorNumber, CreateRoomPlayerView(player));
            
            UpdatePlayersCountInRoomView();
        }

        private void OnPlayerLeave(Player player)
        {
            DeleteRoomPlayerView(player.ActorNumber);
            
            UpdatePlayersCountInRoomView();
        }

        private void OnMasterClientSwitch(Player newMasterClient)
        {
            if (_roomPlayerViews.TryGetValue(_currentMasterClientId, out var currentMasterClient))
            {
                currentMasterClient.ChangeSprite(_roomPlayerSprite);
            }
            
            _roomPlayerViews[newMasterClient.ActorNumber].ChangeSprite(_roomOwnerSprite);
            _currentMasterClientId = newMasterClient.ActorNumber;

            if (newMasterClient.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                _view.ChangeStatusMessage(
                    PhotonMessages.YourRoomOwnerText, 
                    PhotonMessages.YourRoomOwnerColor, 
                    PhotonMessages.YourRoomOwnerLifeTime
                );
            }
        }

        #endregion
    }
}