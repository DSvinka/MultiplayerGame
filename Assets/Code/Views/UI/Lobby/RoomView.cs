using TMPro;
using UnityEngine;

namespace Code.Views.UI.Lobby
{
    public class RoomView: MonoBehaviour
    {
        [Header("UI")] 
        [SerializeField] private TMP_Text _playersCountText;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _codeText;

        public void Setup(string roomName, string roomCode, int currentPlayersCount, byte maxPlayersCount)
        {
            _codeText.text = roomCode;
            UpdateRoomName(roomName);
            UpdatePlayersCount(currentPlayersCount, maxPlayersCount);
        }

        public void UpdateRoomName(string roomName)
        {
            _nameText.text = roomName;
        }
        
        public void UpdatePlayersCount(int currentPlayersCount, byte maxPlayersCount)
        {
            _playersCountText.text = $"{currentPlayersCount}/{maxPlayersCount}";
        }
    }
}