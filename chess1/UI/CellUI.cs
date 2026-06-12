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


        public CellUI(int row, int col, int cellSize, Color backgroundColor)
        {
            Position = new Position(row, col);
            CellField = CreateCellField(row, col, cellSize, backgroundColor);
            StartBorderPaintHandler = DrawStartBorder;
            HasStartBorder = false;
            // todo отрисовать по модели
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

        public void UpdateFigure(Figure figure)
        {
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
    
    }
}
