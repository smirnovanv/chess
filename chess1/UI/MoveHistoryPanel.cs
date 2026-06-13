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
    public class MoveHistoryPanel
    {
        private Panel panel;
        private ListBox moveHistoryListBox;
        private Board board;

        public Panel Panel => panel;

        public MoveHistoryPanel(Board board)
        {
            this.board = board;
            CreatePanel();
            board.MoveMade += OnMoveMade;
        }

        private void CreatePanel()
        {
            panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.BackColor = Color.FromArgb(250, 250, 250);
            panel.Padding = new Padding(5);
            panel.BorderStyle = BorderStyle.FixedSingle;

            // Заголовок
            Label titleLabel = new Label();
            titleLabel.Text = "История ходов";
            titleLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            titleLabel.ForeColor = Color.FromArgb(50, 50, 50);
            titleLabel.Dock = DockStyle.Top;
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.Height = 35;
            titleLabel.BackColor = Color.FromArgb(200, 200, 200);

            // ListBox для истории
            moveHistoryListBox = new ListBox();
            moveHistoryListBox.Dock = DockStyle.Fill;
            moveHistoryListBox.Font = new Font("Consolas", 10, FontStyle.Regular);
            moveHistoryListBox.IntegralHeight = false;
            moveHistoryListBox.BackColor = Color.White;

            // Кнопка очистки
            //Button clearButton = new Button();
            //clearButton.Text = "Очистить историю";
            //clearButton.Dock = DockStyle.Bottom;
            //clearButton.Height = 30;
            //clearButton.BackColor = Color.FromArgb(220, 220, 220);
            //clearButton.FlatStyle = FlatStyle.Flat;
            //clearButton.Click += (s, e) => ClearHistory();

            panel.Controls.Add(moveHistoryListBox);
            //panel.Controls.Add(clearButton);
            panel.Controls.Add(titleLabel);
        }

        private void OnMoveMade(object sender, Move move)
        {
            UpdateHistory();
        }

        public void UpdateHistory()
        {
            if (moveHistoryListBox == null) return;

            moveHistoryListBox.Items.Clear();

            var moves = board.GetAllMoves();
            for (int i = 0; i < moves.Count; i++)
            {
                int moveNumber = i / 2 + 1;
                if (i % 2 == 0) // Ход белых
                {
                    string moveText = $"{moveNumber}. {FormatMove(moves[i])}";
                    if (i + 1 < moves.Count) // Ход черных
                    {
                        moveText += $" | {FormatMove(moves[i + 1])}";
                    }
                    moveHistoryListBox.Items.Add(moveText);
                }
            }

            // Прокрутка к последнему ходу
            if (moveHistoryListBox.Items.Count > 0)
            {
                moveHistoryListBox.TopIndex = moveHistoryListBox.Items.Count - 1;
            }
        }

        private string FormatMove(Move move)
        {
            // Простое форматирование хода
            string pieceSymbol = GetPieceSymbol(move.Piece.Type);
            string from = $"{((char)('a' + move.From.Col))}{8 - move.From.Row}";
            string to = $"{((char)('a' + move.To.Col))}{8 - move.To.Row}";
            string capture = move.CapturedPiece != null ? "x" : "";

            return $"{pieceSymbol}{from}{capture}{to}";
        }

        private static readonly Dictionary<FigureType, string> PieceSymbols = new Dictionary<FigureType, string>
{
    { FigureType.Pawn, "" },
    { FigureType.Knight, "N" },
    { FigureType.Bishop, "B" },
    { FigureType.Rook, "R" },
    { FigureType.Queen, "Q" },
    { FigureType.King, "K" }
};

        private string GetPieceSymbol(FigureType type)
        {
            return PieceSymbols.TryGetValue(type, out string symbol) ? symbol : "";
        }

        //public void ClearHistory()
        //{
        //    board.ClearHistory();
        //    UpdateHistory();
        //}
    }
}
