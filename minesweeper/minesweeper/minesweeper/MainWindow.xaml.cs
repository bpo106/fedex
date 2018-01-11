using System.Collections.Generic;
using System.Windows;

namespace minesweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameLogic gameLogic = new GameLogic();
        Board board;
        List<Tile> area = new List<Tile>();

        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < 400; i++) {
                area.Add(new Tile());
            }
            board = new Board(canvas);
            gameLogic.SetArea(area, 20, 50);
            gameLogic.DrawBoard(board, area);
        }
    }
}
