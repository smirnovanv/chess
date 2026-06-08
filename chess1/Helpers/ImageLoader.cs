using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using chess1.Models;

namespace chess1.Helpers
{
    public static class ImageLoader
    {
        private static string imagePath = Path.Combine(Application.StartupPath, "Resources", "Images");

        public static Image GetFigureImage(Figure figure)
        {
            if (figure == null) return null;

            string fileName = GetFileName(figure);
            string fullPath = Path.Combine(imagePath, fileName);

            if (File.Exists(fullPath))
            {
                return Image.FromFile(fullPath);
            }

            // Если файл не найден, пробуем искать в embedded ресурсах
            return GetEmbeddedImage(figure);
        }

        private static string GetFileName(Figure figure)
        {
            string prefix = figure.Color == FigureColor.White ? "white" : "black";
            string pieceName = figure.Type.ToString().ToLower();

            // return $"{prefix}-{pieceName}.png";
            return "white-queen.png";
        }

        private static Image GetEmbeddedImage(Figure figure)
        {
            string resourceName = $"chess1.Resources.Images.{GetFileName(figure)}";
            var stream = typeof(ImageLoader).Assembly.GetManifestResourceStream(resourceName);

            if (stream != null)
            {
                return Image.FromStream(stream);
            }

            return null;
        }
    }
}
