using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minesweeper
{
    public class Tile
    {
        public bool hasMine = false;
        int neighbouringMines = 0;

        public void setMine()
        {
            if (!hasMine)
            {
                hasMine = true;
            }
        }
    }
}
