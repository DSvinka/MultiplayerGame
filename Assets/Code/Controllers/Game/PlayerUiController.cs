using Code.Views.UI.Game;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Controllers.Game
{
    public class PlayerUiController: MonoBehaviour
    {
        #region Private Fields
        
        [SerializeField] private Camera _camera;
        [SerializeField] private Canvas _canvas;
        
        [Header("Settings")]
        [SerializeField] private Vector3 _screenOffset = new Vector3(0f, 30f, 0f);
        
        [Header("Prefabs")]
        [SerializeField] private GameObject _playerUiPrefab;
        
        public PlayerUiView PlayerUiView => _playerUiView;
	    
	    private PlayerUiView _playerUiView;
	    
        private PlayerView _playerView;
        private Transform _playerTransform;
        private Renderer _playerRenderer;
        private Vector3 _playerPosition;

        private float _characterControllerHeight;

        private CanvasGroup _canvasGroup;

        private bool _playerUiInstantiated;
        private bool _playerViewСonfigured;

        #endregion

		#region Unity Callbacks

		private void Update()
		{
			if (!_playerViewСonfigured || !_playerUiInstantiated)
				return;
			
			_playerUiView.ChangeHealth(_playerView.Health);
		}
		
		private void LateUpdate () 
		{
			if (!_playerViewСonfigured || !_playerUiInstantiated)
				return;

			_canvasGroup.alpha = _playerRenderer.isVisible ? 1f : 0f;
			
			_playerPosition = _playerTransform.position;
			_playerPosition.y += _characterControllerHeight;
			
			_playerUiView.transform.position = _camera.WorldToScreenPoint(_playerPosition) + _screenOffset;
		}
		
		#endregion
		
		/// <summary>
		/// Устанавливает игрока, от которого будет браться информация
		/// </summary>
		/// <param name="playerView">Игрок</param>
		public void SetPlayerController(PlayerView playerView)
		{
			_playerView = playerView;
            _playerTransform = _playerView.gameObject.GetComponent<Transform>();
            _playerRenderer = _playerView.gameObject.GetComponentInChildren<Renderer>();

            var characterController = _playerView.gameObject.GetComponent<CharacterController>();
            
			_characterControllerHeight = characterController.height;
			_playerUiView.Setup(_playerView.NickName, _playerView.Health);

			_playerViewСonfigured = true;
		}

		/// <summary>
		/// Создаёт и сохраняет объект PlayerUiView
		/// </summary>
		public void InstantiatePlayerUi()
		{
			var playerUiGo = PhotonNetwork.Instantiate(_playerUiPrefab.name, _playerPosition, Quaternion.identity);
			_playerUiView = playerUiGo.GetComponent<PlayerUiView>();
			_canvasGroup = playerUiGo.GetComponent<CanvasGroup>();
			
			_playerUiView.transform.SetParent(_canvas.transform, false);

			_playerUiInstantiated = true;
		}
    }
}