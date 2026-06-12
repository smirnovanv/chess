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
        private FigureColor CurrentPlayerColor;
        private List<Position> possibleMoves;

        public GameBoard()
        {
            InitializeComponent();
            SetWindowSettings(900, 700);
            InitializeUI();
            CurrentPlayerColor = FigureColor.White;
            InitializeBoardUI();
        }

        private void GameBoard_Load(object sender, EventArgs e)
        {

        }

        private void InitializeUI()
        {
            topPanel = new TopPanelUI();
            topPanel.SubscribeToBackButton(BackButton_Click);
            this.Controls.Add(topPanel.Panel);
        }

        private void InitializeBoardUI()
        {
            board = new Board();
            // Создаем контейнер с доской
            boardContainerUI = new BoardContainerUI(board);
            boardContainerUI.CellClicked += OnCellClicked;  // Подписываемся на событие
            this.Controls.Add(boardContainerUI.BoardContainer);
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

        private bool IsPlayerFigure(Cell cell)
        {
            Figure figure = cell.Figure;

            return figure != null && (figure.Color == CurrentPlayerColor);
        }

        private void OnCellClicked(object sender, Position pos)
        {
            Cell prevSelectedCell = board.LastSelectedCell; // модель пред. клетки

            Cell currSelectedCell = board.GetCellAt(pos.Col, pos.Row); // модель текущей клетки

            // ВАРИАНТ О Первый выбор, клетка пустая или с фигурой противника
            if (prevSelectedCell == null && !IsPlayerFigure(currSelectedCell))
            {
                return;
            }

            // ВАРИАНТ 1 Первый выбор, клетка со своей фигурой
            if (prevSelectedCell == null && IsPlayerFigure(currSelectedCell))
            {
                SelectPlayerStartFigure(pos);
                return;
            }

            // к этому моменту своя фигура была ранее выбрана
            bool isSameCell = IsSamePosition(prevSelectedCell.Position, currSelectedCell.Position);

            // ВАРИАНТ 2. Та же клетка
            if (isSameCell) 
            {
                return;
            }

            // ВАРИАНТ 3. На новой клетке своя фигура -> пока переключаемся (потом рассмотреть рокировку)
            if (IsPlayerFigure(currSelectedCell)) 
            {
                ClearPossibleMoves();
                ClearSelectedCell();
                SelectPlayerStartFigure(pos);
                return;
            }

            // новая клетка подходит для хода (ходим - кушаем или просто ходим)
            if (IsPossibleMove(pos)) 
            {
                Figure movingFigure = prevSelectedCell.Figure;
                // перемещаем в модели
                board.MoveFigure(prevSelectedCell, currSelectedCell);

                // перемещаем в UI
                CellUI prevCellUI = boardContainerUI.GetCellUI(prevSelectedCell);
                CellUI currCellUI = boardContainerUI.GetCellUI(currSelectedCell);
                prevCellUI.UpdateFigure(null);
                currCellUI.UpdateFigure(movingFigure);

                ClearPossibleMoves();
                ClearSelectedCell();
                // ход завершен 1 TODO

                return;
            }
        }

        private void SelectPlayerStartFigure(Position pos)
        {
            Cell startCell = board.GetCellAt(pos.Col, pos.Row);
            board.LastSelectedCell = startCell;

            CellUI cellUI = boardContainerUI.GetCell(pos.Row, pos.Col);
            cellUI.SetStartBorder();

            possibleMoves = startCell.Figure.GetPossibleMoves(pos, board);

            foreach (Position move in possibleMoves)
            {
                CellUI targetCell = boardContainerUI.GetCell(move.Row, move.Col);
                if (targetCell != null)
                {
                    targetCell.SetPossibleMoveBorder();
                }
            }
        }

        private bool IsSamePosition(Position pos1, Position pos2)
        {
            return pos1.Row == pos2.Row && pos1.Col == pos2.Col;
        }

        private void ClearPossibleMoves()
        {
            foreach (Position move in possibleMoves)
            {
                CellUI targetCell = boardContainerUI.GetCell(move.Row, move.Col);
                if (targetCell != null)
                {
                    targetCell.ClearPossibleMoveBorder();
                }
            }

            possibleMoves = null;

        }

        private void ClearSelectedCell()
        {
            CellUI cellUI = boardContainerUI.GetCellUI(board.LastSelectedCell);
            cellUI.ClearStartBorder();

            board.LastSelectedCell = null; 
        }

        public bool IsPossibleMove(Position position)
        {
            if (possibleMoves == null) return false;

            return possibleMoves.Any(move =>
                move.Row == position.Row && move.Col == position.Col);
        }
    }
}
