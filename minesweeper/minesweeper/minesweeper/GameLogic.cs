using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minesweeper
{
    public static class GameLogic
    {
        public static void SetArea(List<Tile> area, int rows, int mines)
        {
            PlaceMines(area, rows, mines);
            SetValues(area, rows);
        }

        static void PlaceMines (List<Tile> area, int rows, int mines)
        {
            var randx = new Random();
            var randy = new Random();
            while (mines > 0)
            {
                int x = randx.Next(0, area.Count/rows);
                int y = randy.Next(0, rows);
                if (!(area[rows * y + x].hasMine))
                {
                    area[rows * y + x].SetMine();
                    mines--;
                }
            }
        }

        static void SetValues (List<Tile> area, int rows)
        {
            for (int i = 0; i < area.Count / rows; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if ((i * j > 0 && area[rows * (j - 1) + i - 1].hasMine) || //nem bal felső
                        (j > 0 && area[rows * (j - 1) + i].hasMine) || // nem felső
                        (j > 0 && i < area.Count / rows - 1 && area[rows * (j - 1) + i + 1].hasMine) || // nem jobb felső
                        (i < area.Count / rows - 1 && area[rows * j + i + 1].hasMine) || // nem jobb
                        (i < area.Count / rows - 1 && j < rows - 1 && area[rows * (j + 1) + i + 1].hasMine) || // nem jobb alsó
                        (j < rows - 1 && area[rows * (j + 1) + i].hasMine) || //nem alsó
                        (i > 0 && j < rows - 1 && area[rows * (j + 1) + i - 1].hasMine) || // nem bal alsó
                        (i > 0 && area[rows * j + i - 1].hasMine) //  nem bal
                        )
                    {
                        area[rows * j + i].neighbouringMines++;
                    }
                }
            }
        }
    }
}
