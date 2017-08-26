using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using winMacros;
using System.Windows;
using System.IO;
using System.Windows.Media.Imaging;

namespace WiisyScreen
{
    public static class WiisyScreenUIHelper
    {
        public static ActionBubble CreateActionBubble(clickedHandler runFunction, ImageBrush imageBrush)
        {
            ActionBubble newActionBubble = new ActionBubble();
            newActionBubble.Width = MainWindow.k_bubbleDiamater;
            newActionBubble.Height = MainWindow.k_bubbleDiamater;
            newActionBubble.Opacity = 1;
            newActionBubble.Margin = new System.Windows.Thickness(4);
            newActionBubble.setApp(runFunction, imageBrush);

            return newActionBubble;

        }

        public static ImageBrush createImageForEllipse(string imageName)
        {
            return new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/WiisyScreen;component/Resources/" + imageName)));
        }

        public static ActionBubble CreateCustomizeActionBubble()
        {
            ActionBubble ab = null;
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".exe";
            dlg.Filter = "exe Files (*.exe)|*.exe";

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                ab = createBubbleFromExe(dlg.FileName);
            }


            return ab;
        }

        public static ActionBubble createBubbleFromExe(string i_FileName)
        {
            ActionBubble ab = null;
            ImageBrush theIcon = utils.ImageBrushFromIconConverter.createImageBrushFromIcon(GeneralWinUtils.GetLargeIcon(i_FileName));
            ab = CreateActionBubble(() => { Process.Start(i_FileName); }, theIcon);
            ab.BubbleData.ActionData = i_FileName;
            ab.BubbleData.BubbleType = eBubbleType.Exe;

            return ab;

        }

        public static ActionBubble CreateActionBubbleFromData(ActionBubble.ActionBubbleData i_ActionBubbleData)
        {
            ActionBubble res = null;

            switch (i_ActionBubbleData.BubbleType)
            {
                case eBubbleType.Empty:
                    res = CreateActionBubble(null, null);
                    res.Opacity = 0.4;
                    break;
                case eBubbleType.Exe:
                    res = createBubbleFromExe(i_ActionBubbleData.ActionData);
                    break;
                case eBubbleType.Board:
                    res = CreateActionBubble(MainWindow.runBoard, createImageForEllipse("whiteboard-icon.png"));
                    res.BubbleData = new ActionBubble.ActionBubbleData("BoardApp", eBubbleType.Board);
                    break;
                case eBubbleType.Macro:
                    res = CreateActionBubble(MainWindow.runMacroApp, createImageForEllipse("macroicon.png"));
                    res.BubbleData = new ActionBubble.ActionBubbleData("MacroApp", eBubbleType.Macro);
                    break;
                case eBubbleType.Calc:
                    res = CreateActionBubble(() => winMacros.Macros.calc(), createImageForEllipse("calc.png"));
                    res.BubbleData = new ActionBubble.ActionBubbleData("calc", eBubbleType.Calc);
                    break;
                default:
                    break;
            }

            return res;
        }

        public static string chooseFolder(string path)
        {
            string res = null;

            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.SelectedPath = path;
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    res = dialog.SelectedPath;
                }
            }

            return res;
        }

        public static string[] ChoosePics(string path)
        {
            string[] fileNames = null;
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.InitialDirectory = path;
            dlg.Multiselect = true;
            dlg.DefaultExt = ".png";
            dlg.Filter = "png Files (*.png)|*.png";

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                fileNames = dlg.FileNames;
            }

            return fileNames;
        }



        public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        public static T ReadFromBinaryFile<T>(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }
    }
}
