using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Interfaces
{
    interface IWritable
    {
        void WriteData();

        void WriteData(string key, string value);

        void WriteData(string key, float value);

        void WriteData(string key, int value);

        void WriteData(string key, bool value);
    }
}
