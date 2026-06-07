using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using chess1.Models;

namespace chess1
{
    public partial class GameBoard : BaseForm
    {
        private Label turnLabel;
        private Button backButton;
        private TableLayoutPanel chessBoard;
        private Button[,] cells;

        public GameBoard()
        {
            InitializeComponent();
            SetWindowSettings(900, 700);
            CreateChessBoard();
        }

        private void GameBoard_Load(object sender, EventArgs e)
        {

        }
        private void CreateChessBoard()
        {
            // Верхняя панель с информацией
            Panel topPanel = new Panel();
            topPanel.Height = 80;
            topPanel.Dock = DockStyle.Top;
            topPanel.BackColor = Color.FromArgb(50, 50, 50);

            // Метка с информацией о ходе
            turnLabel = new Label();
            turnLabel.Text = "Ход белых";
            turnLabel.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            turnLabel.ForeColor = Color.White;
            turnLabel.TextAlign = ContentAlignment.MiddleCenter;
            turnLabel.Dock = DockStyle.Fill;

            // Кнопка "Назад"
            backButton = new Button();
            backButton.Text = "← Выход";
            backButton.Font = new Font("Segoe UI", 12);
            backButton.Size = new Size(100, 40);
            backButton.Location = new Point(20, 20);
            backButton.BackColor = Color.FromArgb(200, 70, 70);
            backButton.ForeColor = Color.White;
            backButton.FlatStyle = FlatStyle.Flat;
            backButton.Click += BackButton_Click;

            topPanel.Controls.Add(turnLabel);
            topPanel.Controls.Add(backButton);

            Panel boardContainer = new Panel();
            boardContainer.Dock = DockStyle.Fill;
            boardContainer.BackColor = Color.FromArgb(240, 240, 240);

            // Шахматная доска
            chessBoard = new TableLayoutPanel();
            //chessBoard.Dock = DockStyle.Fill;
            chessBoard.Size = new Size(280, 280);
            chessBoard.ColumnCount = 8;
            chessBoard.RowCount = 8;
            //chessBoard.Padding = new Padding(20);
            chessBoard.BackColor = Color.FromArgb(240, 240, 240);
            chessBoard.Anchor = AnchorStyles.None;
            chessBoard.Location = new Point(
        (boardContainer.Width - chessBoard.Width) / 2,
        (boardContainer.Height - chessBoard.Height) / 2
    );

            int cellSize = 280 / 8; // 35 пикселей на клетку

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
            this.Controls.Add(topPanel);
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
