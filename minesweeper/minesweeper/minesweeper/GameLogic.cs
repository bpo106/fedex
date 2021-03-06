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
        private readonly int rows;
        private readonly IInputElement canvas;
        private int temp;
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

        public void SetArea()
        {
            SetPosition();
            PlaceMines();
            SetValues();
        }

        void SetPosition()
        {
            for (int i = 0; i < area.Count; i++)
            {
                if (i < (area.Count / rows))
                {
                    area[i].farUp = true;
                }
                if (i % (area.Count / rows) == 0)
                {
                    area[i].farLeft = true;
                }
                if ((i + 1) % (area.Count / rows) == 0)
                {
                    area[i].farRight = true;
                }
                if (i >= area.Count - (area.Count / rows))
                {
                    area[i].farDown = true;
                }
            }
        }

        void PlaceMines ()
        {
            var random = new Random();
            while (mines > 0)
            {
                int place = random.Next(0, area.Count);
                if (!(area[place].hasMine))
                {
                    area[place].hasMine = true;
                    mines--;
                }
            }
        }

        void SetValues ()
        {
            for (int i = 0; i < area.Count; i++)
            {
                bool[] positions = new bool[] {
                !(area[i].farLeft || area[i].farUp) && area[i - area.Count / rows - 1].hasMine,
                !(area[i].farLeft || area[i].farDown) && area[i + area.Count / rows - 1].hasMine,
                !(area[i].farRight || area[i].farDown) && area[i + area.Count / rows + 1].hasMine,
                !(area[i].farRight || area[i].farUp) && area[i - area.Count / rows + 1].hasMine,
                !area[i].farLeft && area[i - 1].hasMine,
                !area[i].farUp && area[i - area.Count / rows].hasMine,
                !area[i].farDown && area[i + area.Count / rows].hasMine,
                !area[i].farRight && area[i + 1].hasMine};
                for (int j = 0; j < positions.Length; j++)
                {
                    if (positions[j])
                    {
                        area[i].neighbouringMines++;
                    }
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

        private void RevealNextTiles (int element)
        {
            if (!(area[element].hasMine) && !(area[element].isRevealed) && !(area[element].isProtected))
            {
                area[element].isRevealed = true;
                board.AddImage("./Images/" + area[element].neighbouringMines.ToString() + ".png", 30 * (element % (area.Count / rows)), 30 * (element / (area.Count / rows)));
                coveredMinelessTiles--;

                if (area[element].neighbouringMines == 0 && coveredMinelessTiles > 0) // Ezt az egészet lehet, hogy áthúzzuk a RevealNextTiles-ba, azt meg kurva sok (na jó, nem olyan sok) paraméterrel megszórjuk 
                {
                    if (!area[element].farLeft)
                    {
                        RevealNextTiles(element - 1);
                        if (!area[element].farUp)
                        {
                            RevealNextTiles(element - (area.Count / rows) - 1);
                        }
                    }
                    if (!area[element].farDown)
                    {
                        RevealNextTiles(element + (area.Count / rows));
                        if (!area[element].farLeft)
                        {
                            RevealNextTiles(element + (area.Count / rows) - 1);
                        }
                    }
                    if (!area[element].farRight)
                    {
                        RevealNextTiles(element + 1);
                        if (!area[element].farDown)
                        {
                            RevealNextTiles(element + (area.Count / rows) + 1);
                        }
                    }
                    if (!area[element].farUp)
                    {
                        RevealNextTiles(element - (area.Count / rows));
                        if (!area[element].farRight)
                        {
                            RevealNextTiles(element - (area.Count / rows) + 1);
                        }
                    }
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
                temp = area.Count / rows * tempy + tempx;
                if (!(area[temp].isProtected))
                {
                    if (area[temp].neighbouringMines == 0)
                    {
                        RevealNextTiles(temp);
                    }
                    else
                    {
                        area[area.Count / rows * tempy + tempx].isRevealed = true;
                        board.AddImage("./Images/" + area[temp].neighbouringMines.ToString() + ".png", 30 * tempx, 30 * tempy);
                        coveredMinelessTiles--;
                    }
                    if (!area[temp].hasMine)
                    {
                        Win();
                    }
                }
                if (area[temp].hasMine && !(area[temp].isProtected))
                {
                    amIEnded = true;
                    for (int i = 0; i < area.Count / rows; i++)
                    {
                        for (int j = 0; j < rows; j++)
                        {
                            if (area[area.Count / rows * j + i].hasMine)
                            {
                                string path = i == tempx && j == tempy ? "./Images/red.png" : "./Images/0.png";
                                board.AddImage(path, 30 * i, 30 * j);
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
                    temp = area.Count / rows * tempy + tempx;
                    area[temp].Flag(board, tempx, tempy);
                    var image = new Image();
                    string path = area[temp].isProtected ? "./Images/flag.png" : "./Images/button.png";
                    image.Source = new BitmapImage(new Uri(path, UriKind.Relative));
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

                        board.AddButton(button, 30 * (i % (area.Count / rows)), 30 * (i / (area.Count / rows)));
                    }
                }
                MessageBox.Show("You won!");
            }
        }
    }
}
