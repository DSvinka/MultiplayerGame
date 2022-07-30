using System;
using System.Collections.Generic;
using Code.Utils;
using PlayFab.ClientModels;

namespace Code.Models.UserData
{
    public class UserDataModel: IDisposable
    {
        public event Action<string, string, bool> OnDataUpdated;

        private SubscribeValue<float> _health;
        
        public float Health { get => _health.Value; set => _health.Value = value;
        }

        public UserDataModel(int health = 0)
        {
            _health = new SubscribeValue<float>(nameof(Health), health);
            _health.OnValueChange += OnDataUpdate;
        }

        private void OnDataUpdate(string key, string value, bool sendChangeToServer)
        {
            OnDataUpdated?.Invoke(key, value, sendChangeToServer);
        }

        #region Dictionary Methods

        /// <summary>
        /// Переводит этот объект в Dictionary для дальнейшей отправки на сервер.
        /// </summary>
        /// <returns>Готовый Dictionary</returns>
        public Dictionary<string, string> ToDict()
        {
            return new Dictionary<string, string>
            {
                { nameof(Health), Health.ToString() }
            };
        }

        /// <summary>
        /// Получает значения из <paramref name="dictionary"/> и тихо обновляет данные в этом объекте
        /// </summary>
        /// <remarks>
        /// Тихое обновление данных - Обновление данных не вызывая ивент. 
        /// </remarks>
        /// <param name="dictionary">Новые данные</param>
        public void LoadFromDict(Dictionary<string, UserDataRecord> dictionary)
        {
            _health.ChangeValueClientOnly(int.Parse(dictionary[nameof(Health)].Value));
        }

        #endregion
        
        public void Dispose()
        {
            _health.OnValueChange -= OnDataUpdate;
        }
    }
}