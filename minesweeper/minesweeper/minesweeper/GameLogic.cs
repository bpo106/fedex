﻿using System;
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
        private int rows;
        private IInputElement canvas;
        private int tempx;
        private int tempy;
        private bool amIEnded;
        private int coveredMinelessTiles;
        private int mines;

        public GameLogic(Board board, List<Tile> area, int rows, int mines)
        {
            this.area = area;
            this.board = board;
            amIEnded = false;
            this.mines = mines;
            this.rows = rows;
            coveredMinelessTiles = area.Count - mines;
        }

        public void SetArea(List<Tile> area)
        {
            PlaceMines();
            SetValues();
        }

        void PlaceMines ()
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

        void SetValues ()
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
            for (int i = 0; i < area.Count / rows; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    var image = new Image();
                    if (area[area.Count / rows * j + i].isProtected)
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
                    if (!(area[area.Count / rows * j + i].hasMine))
                    {
                        board.AddImage("./Images/" + area[area.Count / rows * j + i].neighbouringMines.ToString() + ".png", 30 * i, 30 * j);
                    }
                    if (!area[area.Count / rows * j + i].isRevealed)
                    {
                        board.AddButton(button, 30 * i, 30 * j);
                    }
                }
            }
        }

        private void RevealNextTiles (int x, int y)
        {
            if (!(area[area.Count / rows * y + x].hasMine) && !(area[area.Count / rows * y + x].isRevealed) && !(area[20 * y + x].isProtected))
            {
                area[area.Count / rows * y + x].isRevealed = true;
                board.AddImage("./Images/" + area[20 * y + x].neighbouringMines.ToString() + ".png", 30 * x, 30 * y);
                coveredMinelessTiles--;

                if (area[area.Count / rows * y + x].neighbouringMines == 0 && coveredMinelessTiles > 0)
                {
                    if (x > 0 && y > 0)
                        RevealNextTiles(x - 1, y - 1);
                    if (x > 0)
                        RevealNextTiles(x - 1, y);
                    if (x > 0 && y < rows - 1)
                        RevealNextTiles(x - 1, y + 1);
                    if (y < rows - 1)
                        RevealNextTiles(x, y + 1);
                    if (x < area.Count / rows - 1 && y < rows - 1)
                        RevealNextTiles(x + 1, y + 1);
                    if (x < area.Count / rows - 1)
                        RevealNextTiles(x + 1, y);
                    if (x < area.Count / rows - 1 && y > 0)
                        RevealNextTiles(x + 1, y - 1);
                    if (y > 0)
                        RevealNextTiles(x, y - 1);
                }
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
                    if (area[20 * tempy + tempx].neighbouringMines == 0)
                    {
                        RevealNextTiles(tempx, tempy);
                    }
                    else
                    {
                        area[area.Count / rows * tempy + tempx].isRevealed = true;
                        board.AddImage("./Images/" + area[20 * tempy + tempx].neighbouringMines.ToString() + ".png", 30 * tempx, 30 * tempy);
                        coveredMinelessTiles--;
                    }
                    Win();
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
                    MessageBox.Show("You lost!");
                }
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
                        var image = new Image();
                        var button = new Button() { Width = 30, Height = 30, Content = image };
                        image.Source = new BitmapImage(new Uri("./Images/flag.png", UriKind.Relative));

                        board.AddButton(button, 30 * (i % 20), 30 * (i / 20));
                    }
                }
                MessageBox.Show("You won!");
            }
        }
    }
}
