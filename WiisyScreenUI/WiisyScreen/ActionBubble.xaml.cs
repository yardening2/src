using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace WiisyScreen
{
    /// <summary>
    /// Interaction logic for ActionBubble.xaml
    /// </summary>
    public partial class ActionBubble : UserControl
    {
        public ActionBubble()
        {
            InitializeComponent();
        }

        public System.Windows.Media.Brush FrontBrush
        {
            get { return front.Fill; }
            set { front.Fill = value; }
        }
    }  
}
