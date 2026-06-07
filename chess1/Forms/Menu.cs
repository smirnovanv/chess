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
    public partial class Menu : BaseForm
    {
        public Menu()
        {
            InitializeComponent();
            SetWindowSettings(900, 700);
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Открыть игровую форму и закрыть меню
            GameBoard gameBoard = new GameBoard();
            gameBoard.Show();      // Показать игровое окно
            this.Hide();           // Скрыть меню (не закрывать полностью)

            // Опционально: когда закроют игру, показать меню снова
            gameBoard.FormClosed += (s, args) => this.Show();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // todo: сохраненная игра
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
