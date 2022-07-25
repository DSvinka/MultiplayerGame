using Code.Controllers;
using Code.Managers;
using Code.Shared.Constants.UI;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Views.UI
{
    public class LauncherView: MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Button _connectButton;
        [SerializeField] private TMP_Text _connectButtonText;

        [Header("Менеджеры")]
        [SerializeField] private ConnectionManager _connectionManager;

        private void Start()
        {
            _connectButton.onClick.AddListener(OnConnectButton);

            _connectionManager.OnConnect += OnConnect;
            _connectionManager.OnDisconnect += OnDisconnect;
        }

        private void OnDestroy()
        {
            _connectButton.onClick.RemoveAllListeners();
            
            _connectionManager.OnConnect -= OnConnect;
            _connectionManager.OnDisconnect -= OnDisconnect;
        }

        /// <summary>
        /// Изменяет текст кнопки
        /// </summary>
        /// <param name="text">Текст кнопки</param>
        private void ChangeConnectButtonText(string text)
        {
            _connectButtonText.text = text;
        }

        private void OnDisconnect(DisconnectCause disconnectCause)
        {
            ChangeConnectButtonText(PhotonButtons.ConnectText);
        }

        private void OnConnect()
        {
            ChangeConnectButtonText(PhotonButtons.DisconnectText);
        }

        private void OnConnectButton()
        {
            if (_connectionManager.IsConnected)
            {
                _connectionManager.Disconnect();
            }
            else
            {
                _connectionManager.Connect();
            }
        }
    }
}