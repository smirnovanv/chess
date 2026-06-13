using chess1.Helpers;
using chess1.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chess1.UI
{
    public class BoardContainerUI
    {
        private Board board;
        private Dictionary<Cell, CellUI> cellToPictureBox;
        private TableLayoutPanel chessBoard;
        public Panel BoardContainer { get; private set; }
        private MoveHistoryPanel moveHistoryPanel;

        public event EventHandler<Position> CellClicked;

        public BoardContainerUI(Board boardModel)
        {
            board = boardModel;
            CreateBoardContainer();
        }
        private void CreateBoardContainer() 
        {
            // все поле окна
            BoardContainer = new Panel();
            BoardContainer.Dock = DockStyle.Fill;
            BoardContainer.BackColor = Color.FromArgb(240, 240, 240);

            // Создаем TableLayoutPanel табличная разметка для размещения истории и доски
            TableLayoutPanel mainContainer = new TableLayoutPanel();
            mainContainer.Dock = DockStyle.Fill;
            mainContainer.ColumnCount = 2;
            mainContainer.RowCount = 1;
            mainContainer.BackColor = Color.FromArgb(240, 240, 240);

            // Настройка колонок
            mainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220)); // для - История ходов
            mainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));  //для - Доска


            // создание контейнера блока доска + нотация
            Panel boardPanel = new Panel();
            boardPanel.Dock = DockStyle.Fill;
            boardPanel.BackColor = Color.FromArgb(240, 240, 240);

            int boardWidth = 280;
            int cellSize = boardWidth / 8; // 35 пикселей на клетку
            int notationWidth = 24; // Ширина для цифр
            int notationHeight = 24; // Высота для букв

            // Создаем главную панель, которая будет содержать нотацию и доску
            TableLayoutPanel mainBoardPanel = CreateMainBoardPanel(boardWidth, notationWidth, notationHeight);
            AddNotation(mainBoardPanel, cellSize, notationWidth, notationHeight);
            AddChessBoard(mainBoardPanel, boardWidth, cellSize);

            CenterPanel(mainBoardPanel, boardPanel);

            boardPanel.Controls.Add(mainBoardPanel);

            moveHistoryPanel = new MoveHistoryPanel(board);

            mainContainer.Controls.Add(moveHistoryPanel.Panel, 0, 0);
            mainContainer.Controls.Add(boardPanel, 1, 0);

            // добавление в окно табличного лэйаута
            BoardContainer.Controls.Add(mainContainer);
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
        public CellUI GetCell(int row, int col)
        {
            if (row >= 0 && row < 8 && col >= 0 && col < 8) {
                Cell modelCell = board.GetCellAt(col, row);

                return cellToPictureBox[modelCell];
            }
                
            return null;
        }

        public CellUI GetCellUI(Cell modelCell)
        {
          return cellToPictureBox[modelCell];
        }

        private TableLayoutPanel CreateMainBoardPanel(int boardWidth, int notationWidth, int notationHeight)
        {
            TableLayoutPanel mainBoardPanel = new TableLayoutPanel();
            mainBoardPanel.Size = new Size(boardWidth + notationWidth * 2, boardWidth + notationHeight * 2);
            mainBoardPanel.ColumnCount = 3;
            mainBoardPanel.RowCount = 3;
            mainBoardPanel.BackColor = Color.Transparent;
            mainBoardPanel.Anchor = AnchorStyles.None;

            // Настройка размеров
            mainBoardPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, notationWidth));
            mainBoardPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, boardWidth));
            mainBoardPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, notationWidth));

            mainBoardPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, notationHeight));
            mainBoardPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, boardWidth));
            mainBoardPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, notationHeight));

            return mainBoardPanel;
        }

        private void AddNotation(TableLayoutPanel mainBoardPanel, int cellSize, int notationWidth, int notationHeight)
        {
            // Верхние буквы
            Panel topLetters = NotationPanelBuilder.CreateLettersPanel(cellSize, notationHeight);
            mainBoardPanel.Controls.Add(topLetters, 1, 0);

            // Нижние буквы
            Panel bottomLetters = NotationPanelBuilder.CreateLettersPanel(cellSize, notationHeight);
            mainBoardPanel.Controls.Add(bottomLetters, 1, 2);

            // Левые цифры
            Panel leftNumbers = NotationPanelBuilder.CreateNumbersPanel(cellSize, notationWidth);
            mainBoardPanel.Controls.Add(leftNumbers, 0, 1);

            // Правые цифры
            Panel rightNumbers = NotationPanelBuilder.CreateNumbersPanel(cellSize, notationWidth);
            mainBoardPanel.Controls.Add(rightNumbers, 2, 1);

            // Углы
            Panel topLeftCorner = NotationPanelBuilder.CreateEmptyCorner(notationWidth, notationHeight);
            Panel topRightCorner = NotationPanelBuilder.CreateEmptyCorner(notationWidth, notationHeight);
            Panel bottomLeftCorner = NotationPanelBuilder.CreateEmptyCorner(notationWidth, notationHeight);
            Panel bottomRightCorner = NotationPanelBuilder.CreateEmptyCorner(notationWidth, notationHeight);

            mainBoardPanel.Controls.Add(topLeftCorner, 0, 0);
            mainBoardPanel.Controls.Add(topRightCorner, 2, 0);
            mainBoardPanel.Controls.Add(bottomLeftCorner, 0, 2);
            mainBoardPanel.Controls.Add(bottomRightCorner, 2, 2);
        }
    
        private void AddChessBoard(TableLayoutPanel mainBoardPanel, int boardWidth, int cellSize)
        {
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

            cellToPictureBox = new Dictionary<Cell, CellUI>();

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {

                    Cell cellModel = board.GetCellAt(col, row);
                    Color color = cellModel.Type == CellType.Light ? Color.FromArgb(240, 217, 181)  // Светлая
                        : Color.FromArgb(181, 136, 99);

                    CellUI cellUI = new CellUI(row, col, cellSize, color);

                    cellUI.SubscribeToClick(Cell_Click);

                    cellToPictureBox[cellModel] = cellUI;
                    chessBoard.Controls.Add(cellUI.CellField, col, row);

                    // отрисовка фигур
                    if (cellModel.Figure != null)
                    {
                        cellUI.UpdateFigure(cellModel.Figure);
                    }
                }
            }

            mainBoardPanel.Controls.Add(chessBoard, 1, 1);
        }

        private void CenterPanel(TableLayoutPanel panel, Control parent)
        {
            panel.Location = new Point(
                (parent.Width - panel.Width) / 2,
                (parent.Height - panel.Height) / 2
            );
        }

    }
}
