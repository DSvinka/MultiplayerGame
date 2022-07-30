using System;

namespace Code.Utils
{
    public class SubscribeValue<T>
    {
        /// <summary>
        /// T1 - Ключ, T2 - Значение, T3 - Нужно ли отправлять изменение на сервер
        /// </summary>
        public event Action<string, string, bool> OnValueChange;

        private string _key;
        private T _value;

        public string Key => _key;
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                OnValueChange?.Invoke(_key, _value.ToString(), true);
            }
        }

        public SubscribeValue(string key, T value)
        {
            _key = key;
            _value = value;
        }

        /// <summary>
        /// Меняет значение без отправки данных на сервер, только на клиенте (например когда получаем новые данные с сервера)
        /// </summary>
        /// <param name="value">Новое значение</param>
        public void ChangeValueClientOnly(T value)
        {
            _value = value;
            OnValueChange?.Invoke(_key, _value.ToString(), false);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}