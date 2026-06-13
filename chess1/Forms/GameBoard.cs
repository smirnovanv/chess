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
        private TableLayoutPanel mainLayout;

        public GameBoard()
        {
            InitializeComponent();
            SetWindowSettings(900, 700);
            CurrentPlayerColor = FigureColor.White;
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

        private bool IsPossibleMove(Position position)
        {
            if (possibleMoves == null) return false;

            return possibleMoves.Any(move =>
                move.Row == position.Row && move.Col == position.Col);
        }

        private void TogglePlayerColor()
        {
            if (CurrentPlayerColor == FigureColor.White) 
            {
                CurrentPlayerColor = FigureColor.Black;
                topPanel.SetTurnText("Ход черных");

            } else
            {
                CurrentPlayerColor = FigureColor.White;
                topPanel.SetTurnText("Ход белых");
            }
        }

        private void HandleMoveEnd()
        {
            TogglePlayerColor();
        }

    }
}
