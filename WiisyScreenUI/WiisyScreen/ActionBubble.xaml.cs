using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WiisyScreen
{
    /// <summary>
    /// Interaction logic for ActionBubble.xaml
    /// </summary>
    /// 
    public delegate void clickedHandler();

    public partial class ActionBubble : UserControl
    {
        public event clickedHandler clickHandler = null;
        public int onClickAnimationSize { get; set; }

        public ActionBubble()
        {
            InitializeComponent();
            onClickAnimationSize  = 7;
        }

        public Brush FrontBrush
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
            if(clickHandler != null)
            {
                clickHandler.Invoke();
            }
        }

        public void setApp(clickedHandler run, ImageBrush icon)
        {
            this.front.Fill = icon;
            clickHandler = run;
        }
    }  
}
