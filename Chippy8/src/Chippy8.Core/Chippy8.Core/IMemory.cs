using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chippy8.Core
{
    public interface IMemory
    {
        public bool Write(byte data, byte location);
    }
}
