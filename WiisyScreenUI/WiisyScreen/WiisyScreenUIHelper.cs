using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using winMacros;
using System.Windows;

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
            newActionBubble.Margin = new System.Windows.Thickness(6);
            newActionBubble.setApp(runFunction, imageBrush);

            return newActionBubble;

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
                ImageBrush theIcon = utils.ImageBrushFromIconConverter.createImageBrushFromIcon(GeneralWinUtils.GetLargeIcon(dlg.FileName));
                ab = CreateActionBubble(() => { Process.Start(dlg.FileName); }, theIcon);
            }

            return ab;
        }

        public static string chooseFolder()
        {
            string res = null;

            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if(result == System.Windows.Forms.DialogResult.OK)
                {
                    res = dialog.SelectedPath;
                }
            }

            return res;
        }
    }
}
