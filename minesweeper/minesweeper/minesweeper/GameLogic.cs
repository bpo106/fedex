using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minesweeper
{
    public class GameLogic
    {
        public Tile[,] SetArea(int x, int y, int mines)
        {
            Tile[,] area = new Tile[y,x];
            PlaceMines(area, mines);
            SetValues(area);
            return area;
        }

        void PlaceMines (Tile[,] area, int mines)
        {
            var randx = new Random();
            var randy = new Random();
            while (mines > 0)
            {
                Tile currentTile = area[randy.Next(area.GetLength(0)), randx.Next(area.GetLength(1))];
                if (!currentTile.hasMine)
                {
                    currentTile.SetMine();
                    mines--;
                }
            }
        }

        void SetValues (Tile[,] area)
        {
            for (int i = 0; i < area.GetLength(1); i++)
            {
                for (int j = 0; j < area.GetLength(0); j++)
                {
                    if ((i * j > 0 && area[j - 1, i - 1].hasMine) ||
                        (i > 0 && area[j, i - 1].hasMine) ||
                        (j > 0 && area[j - 1, i].hasMine) ||
                        (i < area.GetLength(1) - 1 && j > 0 && area[j - 1, i + 1].hasMine) ||
                        (i < area.GetLength(1) - 1 && area[j, i + 1].hasMine) ||
                        (i < area.GetLength(1) - 1 && j < area.GetLength(0) - 1 && area[j + 1, i + 1].hasMine) ||
                        (j < area.GetLength(0) - 1 && area[j + 1, i].hasMine) ||
                        (i > 0  && j < area.GetLength(0) - 1 && area[j + 1, i - 1].hasMine)
                        )
                    {
                        area[j, i].neighbouringMines++;
                    }
                }
            }
        }
    }
}
