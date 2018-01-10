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
        List<Tile> area = new List<Tile>();

        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < 400; i++) {
                area.Add(new Tile());
            }
            board = new Board(canvas);
            GameLogic.SetArea(area, 20, 100);
            area[5].isRevealed = true;
            DrawBoard(board);
        }

        void DrawBoard (Board board)
        {
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    var button = new Button() { Width = 30, Height = 30 };
                    string file;
                    if (area[20 *j + i].isRevealed)
                    {
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
                    }
                    else
                    {
                        board.AddButton(button, 30 * i, 30 * j);
                    }
                }
            }
        }


    }
}
