using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace iba.Utility
{
    class GraphicsUtilities
    {
        static public Image PaintOnWhite(Image originalImage)
        {
            Image newImage = new Bitmap(originalImage.Width, originalImage.Height);
            Graphics newImageGraphics = Graphics.FromImage(newImage);
            //You may need to verify that the original image's
            //transparency is set correctly before this point
            newImageGraphics.FillRectangle(Brushes.White, 0, 0, 16, 16);
            newImageGraphics.DrawImageUnscaled(originalImage, new Point(0, 0));
            newImageGraphics.Dispose();
            return newImage;
        }
    }
}
