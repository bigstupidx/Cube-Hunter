using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Interfaces
{
    interface IReadable
    {
        void ReadData();

        string ReadStringData(string key);

        float ReadFloatData(string key);

        int ReadIntData(string key);

        bool ReadBoolData(string key);
        
    }
}
