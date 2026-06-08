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
            InitializeGame();
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
            // Создаем контейнер с доской
            boardContainerUI = new BoardContainerUI();
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
            // Временное сообщение для теста
            MessageBox.Show($"Клик по клетке {pos.Row}, {pos.Col}", "Координаты");
        }

        private void InitializeGame()
        {
            board = new Board();
            UpdateBoardDisplay();
        }

        private void UpdateBoardDisplay()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Figure figure = board.GetFigureAt(new Position(row, col));
                    boardContainerUI.UpdateFigure(row, col, figure);
                }
            }
        }

    }
}
