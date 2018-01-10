using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace minesweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Board board;

        public MainWindow()
        {
            InitializeComponent();
            board = new Board(canvas);
            DrawBackground(board);
            DrawBoard(board);
        }

        static void DrawBackground (Board board)
        {
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    board.AddImage("./Images/1.gif", 30 * i, 30 * j);
                }
            }
        }

        static void DrawBoard (Board board)
        {
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    var button = new Button() { Width = 30, Height = 30 };
                    board.AddButton(button, 30 * i, 30 * j);
                }
            }
        }


    }
}
