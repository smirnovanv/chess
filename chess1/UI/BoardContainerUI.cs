using chess1.Helpers;
using chess1.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chess1.UI
{
    public class BoardContainerUI
    {
        private Board board;
        private Dictionary<Cell, PictureBox> cellToPictureBox;
        private TableLayoutPanel chessBoard;
        public Panel BoardContainer { get; private set; }

        public event EventHandler<Position> CellClicked;
        public BoardContainerUI(Board boardModel)
        {
            board = boardModel;
            CreateBoardContainer();
        }
        private void CreateBoardContainer() 
        {
            BoardContainer = new Panel();
            BoardContainer.Dock = DockStyle.Fill;
            BoardContainer.BackColor = Color.FromArgb(240, 240, 240);

            int boardWidth = 280;
            int cellSize = boardWidth / 8; // 35 пикселей на клетку
            int notationWidth = 24; // Ширина для цифр
            int notationHeight = 24; // Высота для букв

            // Создаем главную панель, которая будет содержать нотацию и доску
            TableLayoutPanel mainBoardPanel = new TableLayoutPanel();
            mainBoardPanel.Size = new Size(boardWidth + notationWidth * 2,
                boardWidth + notationHeight * 2);
            mainBoardPanel.ColumnCount = 3;
            mainBoardPanel.RowCount = 3;
            mainBoardPanel.BackColor = Color.Transparent;
            mainBoardPanel.Anchor = AnchorStyles.None;

            // Настройка размеров колонок и строк
            mainBoardPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, notationWidth)); // Левая нумерация
            mainBoardPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, boardWidth)); // Доска
            mainBoardPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, notationWidth)); // Правая нумерация

            mainBoardPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, notationHeight)); // Верхние буквы
            mainBoardPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, boardWidth));     // Доска
            mainBoardPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, notationHeight)); // Нижние буквы

            // ========== ВЕРХНИЕ БУКВЫ (A-H) ==========
            Panel topLettersPanel = CreateLettersPanel(cellSize, notationHeight);

            // ========== ЛЕВЫЕ ЦИФРЫ (8-1) ==========
            Panel leftNumbersPanel = CreateNumbersPanel(cellSize, notationWidth);

            // ========== ПРАВЫЕ ЦИФРЫ (8-1) ==========
            Panel rightNumbersPanel = CreateNumbersPanel(cellSize, notationWidth);

            // ========== НИЖНИЕ БУКВЫ (A-H) ==========
            Panel bottomLettersPanel = CreateLettersPanel(cellSize, notationHeight);

            // Шахматная доска
            chessBoard = new TableLayoutPanel();
            chessBoard.Size = new Size(boardWidth, boardWidth);
            chessBoard.ColumnCount = 8;
            chessBoard.RowCount = 8;
            chessBoard.BackColor = Color.FromArgb(240, 240, 240);

            // Настройка размеров клеток
            for (int i = 0; i < 8; i++)
            {
                chessBoard.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, cellSize));
                chessBoard.RowStyles.Add(new RowStyle(SizeType.Absolute, cellSize));
            }

            cellToPictureBox = new Dictionary<Cell, PictureBox>();

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {

                    Cell cellModel = board.GetCellAt(col, row);
                    Color color = cellModel.type == CellType.Light ? Color.FromArgb(240, 217, 181)  // Светлая
                        : Color.FromArgb(181, 136, 99);

                    PictureBox cellUI = CreateCellUI(row, col, cellSize, color);

                    cellToPictureBox[cellModel] = cellUI;
                    chessBoard.Controls.Add(cellUI, col, row);

                    // отрисовка фигур
                    if (cellModel.figure != null)
                    {
                        UpdateFigure(row, col, cellModel.figure);
                    }
                }
            }

            // Угловые пустые панели
            Panel topLeftCorner = CreateEmptyCorner(notationWidth, notationHeight);
            Panel topRightCorner = CreateEmptyCorner(notationWidth, notationHeight);
            Panel bottomLeftCorner = CreateEmptyCorner(notationWidth, notationHeight);
            Panel bottomRightCorner = CreateEmptyCorner(notationWidth, notationHeight);

            // Добавляем все элементы в mainBoardPanel
            // Row 0 (верхняя)
            mainBoardPanel.Controls.Add(topLeftCorner, 0, 0);
            mainBoardPanel.Controls.Add(topLettersPanel, 1, 0);
            mainBoardPanel.Controls.Add(topRightCorner, 2, 0);

            // Row 1 (средняя - доска и цифры)
            mainBoardPanel.Controls.Add(leftNumbersPanel, 0, 1);
            mainBoardPanel.Controls.Add(chessBoard, 1, 1);
            mainBoardPanel.Controls.Add(rightNumbersPanel, 2, 1);

            // Row 2 (нижняя)
            mainBoardPanel.Controls.Add(bottomLeftCorner, 0, 2);
            mainBoardPanel.Controls.Add(bottomLettersPanel, 1, 2);
            mainBoardPanel.Controls.Add(bottomRightCorner, 2, 2);

            // Центрируем всю панель с нотацией
            mainBoardPanel.Anchor = AnchorStyles.None;
            mainBoardPanel.Location = new Point(
                (BoardContainer.Width - mainBoardPanel.Width) / 2,
                (BoardContainer.Height - mainBoardPanel.Height) / 2
            );

            BoardContainer.Controls.Add(mainBoardPanel);
        }

        private Panel CreateLettersPanel(int cellSize, int height)
        {
            Panel panel = new Panel();
            panel.Size = new Size(cellSize * 8, height);
            panel.BackColor = Color.FromArgb(240, 240, 240);

            char[] letters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' };
            for (int i = 0; i < 8; i++)
            {
                Label letterLabel = new Label();
                letterLabel.Text = letters[i].ToString();
                letterLabel.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                letterLabel.ForeColor = Color.FromArgb(80, 80, 80);
                letterLabel.TextAlign = ContentAlignment.MiddleCenter;
                letterLabel.Size = new Size(cellSize, height);
                letterLabel.Location = new Point(i * cellSize, 0);
                letterLabel.BackColor = Color.Transparent;

                panel.Controls.Add(letterLabel);
            }

            return panel;
        }

        private Panel CreateNumbersPanel(int cellSize, int width)
        {
            Panel panel = new Panel();
            panel.Size = new Size(width, cellSize * 8);
            panel.BackColor = Color.FromArgb(240, 240, 240);

            for (int i = 0; i < 8; i++)
            {
                Label numberLabel = new Label();
                numberLabel.Text = (8 - i).ToString(); // 8,7,6,5,4,3,2,1
                numberLabel.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                numberLabel.ForeColor = Color.FromArgb(80, 80, 80);
                numberLabel.TextAlign = ContentAlignment.MiddleCenter;
                numberLabel.Size = new Size(width, cellSize);
                numberLabel.Location = new Point(0, i * cellSize);
                numberLabel.BackColor = Color.Transparent;
                panel.Controls.Add(numberLabel);
            }

            return panel;
        }

        private Panel CreateEmptyCorner(int width, int height)
        {
            Panel panel = new Panel();
            panel.Size = new Size(width, height);
            panel.BackColor = Color.FromArgb(240, 240, 240);
            return panel;
        }

        private void Cell_Click(object sender, EventArgs e)
        {
            PictureBox clickedCell = sender as PictureBox;
            if (clickedCell != null && clickedCell.Tag is Position pos)
            {
                // Вызываем событие, передавая координаты
                CellClicked?.Invoke(this, pos);
            }
        }

        // Метод для получения клетки по координатам
        public PictureBox GetCell(int row, int col)
        {
            if (row >= 0 && row < 8 && col >= 0 && col < 8) {
                Cell modelCell = board.GetCellAt(col, row);

                return cellToPictureBox[modelCell];
            }
                
            return null;
        }

        public void UpdateFigure(int row, int col, Figure figure)
        {
            PictureBox cell = GetCell(row, col);
            if (cell != null)
            {
                Image pieceImage = ImageLoader.GetFigureImage(figure);
                cell.Image = pieceImage;
            }
        }

        // test
        public void UpdateCell(int row, int col)
        {
            PictureBox cell = GetCell(row, col);
            //cell.BorderStyle = BorderStyle.FixedSingle;

            cell.Paint += (sender, e) =>
            {
                // Рисуем желтую рамку
                using (Pen yellowPen = new Pen(Color.Yellow, 3))
                {
                    e.Graphics.DrawRectangle(yellowPen,
                        new Rectangle(0, 0, cell.Width - 1, cell.Height - 1));
                }
            };

            cell.Invalidate(); // Перерисовываем
        }

        private PictureBox CreateCellUI(int row, int col, int cellSize, Color color)
        {
            PictureBox cell = new PictureBox();
            cell.Size = new Size(cellSize, cellSize);
            cell.SizeMode = PictureBoxSizeMode.Zoom;  // Изображение вписывается в клетку

            Cell modelCell = board.GetCellAt(col, row);
            cell.BackColor = color;

            cell.Tag = new Position(row, col);
            cell.Click += Cell_Click;

            return cell;
        }

    }
}
