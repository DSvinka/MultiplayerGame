using System;
using Code.Models.UserData;
using Photon.Pun;
using UnityEngine;

namespace Code.Views.UI.Game
{
    public class PlayerView : MonoBehaviourPunCallbacks, IPunObservable
    {
        [SerializeField] private GameObject _beams;

        public GameObject Beams => _beams;
        public bool IsMine => photonView.IsMine;
        public string NickName => photonView.Owner.NickName;

        public int Health
        {
            get => _health;
            set => ChangeHealth(value);
        }

        public bool IsFiring { get; set; }
        
        private CharacterDataModel _characterDataModel;
        private int _health;
        private bool _needUpdate;


        #region Unity Callbacks

        private void Update()
        {
            if (IsFiring != _beams.activeInHierarchy)
            {
                _beams.SetActive(IsFiring);
            }

            if (_needUpdate && photonView.IsMine)
            {
                Health = _health;
                
                _needUpdate = false;
            }
        }

        #endregion

        private void ChangeHealth(int newHealth)
        {
            if (photonView.IsMine)
            {
                _health = newHealth;
                _characterDataModel.Health = newHealth;
            }
            else
            {
                _health = newHealth;
                _needUpdate = true;
            }
        }

        #region IPunObservable Callbacks

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(IsFiring);
                stream.SendNext(_health);
                stream.SendNext(_needUpdate);
            }
            else
            {
                IsFiring = (bool) stream.ReceiveNext();
                _health = (int) stream.ReceiveNext();
                _needUpdate = (bool) stream.ReceiveNext();
            }
        }

        #endregion

        public void SetUserDataModel(CharacterDataModel characterDataModel)
        {
            _characterDataModel = characterDataModel;
        }
    }
}