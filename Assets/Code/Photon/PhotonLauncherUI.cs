using Code.Shared.Constants.UI;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Photon
{
    public class PhotonLauncherUI: MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Button _connectButton;
        [SerializeField] private TMP_Text _connectButtonText;

        [Header("Objects")]
        [SerializeField] private PhotonLauncher _photonLauncher;

        private void Start()
        {
            _connectButton.onClick.AddListener(OnConnectButton);

            _photonLauncher.OnConnect += OnConnect;
            _photonLauncher.OnDisconnect += OnDisconnect;
        }

        private void OnDestroy()
        {
            _connectButton.onClick.RemoveAllListeners();
            
            _photonLauncher.OnConnect -= OnConnect;
            _photonLauncher.OnDisconnect -= OnDisconnect;
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
            if (_photonLauncher.IsConnected)
            {
                _photonLauncher.Disconnect();
            }
            else
            {
                _photonLauncher.Connect();
            }
        }
    }
}