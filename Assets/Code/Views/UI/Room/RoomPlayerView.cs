using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Views.UI.Room
{
    public class RoomPlayerView: MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_Text _nicknameText;
        [SerializeField] private Image _personImage;

        public void Setup(string nickname, Sprite sprite)
        {
            _nicknameText.text = nickname;
            _personImage.sprite = sprite;
        }

        public void ChangeSprite(Sprite sprite)
        {
            _personImage.sprite = sprite;
        }
    }
}