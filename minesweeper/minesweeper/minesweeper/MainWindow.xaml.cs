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

        static void DrawBoard (Board board)
        {
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    var button = new Button() { Width = 20, Height = 20 };
                    board.SetPosition(button, 20 * i, 20 * j);
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            var board = new Board(canvas);
            DrawBoard(board);
        }
    }
}
