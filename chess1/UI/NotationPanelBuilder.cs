using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chess1.UI
{
    public static class NotationPanelBuilder
    {
        public static Panel CreateLettersPanel(int cellSize, int height)
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

        public static Panel CreateNumbersPanel(int cellSize, int width)
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

        public static Panel CreateEmptyCorner(int width, int height)
        {
            Panel panel = new Panel();
            panel.Size = new Size(width, height);
            panel.BackColor = Color.FromArgb(240, 240, 240);
            return panel;
        }
    }
}
