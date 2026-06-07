using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chess1.UI
{
    public class TopPanelUI
    {
        public Panel Panel { get; private set; }
        public Label TurnLabel { get; private set; }
        public Button BackButton { get; private set; }

        public TopPanelUI()
        {
            CreatePanel();
        }

        private void CreatePanel()
        {
            Panel = new Panel();
            Panel.Height = 80;
            Panel.Dock = DockStyle.Top;
            Panel.BackColor = Color.FromArgb(50, 50, 50);

            // Метка хода
            TurnLabel = new Label();
            TurnLabel.Text = "Ход белых";
            TurnLabel.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            TurnLabel.ForeColor = Color.White;
            TurnLabel.TextAlign = ContentAlignment.MiddleCenter;
            TurnLabel.Dock = DockStyle.Fill;

            // Кнопка назад
            BackButton = new Button();
            BackButton.Text = "← Выход";
            BackButton.Font = new Font("Segoe UI", 12);
            BackButton.Size = new Size(100, 40);
            BackButton.Location = new Point(20, 20);
            BackButton.BackColor = Color.FromArgb(200, 70, 70);
            BackButton.ForeColor = Color.White;
            BackButton.FlatStyle = FlatStyle.Flat;

            Panel.Controls.Add(BackButton);
            Panel.Controls.Add(TurnLabel);
        }

        // текст о ходе белых/черных
        public void SetTurnText(string text)
        {
            TurnLabel.Text = text;
        }

        public void SubscribeToBackButton(EventHandler handler)
        {
            BackButton.Click += handler;
        }
    }
}
