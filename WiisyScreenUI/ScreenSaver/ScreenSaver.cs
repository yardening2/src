using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Windows.Forms;


namespace ScreenSaver
{
    static public class ScreenSaver
    {
        static public string SaveAsImage(string dirToSaveTo)
        {
            String[] filesInDirectory = Directory.GetFiles(dirToSaveTo);
            string imagePath = dirToSaveTo + "\\" + "screenshot" + filesInDirectory.Count((filename) => {
                return filename.Contains("screenshot");
            }) + ".png";

            Bitmap printscreen = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height - 100);
            Graphics graphics = Graphics.FromImage(printscreen as System.Drawing.Image);
            graphics.CopyFromScreen(0, 0, 0, 0, printscreen.Size);

            printscreen.Save(imagePath, ImageFormat.Png);

            return imagePath;
        }

        
        static public void CreatePDF(string dirToSaveTo, string[] filesToGenerate)
        {
            String[] filesInDirectory = Directory.GetFiles(dirToSaveTo);
            string PDFPath = dirToSaveTo + "\\" + "generatedPDF" + filesInDirectory.Count((filename) => {
                return filename.Contains("generatedPDF");
            }) + ".pdf";
            //String[] filesInDirectory = Directory.GetFiles(dirToSaveTo);
            Document document = new Document();
            using (var stream = new FileStream(PDFPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                PdfWriter.GetInstance(document, stream);
                document.Open();
                foreach (var file in filesToGenerate)
                {
                    if (file.EndsWith("png"))
                    {
                        using (var imageStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            var image = iTextSharp.text.Image.GetInstance(imageStream);
                            image.ScaleToFit(document.PageSize.Width - (document.LeftMargin + document.RightMargin),
                                document.PageSize.Height - (document.BottomMargin + document.TopMargin));
                            document.Add(image);
                        }
                    }
                }

                document.Close();
            }
        }//CreatePDF
    }
}
