﻿using System;
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
            if (!isProtected)
            {
                isRevealed = true;
                if (hasMine)
                {
                    //ASDASDASDASD
                }
            }
        }
    }
}
