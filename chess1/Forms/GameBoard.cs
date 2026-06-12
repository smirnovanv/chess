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

        public GameBoard()
        {
            InitializeComponent();
            SetWindowSettings(900, 700);
            InitializeUI();
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

        private void OnCellClicked(object sender, Position pos)
        {
            Cell prevSelectedCell = board.selectedStartCell;

            Cell currSelectedCell = board.GetCellAt(pos.Col, pos.Row);

            if (prevSelectedCell == null) 
            {
                Cell startCell = board.GetCellAt(pos.Col, pos.Row);
                board.selectedStartCell = startCell;

                CellUI cellUI = boardContainerUI.GetCell(pos.Row, pos.Col);
                cellUI.SetStartBorder();

                return;

            }

            Position prevSelectedCellPosition = prevSelectedCell.Position;
            Position currSelectedCellPosition = currSelectedCell.Position;

            bool isSameCell = prevSelectedCellPosition.Row == currSelectedCellPosition.Row && prevSelectedCellPosition.Col == currSelectedCellPosition.Col;

            // Временное сообщение для теста
            // MessageBox.Show($"Клик по клетке {pos.Row}, {pos.Col}", "Координаты");

            // обнуляем пред. обводку
            bool isMoveAvailable = prevSelectedCell.Figure != null && !isSameCell && currSelectedCell.Figure == null;

            Figure movingFigure = prevSelectedCell.Figure;

            if (isMoveAvailable) {
                board.MoveFigure(prevSelectedCell, currSelectedCell);

                CellUI prevCellUI = boardContainerUI.GetCell(prevSelectedCellPosition.Row, prevSelectedCellPosition.Col);
                CellUI currCellUI = boardContainerUI.GetCell(currSelectedCellPosition.Row, currSelectedCellPosition.Col);
                prevCellUI.UpdateFigure(null);
                currCellUI.UpdateFigure(movingFigure);
            }

            if (board.selectedStartCell != null) 
            {
                
                CellUI cellUI = boardContainerUI.GetCell(prevSelectedCellPosition.Row, prevSelectedCellPosition.Col);

                board.selectedStartCell = null;
                cellUI.ClearStartBorder();
            }

            if (!isMoveAvailable)
            {
                // делаем новую
                Cell startCell = board.GetCellAt(pos.Col, pos.Row);
                board.selectedStartCell = startCell;
                CellUI startCellUI = boardContainerUI.GetCell(pos.Row, pos.Col);
                startCellUI.SetStartBorder();
            }

        }

    }
}
