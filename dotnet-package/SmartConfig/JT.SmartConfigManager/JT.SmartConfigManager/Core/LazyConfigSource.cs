using JT.SmartConfigManager.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JT.SmartConfigManager.Core
{
    public abstract class LazyConfigSource<T> : IConfigSource where T : class, new()
    {
        private Dictionary<string, object> _values = new();

        protected void SetValue(string key, object value)
        {
            _values[key] = value;
        }

        public Dictionary<string, object> GetTypedValues() => _values;

        public abstract Task<Dictionary<string, string>> LoadAsync();
    }
}
