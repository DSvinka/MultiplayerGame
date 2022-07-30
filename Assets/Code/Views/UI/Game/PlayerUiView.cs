using System;
using System.Text;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Views.UI.Game
{
    public class PlayerUiView: MonoBehaviourPunCallbacks, IPunObservable
    {
        [Header("UI")]
        [SerializeField] private TMP_Text _playerNameText;
        [SerializeField] private Slider _playerHealthSlider;
        
        
        public TMP_Text PlayerNameText => _playerNameText;
        public Slider PlayerHealthSlider => _playerHealthSlider;
        
        private string _nickname;
        private float _health;

        public void LateUpdate()
        {
            PlayerNameText.text = _nickname;
            PlayerHealthSlider.value = _health;
        }

        public void Setup(string nickname, float health)
        {
            _nickname = nickname;
            _health = health;
        }

        public void ChangeHealth(float health)
        {
            _health = health;
        }
        
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_nickname);
                stream.SendNext(_health);
            }
            else
            {
                _nickname = (string)stream.ReceiveNext();
                _health = (float) stream.ReceiveNext();
            }
        }
    }
}