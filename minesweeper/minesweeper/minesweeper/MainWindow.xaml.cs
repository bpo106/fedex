using System.Collections.Generic;
using System.Windows;

namespace minesweeper
{
    public partial class MainWindow : Window
    {
        Board board;
        List<Tile> area = new List<Tile>();
        int rows;
        int mines;
        GameLogic gameLogic;

        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < 400; i++) {
                area.Add(new Tile());
            }
            board = new Board(canvas);
            rows = 20;
            mines = 20;

            gameLogic = new GameLogic(board, area, rows, mines);
            gameLogic.area = area;
            gameLogic.SetArea();
            gameLogic.DrawBoard(board, area);
        }
    }
}
