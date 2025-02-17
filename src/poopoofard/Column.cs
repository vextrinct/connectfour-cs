using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace poopoofard
{
    internal class Column
    {
        private byte[] cells = { 0, 0, 0, 0, 0, 0 };
        public byte GetCell(int index)
        {
            return cells[index];
        }
        public void SetCell(int index, int val)
        {
            cells[index] = (byte)val;
        }
    }
}
