using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minesweeper
{
    public class Tile
    {
        public bool hasMine;
        public bool isProtected;
        public int neighbouringMines;
        public bool isRevealed;

        public Tile()
        {
            hasMine = false;
            isProtected = false;
            neighbouringMines = 0;
            isRevealed = false;
        }

        public void Flag (Board board, int x, int y)
        {
            isProtected = !isProtected;
        }

        public void Reveal ()
        {
            if (!isProtected)
            {
                isRevealed = true;
            }
        }
    }
}
