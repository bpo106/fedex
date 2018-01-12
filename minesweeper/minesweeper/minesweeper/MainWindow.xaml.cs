using System.Collections.Generic;
using System.Windows;

namespace minesweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Board board;
        List<Tile> area = new List<Tile>();
        GameLogic gameLogic;

        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < 400; i++) {
                area.Add(new Tile());
            }
            board = new Board(canvas);

            gameLogic = new GameLogic(board, area);
            gameLogic.area = area;
            gameLogic.SetArea(area, 20, 50);
            gameLogic.DrawBoard(board, area);
        }
    }
}
