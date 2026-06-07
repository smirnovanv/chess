using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chess1
{
    public class BaseForm: Form
    {
        protected virtual void SetWindowSettings(int width, int height) 
        {
            this.Text = "Шахматы - Игра";
            this.Size = new Size(width, height);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.MinimumSize = new Size(width, height);
            this.MaximumSize = new Size(width, height);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
        }
    }
}
