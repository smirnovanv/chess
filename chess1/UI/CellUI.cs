using chess1.Helpers;
using chess1.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace chess1.UI
{
    public class CellUI
    {
        public Position Position;
        public PictureBox CellField;

        private bool HasStartBorder;
        private PaintEventHandler StartBorderPaintHandler;

        private bool HasPossibleMoveBorder;
        private PaintEventHandler PossibleMoveBorderPaintHandler;
        private Cell CellModel;


        public CellUI(int cellSize, Cell cellModel) // todo refactor
        {
            Position = cellModel.Position;
            Color color = cellModel.Type == CellType.Light ? Color.FromArgb(240, 217, 181)  // Светлая
                        : Color.FromArgb(181, 136, 99);
            CellField = CreateCellField(Position.Row, Position.Col, cellSize, color);
            CellModel = cellModel;
            StartBorderPaintHandler = DrawStartBorder;
            HasStartBorder = false;

            PossibleMoveBorderPaintHandler = DrawPossibleMoveBorder;
            HasPossibleMoveBorder = false;
        }

        private PictureBox CreateCellField(int row, int col, int cellSize, Color color)
        {
            PictureBox cell = new PictureBox();
            cell.Size = new Size(cellSize, cellSize);
            cell.SizeMode = PictureBoxSizeMode.Zoom;  // Изображение вписывается в клетку
            cell.BackColor = color;
            cell.Tag = new Position(row, col);

            return cell;
        }

        public void SubscribeToClick(EventHandler clickHandler)
        {
            CellField.Click += clickHandler;
        }

        public void UpdateFigure()
        {
            Figure figure = CellModel.Figure;
            Image pieceImage = ImageLoader.GetFigureImage(figure);
            CellField.Image = pieceImage;
        }
        private void DrawStartBorder(object sender, PaintEventArgs e)
        {
            PictureBox cell = sender as PictureBox;
            if (cell == null) return;

            using (Pen yellowPen = new Pen(Color.Yellow, 3))
            {
                e.Graphics.DrawRectangle(yellowPen,
                    new Rectangle(0, 0, cell.Width - 1, cell.Height - 1));
            }
        }

        public void SetStartBorder()
        {
            if (!HasStartBorder)
            {
                HasStartBorder = true;
                CellField.Paint += StartBorderPaintHandler;
                CellField.Invalidate();
            }
        }

        public void ClearStartBorder()
        {
            if (HasStartBorder)
            {
                HasStartBorder = false;
                CellField.Paint -= StartBorderPaintHandler;
                CellField.Invalidate();
            }
        }

        private void DrawPossibleMoveBorder(object sender, PaintEventArgs e)
        {
            PictureBox cell = sender as PictureBox;
            if (cell == null) return;

            using (Pen greenPen = new Pen(Color.Green, 3))
            {
                e.Graphics.DrawRectangle(greenPen,
                    new Rectangle(0, 0, cell.Width - 1, cell.Height - 1));
            }
        }

        public void SetPossibleMoveBorder()
        {
            if (!HasPossibleMoveBorder)
            {
                HasPossibleMoveBorder = true;
                CellField.Paint += PossibleMoveBorderPaintHandler;
                CellField.Invalidate();
            }
        }

        public void ClearPossibleMoveBorder()
        {
            if (HasPossibleMoveBorder)
            {
                HasPossibleMoveBorder = false;
                CellField.Paint -= PossibleMoveBorderPaintHandler;
                CellField.Invalidate();
            }
        }

        public void ClearAllBorders()
        {
            ClearStartBorder();
            ClearPossibleMoveBorder();
        }

    }
}
