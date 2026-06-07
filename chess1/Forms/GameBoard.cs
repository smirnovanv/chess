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
        private TableLayoutPanel chessBoard;
        private Button[,] cells;
        private TopPanelUI topPanel;

        public GameBoard()
        {
            InitializeComponent();
            SetWindowSettings(900, 700);
            InitializeUI();
            CreateChessBoard();
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

        private void CreateChessBoard()
        {
            Panel boardContainer = new Panel();
            boardContainer.Dock = DockStyle.Fill;
            boardContainer.BackColor = Color.FromArgb(240, 240, 240);

            int boardWidth = 280;

            // Шахматная доска
            chessBoard = new TableLayoutPanel();
            chessBoard.Size = new Size(boardWidth, boardWidth);
            chessBoard.ColumnCount = 8;
            chessBoard.RowCount = 8;
            chessBoard.BackColor = Color.FromArgb(240, 240, 240);
            chessBoard.Anchor = AnchorStyles.None;
            chessBoard.Location = new Point(
        (boardContainer.Width - chessBoard.Width) / 2,
        (boardContainer.Height - chessBoard.Height) / 2
    );

            int cellSize = boardWidth / 8; // 35 пикселей на клетку

            // Настройка размеров клеток
            for (int i = 0; i < 8; i++)
            {
                chessBoard.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, cellSize));
                chessBoard.RowStyles.Add(new RowStyle(SizeType.Absolute, cellSize));
            }

            cells = new Button[8, 8];

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Button cell = new Button();
                    //cell.Dock = DockStyle.Fill;
                    cell.Size = new Size(cellSize, cellSize);

                    cell.FlatStyle = FlatStyle.Flat;
                    cell.FlatAppearance.BorderSize = 0;
                    cell.Tag = new Position(row, col);
                    cell.Click += Cell_Click;

                    // Шахматная раскраска
                    if ((row + col) % 2 == 0)
                    {
                        cell.BackColor = Color.FromArgb(240, 217, 181); // Светлая клетка
                    }
                    else
                    {
                        cell.BackColor = Color.FromArgb(181, 136, 99); // Темная клетка
                    }

                    cells[row, col] = cell;
                    chessBoard.Controls.Add(cell, col, row);
                }
            }

            boardContainer.Controls.Add(chessBoard);
            this.Controls.Add(boardContainer); 
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

        private void Cell_Click(object sender, EventArgs e)
        {
            Button clickedCell = sender as Button;
            Position pos = (Position)clickedCell.Tag;

            // Временное сообщение для теста
            MessageBox.Show($"Клик по клетке {pos.Row}, {pos.Col}", "Координаты");
        }

    }
}
