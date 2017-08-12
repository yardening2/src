using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WiisyScreen.utils
{
    public class ImageBrushFromIconConverter
    {
        public static System.Windows.Media.ImageBrush createImageBrushFromIcon(System.Drawing.Icon i_theIcon)
        {
            ImageSource imageSource;
            using (System.Drawing.Bitmap bmp = i_theIcon.ToBitmap())
            {
                var stream = new MemoryStream();
                bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                imageSource = System.Windows.Media.Imaging.BitmapFrame.Create(stream);
            }

            return new ImageBrush(imageSource);
        }
    }
}
