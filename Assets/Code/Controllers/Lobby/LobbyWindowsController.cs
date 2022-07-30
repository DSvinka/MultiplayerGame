using System;
using Code.Managers;
using Photon.Realtime;
using UnityEngine;

namespace Code.Controllers.Lobby
{
    public class LobbyWindowsController: MonoBehaviour
    {
        [Header("UI Windows")] 
        [SerializeField] private GameObject _loadingWindow;
        [SerializeField] private GameObject _roomWindow;
        [SerializeField] private GameObject _lobbyWindow;

        [Header("Managers")] 
        [SerializeField] private LobbyManager _lobbyManager;
        [SerializeField] private RoomManager _roomManager;

        #region Unity Events

        private void Start()
        {
            _lobbyManager.OnLobbyJoin += OnLobbyJoin;
            _roomManager.OnRoomJoin += OnRoomJoin;
            _roomManager.OnRoomLeave += OnRoomLeft;

            WindowsVisibleChange(ELobbyWindows.Loading);

        }

        #endregion

        // По какой то причине, при отключении, а затем включении Canvas (или элемента на котором есть компонент Canvas ),
        // Инпуты ломались, просто переставали работать, не реагировали на нажатия и ввод.
        // Например если игрок заходит в комнату, а потом выходит, то инпуты для создания комнаты перестают работать.
        // Так что я выключаю именно GameObject, на котором нет Canvas
        private void WindowsVisibleChange(ELobbyWindows window)
        {
            _lobbyWindow.SetActive(false);
            _roomWindow.SetActive(false);
            _loadingWindow.SetActive(false);

            switch (window)
            {
                case ELobbyWindows.Lobby:
                    _lobbyWindow.SetActive(true);
                    break;
                    
                case ELobbyWindows.Room:
                    _roomWindow.SetActive(true);
                    break;
                
                case ELobbyWindows.Loading:
                    _loadingWindow.SetActive(true);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(window), window, null);
            }
        }
        
        private void OnLobbyJoin(TypedLobby lobby)
        {
            WindowsVisibleChange(ELobbyWindows.Lobby);
        }

        private void OnRoomJoin(Room room)
        {
            WindowsVisibleChange(ELobbyWindows.Room);
        }

        private void OnRoomLeft()
        {
            WindowsVisibleChange(ELobbyWindows.Lobby);
        }

        private enum ELobbyWindows
        {
            Loading = 0,
            Lobby = 1,
            Room = 2,
        }
    }
}