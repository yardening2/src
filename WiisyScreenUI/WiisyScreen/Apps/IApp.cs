using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiisyScreen.Apps
{
    interface IApp
    {
        String ImageSource
        {
            get;
        }

        void Activate();
    }
}
