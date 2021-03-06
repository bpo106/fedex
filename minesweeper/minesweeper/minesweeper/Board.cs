﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace minesweeper
{
    public class Board
    {
        private const int tileWidth = 30;
        private const int tileHeight = 30;

        private Canvas Canvas { get; set; }
        private SolidColorBrush LineColor { get; set; } = SystemColors.WindowFrameBrush;
        private SolidColorBrush ShapeColor { get; set; } = new SolidColorBrush(Colors.Black);
        public RoutedEventHandler Click { get; internal set; }

        public Board(Canvas canvas)
        {
            Canvas = canvas;
        }

        public void BackgroundColor(Color color)
        {
            Canvas.Background = new SolidColorBrush(color);
        }

        public void StrokeColor(Color color)
        {
            LineColor = new SolidColorBrush(color);
        }

        public void FillColor(Color color)
        {
            ShapeColor = new SolidColorBrush(color);
        }

        public void AddImage(string source, double x, double y)
        {
            var image = new Image()
            {
                Width = tileWidth,
                Height = tileHeight,
                Source = new BitmapImage(new Uri(source, UriKind.Relative))
            };

            Canvas.Children.Add(image);
            SetPosition(image, x, y);
        }

        public void AddImage(Canvas canvas, double x, double y)
        {
            Canvas.Children.Add(canvas);
            SetPosition(canvas, x, y);
        }

        public void AddButton(Button button, double x, double y)
        {
            Canvas.Children.Add(button);
            Canvas.SetLeft(button, x);
            Canvas.SetTop(button, y);
        }

        public void SetPosition(UIElement uIElement, double x, double y)
        {
            Canvas.SetLeft(uIElement, x);
            Canvas.SetTop(uIElement, y);
        }
    }
}