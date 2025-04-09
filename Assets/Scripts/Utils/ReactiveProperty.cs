using System;

namespace DefaultNamespace.Utils
{
    public class ReactiveProperty<T>
    {
        private T _value;
        public T Value
        {
            get => _value;
            set
            {
                if (!Equals(_value, value))
                {
                    var oldValue = _value;
                    _value = value;
                    PreviousValue = oldValue;
                    OnValueChanged?.Invoke(_value);
                }
            }
        }

        public T PreviousValue { get; private set; }

        public event Action<T>? OnValueChanged;

        public ReactiveProperty(T initialValue = default!)
        {
            _value = initialValue;
            PreviousValue = initialValue;
        }

        public void ForceNotify()
        {
            OnValueChanged?.Invoke(_value);
        }
    }
}