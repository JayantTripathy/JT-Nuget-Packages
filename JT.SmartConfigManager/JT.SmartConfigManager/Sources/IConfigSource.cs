using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JT.SmartConfigManager.Sources
{
    public interface IConfigSource
    {
        Dictionary<string, string> Load();
    }
}
