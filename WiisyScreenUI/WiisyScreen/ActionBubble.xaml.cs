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
        private int onClickAnimationSize = 0;
        private double initWidth = 0;
        private double initHeight = 0;
        

        public ActionBubble()
        {
            InitializeComponent();
        }

        public Brush FrontBrush
        {
            get { return front.Fill; }
            set { front.Fill = value; }
        }

        public Brush BackBrush
        {
            get { return back.Fill; }
            set { back.Fill = value; }
        }


        private void actionBubble_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (onClickAnimationSize != 0)
            {
                this.Width = initWidth;
                this.Height = initHeight;
            }
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

        internal void InitAnimation(int i_SizeToChange)
        {
            onClickAnimationSize = i_SizeToChange;
            initHeight = this.Height;
            initWidth = this.Width;
        }

        private void actionBubble_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (onClickAnimationSize != 0)
            {
                this.Width = initWidth + onClickAnimationSize;
                this.Height = initHeight + onClickAnimationSize;
            }
        }

        private void actionBubble_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (onClickAnimationSize != 0)
            {
                this.Width = initWidth;
                this.Height = initHeight;
            }
        }

    }  
}
