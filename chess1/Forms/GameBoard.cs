using chess1.Models;
using chess1.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chess1
{
    public partial class GameBoard : BaseForm
    {
        private TopPanelUI topPanel;
        private BoardContainerUI boardContainerUI;
        private Board board;
        private TableLayoutPanel mainLayout;

        public GameBoard()
        {
            InitializeComponent();
            SetWindowSettings(900, 700);
            InitializeUI();
        }

        private void GameBoard_Load(object sender, EventArgs e)
        {

        }

        private void InitializeUI()
        {
            mainLayout = new TableLayoutPanel();
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.RowCount = 2;
            mainLayout.ColumnCount = 1;

            // Настройка размеров строк: первая строка фиксированной высоты, вторая занимает всё остальное
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 80)); // Верхняя панель
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Остальное пространство

            this.Controls.Add(mainLayout);

            topPanel = new TopPanelUI();
            topPanel.SubscribeToBackButton(BackButton_Click);
            mainLayout.Controls.Add(topPanel.Panel, 0, 0);

            board = new Board();
            // Создаем контейнер с доской
            boardContainerUI = new BoardContainerUI(board);
            boardContainerUI.CellClicked += OnCellClicked;  // Подписываемся на событие
            mainLayout.Controls.Add(boardContainerUI.BoardContainer, 0, 1);
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Выйти из игры? Прогресс не будет сохранен.",
                "Подтверждение",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void OnCellClicked(object sender, Position pos)
        {
            Cell prevSelectedCell = board.LastSelectedCell; // модель пред. клетки

            Cell currSelectedCell = board.GetCellAt(pos.Col, pos.Row); // модель текущей клетки

            bool isFigureSelection = board.IsFigureSelection(prevSelectedCell, currSelectedCell);

            // выбор своей фигуры без хода
            if (isFigureSelection) 
            {
                ClearPossibleMoves();
                ClearSelectedCell();
                SelectPlayerStartFigure(pos);
                return;
            }

            // новая клетка подходит для хода (ходим - кушаем или просто ходим)
            if (board.IsPossibleMove(pos)) 
            {
                Figure movingFigure = prevSelectedCell.Figure;
                // перемещаем в модели
                List<Cell> cellsToUpdate = board.MoveFigure(prevSelectedCell, currSelectedCell); // вернуть список клето для апдейта фигур

                foreach (Cell cellToUpdate in cellsToUpdate) 
                {
                    CellUI cellUI = boardContainerUI.GetCellUI(cellToUpdate);
                    cellUI.UpdateFigure();
                }

                ClearPossibleMoves();
                ClearSelectedCell();

                HandleMoveEnd();

                return;
            }
        }

        private void SelectPlayerStartFigure(Position pos)
        {
            Cell startCell = board.GetCellAt(pos.Col, pos.Row);
            board.LastSelectedCell = startCell;

            CellUI cellUI = boardContainerUI.GetCell(pos.Row, pos.Col);
            cellUI.SetStartBorder();

            board.possibleMoves = startCell.Figure.GetPossibleMoves(pos, board);

            foreach (Position move in board.possibleMoves)
            {
                CellUI targetCell = boardContainerUI.GetCell(move.Row, move.Col);
                if (targetCell != null)
                {
                    targetCell.SetPossibleMoveBorder();
                }
            }
        }

        private void ClearPossibleMoves()
        {
            if (board.possibleMoves == null)
            {
                return;
            }

            foreach (Position move in board.possibleMoves)
            {
                CellUI targetCell = boardContainerUI.GetCell(move.Row, move.Col);
                if (targetCell != null)
                {
                    targetCell.ClearPossibleMoveBorder();
                }
            }

            board.possibleMoves = null;

        }

        private void ClearSelectedCell()
        {
            if (board.LastSelectedCell == null)
            {
                return;
            }

            CellUI cellUI = boardContainerUI.GetCellUI(board.LastSelectedCell);
            cellUI.ClearStartBorder();

            board.LastSelectedCell = null; 
        }

        private void TogglePlayerColor()
        {
            if (board.CurrentPlayerColor == FigureColor.White) 
            {
                board.CurrentPlayerColor = FigureColor.Black;
                topPanel.SetTurnText("Ход черных");

            } else
            {
                board.CurrentPlayerColor = FigureColor.White;
                topPanel.SetTurnText("Ход белых");
            }
        }

        private void HandleMoveEnd()
        {
            TogglePlayerColor();
        }

    }
}
