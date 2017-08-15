using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        //public bool isActive = false;
        public bool IsActive { get; set; } = false;



        public ActionBubble()
        {
            InitializeComponent();
        }

        public ActionBubble(ActionBubble ab)
        {
            InitializeComponent();
            this.Height = ab.Height;
            this.Width = ab.Width;
            this.front = ab.front;
            this.back = ab.back;
            this.clickHandler = ab.clickHandler;
        }


        public void SetActive(bool i_ToActivate)
        {
            this.AllowDrop = IsActive = i_ToActivate;
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
            if (IsActive && clickHandler != null)
            {
                if (onClickAnimationSize != 0)
                {
                    this.Width = initWidth;
                    this.Height = initHeight;
                }
                if (clickHandler != null)
                {
                    clickHandler.Invoke();
                }
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
            if (IsActive && clickHandler != null)
            {
                if (onClickAnimationSize != 0)
                {
                    this.Width = initWidth + onClickAnimationSize;
                    this.Height = initHeight + onClickAnimationSize;
                }
            }
        }

        private void actionBubble_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (IsActive && clickHandler != null)
            {
                if (onClickAnimationSize != 0)
                {
                    this.Width = initWidth;
                    this.Height = initHeight;
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (IsActive == false)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    // Package the data.
                    DataObject data = new DataObject();
                    //data.SetData(DataFormats.StringFormat, circleUI.Fill.ToString());
                    //data.SetData("Double", circleUI.Height);
                    data.SetData("Object", this);

                    // Inititate the drag-and-drop operation.
                    DragDrop.DoDragDrop(this, data, DragDropEffects.Copy | DragDropEffects.Move);
                }
            }
        }

        protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
        {
            base.OnGiveFeedback(e);
            // These Effects values are set in the drop target's
            // DragOver event handler.
            if (e.Effects.HasFlag(DragDropEffects.Copy))
            {
                Mouse.SetCursor(Cursors.Cross);
            }
            else if (e.Effects.HasFlag(DragDropEffects.Move))
            {
                Mouse.SetCursor(Cursors.Pen);
            }
            else
            {
                Mouse.SetCursor(Cursors.No);
            }
            e.Handled = true;
        }


        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            if (e.Data.GetData("Object") is ActionBubble)
            {
                ActionBubble ab = (e.Data.GetData("Object") as ActionBubble);
                this.FrontBrush = ab.FrontBrush;
                this.BackBrush = ab.BackBrush;
                this.clickHandler = ab.clickHandler;
                this.Opacity = ab.Opacity;

                //activat - null...
            }

            e.Handled = true;
        }


    }
}