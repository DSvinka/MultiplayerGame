using Code.Shared.Constants.UI;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class LauncherUI: MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Button _connectButton;
        [SerializeField] private TMP_Text _connectButtonText;

        [Header("Менеджеры")]
        [SerializeField] private LauncherManager _launcherManager;

        private void Start()
        {
            _connectButton.onClick.AddListener(OnConnectButton);

            _launcherManager.OnConnect += OnConnect;
            _launcherManager.OnDisconnect += OnDisconnect;
        }

        private void OnDestroy()
        {
            _connectButton.onClick.RemoveAllListeners();
            
            _launcherManager.OnConnect -= OnConnect;
            _launcherManager.OnDisconnect -= OnDisconnect;
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
            if (_launcherManager.IsConnected)
            {
                _launcherManager.Disconnect();
            }
            else
            {
                _launcherManager.Connect();
            }
        }
    }
}