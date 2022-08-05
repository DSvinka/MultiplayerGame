using System;
using Code.Managers;
using Code.Models.UserData;
using Code.Views.UI.Game;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Controllers.Game
{
    public class PlayerController : MonoBehaviourPunCallbacks
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject _playerPrefab;

        [Header("Controllers")] 
        [SerializeField] private PlayerUiController _playerUiController;
        
        [Header("Managers")]
        [SerializeField] private RoomManager _roomManager;
        [SerializeField] private CharacterDataManager _characterDataManager;


        public PlayerView PlayerView => _playerView;


        private PlayerView _playerView;
        
        private bool _playerInstantiated;


        #region Unity CallBacks

        private void Start()
        {
            InstantiatePlayer();
        }

        private void Update()
        {
            if (_playerInstantiated && _playerView.IsMine)
            {
                ProcessInputs();

                if (_playerView.Health <= 0f)
                {
                    _playerInstantiated = false;
                    _roomManager.LeaveRoom();
                }
            }
        }

        #endregion

        private void InstantiatePlayer()
        {
            var playerGo = PhotonNetwork.Instantiate(_playerPrefab.name, new Vector3(0f,5f,0f), Quaternion.identity, 0);
            _playerView = playerGo.GetComponent<PlayerView>();
            _playerView.SetUserDataModel(_characterDataManager.CharacterDataModel);
            _playerView.Beams.SetActive(false);
            _playerView.Health = 100;

            var cameraWork = playerGo.GetComponent<CameraWork>();
            cameraWork.OnStartFollowing();
                
            _playerUiController.InstantiatePlayerUi();
            _playerUiController.SetPlayerController(_playerView);

            _playerInstantiated = true;
        }

        private void ProcessInputs()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (!_playerView.IsFiring)
                {
                    _playerView.IsFiring = true;
                }
            }

            if (Input.GetButtonUp("Fire1"))
            {
                if (_playerView.IsFiring)
                {
                    _playerView.IsFiring = false;
                }
            }
        }
    }
}