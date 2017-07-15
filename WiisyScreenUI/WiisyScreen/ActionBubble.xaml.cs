using System;
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
        public int onClickAnimationSize { get; set; } = 7;

        public ActionBubble()
        {
            InitializeComponent();
        }

        public System.Windows.Media.Brush FrontBrush
        {
            get { return front.Fill; }
            set { front.Fill = value; }
        }

        private void actionBubble_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Width = this.Width + onClickAnimationSize;
            this.Height = this.Height + onClickAnimationSize;
        }

        private void actionBubble_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.Width = this.Width - onClickAnimationSize;
            this.Height = this.Height - onClickAnimationSize;
        }
    }  
}
