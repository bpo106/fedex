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
        public bool isProtected = false;
        public int neighbouringMines = 0;

        public void SetMine ()
        {
            if (!hasMine)
            {
                hasMine = true;
            }
        }

        public void Flag ()
        {
            isProtected = !isProtected;
        }

        public void Reveal ()
        {
            if (hasMine)
            {
                //ASDASDASDASD
            }
            else
            {
                //asdasdasdasdasd
            }
        }
    }
}
