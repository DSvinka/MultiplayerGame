using System;
using System.Collections.Generic;
using Code.Utils;
using PlayFab.ClientModels;

namespace Code.Models.UserData
{
    public class CharacterDataModel: IDisposable
    {
        public event Action<string, string, bool> OnDataUpdated;

        private SubscribeValue<int> _health;
        private SubscribeValue<int> _damage;
        private SubscribeValue<int> _level;
        
        public int Health { get => _health.Value; set => _health.Value = value; }
        public int Damage { get => _damage.Value; set => _damage.Value = value; }
        public int Level { get => _level.Value; set => _level.Value = value; }

        public CharacterDataModel(int health = 100, int damage = 5, int level = 1)
        {
            _health = new SubscribeValue<int>(nameof(Health), health);
            _health.OnValueChange += OnDataUpdate;
            
            _damage = new SubscribeValue<int>(nameof(Damage), damage);
            _damage.OnValueChange += OnDataUpdate;
            
            _level = new SubscribeValue<int>(nameof(Level), level);
            _level.OnValueChange += OnDataUpdate;
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
                { nameof(Health), Health.ToString() },
                { nameof(Damage), Damage.ToString() },
                { nameof(Level), Level.ToString() },
                
            };
        }

        /// <summary>
        /// Получает значения из <paramref name="dictionary"/> и тихо обновляет данные в этом объекте
        /// </summary>
        /// <remarks>
        /// Тихое обновление данных - Обновление данных не вызывая ивент. 
        /// </remarks>
        /// <param name="dictionary">Новые данные</param>
        public void LoadFromDict(Dictionary<string, int> dictionary)
        {
            _health.ChangeValueClientOnly(dictionary[nameof(Health)]);
            _damage.ChangeValueClientOnly(dictionary[nameof(Damage)]);
            _level.ChangeValueClientOnly(dictionary[nameof(Level)]);
        }

        #endregion
        
        public void Dispose()
        {
            _health.OnValueChange -= OnDataUpdate;
            _damage.OnValueChange -= OnDataUpdate;
            _level.OnValueChange -= OnDataUpdate;
        }
    }
}