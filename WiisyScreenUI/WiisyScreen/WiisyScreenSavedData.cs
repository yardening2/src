using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiisyScreen
{
    [Serializable]
    public class WiisyScreenSavedData
    {
        public List<ActionBubble.ActionBubbleData> MainBubbels { get; set; }
        public List<ActionBubble.ActionBubbleData> RepositoryData { get; set; }
        public WiisyScreenSavedData()
        {
        MainBubbels = null;
        RepositoryData = null;
        }
    }
}
