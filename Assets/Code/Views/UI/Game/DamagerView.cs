using Photon.Pun;
using UnityEngine;

namespace Code.Views.UI.Game
{
    [RequireComponent(typeof(Collider))]
    public class DamagerView: MonoBehaviourPun
    {
        [SerializeField] private float _damage = 0.1f;
        
        private void OnTriggerStay(Collider other)
        {
            if (!other.TryGetComponent<PlayerView>(out var playerView))
            {
                return;
            }
            playerView.Health -= _damage * Time.deltaTime;
        }
    }
}