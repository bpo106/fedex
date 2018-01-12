using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace minesweeper
{
    public class GameLogic
    {
        public Board board;
        public List<Tile> area;
        private IInputElement canvas;
        private int tempx;
        private int tempy;
        private bool amIDead;

        public GameLogic(Board board, List<Tile> area)
        {
            this.area = area;
            this.board = board;
            amIDead = false;
        }

        public void SetArea(List<Tile> area, int rows, int mines)
        {
            PlaceMines(area, rows, mines);
            SetValues(area, rows);
        }

        void PlaceMines (List<Tile> area, int rows, int mines)
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

        void SetValues (List<Tile> area, int rows)
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

        public void DrawBoard (Board board, List<Tile> area)
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
                    button.MouseDown += new MouseButtonEventHandler(ClickHandlerRight);
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

        private void ClickHandlerLeft(object sender, RoutedEventArgs e)
        {
            if (!amIDead)
            {
                Point p = Mouse.GetPosition(canvas);
                tempx = (int)(p.X - 25) / 30;
                tempy = (int)(p.Y - 25) / 30;
                if (!(area[20 * tempy + tempx].isProtected))
                {
                    Button clicked = (Button)sender;
                    clicked.Visibility = Visibility.Hidden;
                }
                if (area[20 * tempy + tempx].hasMine && !(area[20 * tempy + tempx].isProtected))
                {
                    amIDead = true;
                    for (int i = 0; i < 20; i++)
                    {
                        for (int j = 0; j < 20; j++)
                        {
                            if (area[20 * j + i].hasMine)
                            {
                                board.AddImage("./Images/0.png", 30 * i, 30 * j);
                                board.AddImage("./Images/mine.png", 30 * i, 30 * j);
                            }
                        }
                    }
                    MessageBox.Show("You died lol");
                }
                //MessageBox.Show(tempx.ToString() + " " + tempy.ToString());
            }
        }

        private void ClickHandlerRight (object sender, MouseButtonEventArgs e)
        {
            if (!amIDead)
            {
                if (e.RightButton == MouseButtonState.Pressed)
                {
                    Button clicked = (Button)sender;
                    Point p = Mouse.GetPosition(canvas);
                    tempx = (int)(p.X - 25) / 30;
                    tempy = (int)(p.Y - 25) / 30;
                    area[20 * tempy + tempx].Flag(board, tempx, tempy);
                    var image = new Image();
                    if (area[20 * tempy + tempx].isProtected)
                    {
                        image.Source = new BitmapImage(new Uri("./Images/flag.png", UriKind.Relative));
                    }
                    else
                    {
                        image.Source = new BitmapImage(new Uri("./Images/button.png", UriKind.Relative));
                    }
                    clicked.Content = image;
                }
            }
        }
    }
}
