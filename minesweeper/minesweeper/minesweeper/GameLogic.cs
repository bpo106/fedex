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
        private bool amIEnded;
        private int size = 400; //Ez majd később paraméterként jön át
        private int coveredMinelessTiles;
        private int mines;


        public GameLogic(Board board, List<Tile> area, int mines)
        {
            this.area = area;
            this.board = board;
            amIEnded = false;
            this.mines = mines;
        }

        public void SetArea(List<Tile> area, int rows)
        {
            PlaceMines(area, rows, mines);
            SetValues(area, rows);
            coveredMinelessTiles = size - mines;
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
                    if (!(area[20 * j + i].hasMine))
                    {
                        board.AddImage("./Images/" + area[20 * j + i].neighbouringMines.ToString() + ".png", 30 * i, 30 * j);
                    }
                    if (!area[20 * j + i].isRevealed)
                    {
                        board.AddButton(button, 30 * i, 30 * j);
                    }
                }
            }
        }

        private void RevealNextTiles (int x, int y)
        {
            if (area[20 * y + x].neighbouringMines == 0)
            {
                board.AddImage("./Images/" + area[20 * y + x].neighbouringMines.ToString() + ".png", 30 * x, 30 * y);
                if (x > 1 && y > 1) RevealNextTiles(x - 1, y - 1);
                if (x > 1) RevealNextTiles(x - 1, y);
                if (x > 1 && y < 20 - 2) RevealNextTiles(x - 1, y + 1);
                if (y < 20 - 2) RevealNextTiles(x, y - 1);
                if (x < 20 - 2 && y < 20 - 2)  RevealNextTiles(x + 1, y + 1);
                if (x < 20 - 2) RevealNextTiles(x, y + 1);
                if (x < 20 - 2 && y > 0) RevealNextTiles(x - 1, y + 1);
                if (y > 1) RevealNextTiles(x, y - 1);
            }
        }

        private void ClickHandlerLeft(object sender, RoutedEventArgs e)
        {
            if (!amIEnded)
            {
                Point p = Mouse.GetPosition(canvas);
                tempx = (int)(p.X - 25) / 30;
                tempy = (int)(p.Y - 25) / 30;
                if (!(area[20 * tempy + tempx].isProtected))
                {
                    /*if (area[20 * tempy + tempx].neighbouringMines == 0)
                    {
                        RevealNextTiles(tempx, tempy);
                    }
                    else
                    {*/
                        Button clicked = (Button)sender;
                        clicked.Visibility = Visibility.Hidden;
                        coveredMinelessTiles--;
                        Win();
                    //}
                }
                if (area[20 * tempy + tempx].hasMine && !(area[20 * tempy + tempx].isProtected))
                {
                    amIEnded = true;
                    for (int i = 0; i < 20; i++)
                    {
                        for (int j = 0; j < 20; j++)
                        {
                            if (area[20 * j + i].hasMine)
                            {
                                if (i == tempx && j == tempy)
                                {
                                    board.AddImage("./Images/red.png", 30 * i, 30 * j);
                                }
                                else
                                {
                                    board.AddImage("./Images/0.png", 30 * i, 30 * j);
                                }
                                board.AddImage("./Images/mine.png", 30 * i, 30 * j);
                            }
                        }
                    }
                    MessageBox.Show("You died lol");
                }
                //MessageBox.Show(tempx.ToString() + " " + tempy.ToString());
            }
        }

        private void ClickHandlerRight(object sender, MouseButtonEventArgs e)
        {
            if (!amIEnded)
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

        private void Win()
        {
            if (coveredMinelessTiles == 0)
            {
                amIEnded = true;
                for (int i = 0; i < area.Count; i++)
                {
                    if (area[i].hasMine)
                    {
                        board.AddImage("./Images/flag.png", 30 * (i % 20), 30 * (i / 20));
                    }
                }
                MessageBox.Show("You won!");
            }
        }
    }
}
