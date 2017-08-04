using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WiisyScreen
{
    class AppRunner
    {
        private Window window;

        public void run(EventHandler e)
        {
            Window boardApp = BoardApp.MainWindow.Instance;
            openedWindows.Add(boardApp);
            boardApp.Closed += new EventHandler(removeWindowFromOpenedWindows);
            boardApp.Show();
        }
    }
}
