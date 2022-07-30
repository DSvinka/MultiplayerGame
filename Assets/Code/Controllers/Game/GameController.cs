using Code.Shared.Constants;
using Code.Utils;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Controllers.Game
{
    public class GameController: MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject _playerPrefab;
        
        #region Unity Callbacks
        
        private void Start()
        {
            if (!PhotonNetwork.IsConnected)
            {
                SceneManager.LoadScene((int) EScenesIndexes.Lobby);
                return;
            }
        }
        
        private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				LeaveRoom();
			}
		}

        #endregion
        
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #region Photon Callbacks
        
		public override void OnLeftRoom()
		{
			SceneManager.LoadScene((int) EScenesIndexes.Lobby);
		}

		#endregion
    }
}