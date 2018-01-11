using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

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
            var random = new Random();
            while (mines > 0)
            {
                int x = random.Next(0, area.Count/rows);
                int y = random.Next(0, rows);
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
                    if (i * j > 0 && area[rows * (j - 1) + i - 1].hasMine) area[rows * j + i].neighbouringMines++;
                    if (j > 0 && area[rows * (j - 1) + i].hasMine) area[rows * j + i].neighbouringMines++;
                    if (j > 0 && i < area.Count / rows - 1 && area[rows * (j - 1) + i + 1].hasMine) area[rows * j + i].neighbouringMines++;
                    if (i < area.Count / rows - 1 && area[rows * j + i + 1].hasMine) area[rows * j + i].neighbouringMines++;
                    if (i < area.Count / rows - 1 && j < rows - 1 && area[rows * (j + 1) + i + 1].hasMine) area[rows * j + i].neighbouringMines++;
                    if (j < rows - 1 && area[rows * (j + 1) + i].hasMine) area[rows * j + i].neighbouringMines++;
                    if (i > 0 && j < rows - 1 && area[rows * (j + 1) + i - 1].hasMine) area[rows * j + i].neighbouringMines++;
                    if (i > 0 && area[rows * j + i - 1].hasMine) area[rows * j + i].neighbouringMines++;
                }
            }
        }

        static public void DrawBoard (Board board, List<Tile> area)
        {
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    var image = new Image();
                    if (area[20 * j + i].isProtected)
                    {
                        image.Source = new BitmapImage(new Uri("./Images/flag.png", UriKind.Relative));
                    }
                    else
                    {
                        image.Source = new BitmapImage(new Uri("./Images/button.png", UriKind.Relative));
                    }
                    var button = new Button() { Width = 30, Height = 30, Content = image };
                    button.Click += new RoutedEventHandler(ClickHandlerLeft);
                    //button.Click += ClickHandlerRight;
                    string file;
                    if (area[20 * j + i].hasMine)
                    {
                        board.AddImage("./Images/0.png", 30 * i, 30 * j);
                        file = "./Images/mine.png";
                    }
                    else
                    {
                        file = "./Images/" + area[20 * j + i].neighbouringMines.ToString() + ".png";
                    }
                    board.AddImage(file, 30 * i, 30 * j);
                    if (!area[20 * j + i].isRevealed)
                    {
                        board.AddButton(button, 30 * i, 30 * j);
                    }
                }
            }
        }

        static private void ClickHandlerLeft (object sender, RoutedEventArgs e)
        {
            Button clicked = (Button)sender;
            clicked.Visibility = Visibility.Hidden;
            //area[20 * j + i].Reveal();
            //DrawBoard(board, area);
            //átállítja a mező felfedettségi állapotát
            //újrarajzolja a táblát
        }

        static private void ClickHandlerRight (object sender, RoutedEventArgs e)
        {
            //area[20 * j + i].Flag();
            //DrawBoard(board, area);
            //átállítja a zászlót
            //újrarajzolja a táblát
        }
    }
}
