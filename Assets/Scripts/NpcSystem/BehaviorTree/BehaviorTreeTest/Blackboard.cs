using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeTest
{
    [System.Serializable]
    public class Blackboard
    {
        private Dictionary<string, object> _data = new();
        public void Set<T>(string key, T value) => _data[key] = value;
        public T Get<T>(string key, T defaultValue = default)
        {
            if (_data.TryGetValue(key, out var value) && value is T cast)
                return cast;
            return defaultValue;
        }
        public bool Has(string key)
        {
            return _data.ContainsKey(key);
        }
    }
}