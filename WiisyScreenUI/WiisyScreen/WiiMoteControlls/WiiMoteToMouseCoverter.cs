using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using WiimoteLib;

namespace WiisyScreen.WiiMoteControlls
{
    public class WiiMoteToMouseCoverter
    {
        public const int INPUT_MOUSE = 0;
        public const int INPUT_KEYBOARD = 1;
        public const int INPUT_HARDWARE = 2;

        private const int MOUSEEVENTF_MOVE = 0x01;
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x20;
        private const int MOUSEEVENTF_MIDDLEUP = 0x40;
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        private const float k_WiiMoteSpectrum = 65535.0f;

        private int ScreenHeight { get; set; }
        private int ScreenWidth { get; set; }

        public int ControlledAreaTopOffset { get; set; }
        public int ControlledAreaBottomOffset { get; set; }

        private Warper m_Warper;
        private Smoother m_Smoother;

        public WiiMoteToMouseCoverter(Warper i_Warper, WiiMoteWrapper i_WiiMoteWrapper)
        {
            m_Warper = i_Warper;
            m_Smoother = new Smoother();
            RegisterEventsListeners(i_WiiMoteWrapper);
            ScreenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            ScreenHeight =  System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            ControlledAreaTopOffset = 0;
            ControlledAreaBottomOffset = 0;
        }

        public void RegisterEventsListeners(WiiMoteWrapper i_WiiMoteWrapper)
        {
            i_WiiMoteWrapper.InfraRedAppearedEvent += onPenAppeared;
            i_WiiMoteWrapper.InfraRedDisppearedEvent += onPenDisappeared;
            i_WiiMoteWrapper.InfraRedMovedEvent += onPenMoved;
        }

        private void onPenAppeared(object i_WiiMote, WiimoteState i_State)
        {
            PointF coordinates = getWarpedCoordinates(i_State);
            m_Smoother.Reset();
            int mouseActionsflags = MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE | MOUSEEVENTF_LEFTDOWN;
            mouse_event(mouseActionsflags, CalculateDeltaX(coordinates.X), CalculateDeltaY(coordinates.Y), 0, 0);
        }

        private int CalculateDeltaX(float RawX)
        {
            return (int)(RawX * k_WiiMoteSpectrum / ScreenWidth);
        }

        private int CalculateDeltaY(float RawY)
        {
            return (int)(RawY * k_WiiMoteSpectrum / ScreenHeight);
        }

        private PointF getWarpedCoordinates(WiimoteState i_State)
        {
            int rawX = i_State.IRState.RawX1;
            int rawY = i_State.IRState.RawY1;
            PointF coordinates = m_Warper.Warp(rawX, rawY);
            coordinates = m_Smoother.GetSmoothedCursor(coordinates);
            return coordinates;
        }

        public void onPenDisappeared(object i_WiiMote, WiimoteState i_State)
        {
            int deltaX = 0;
            int deltaY = 0;
            int mouseActionsflags = MOUSEEVENTF_LEFTUP;
            mouse_event(mouseActionsflags, deltaX, deltaY, 0, 0);
        }

        public void onPenMoved(object i_WiiMote, WiimoteState i_State)
        {
            PointF coordinates = getWarpedCoordinates(i_State);
            int mouseActionsflags = MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE;
            mouse_event(mouseActionsflags, CalculateDeltaX(coordinates.X), CalculateDeltaY(coordinates.Y), 0, 0);
        }

        [DllImport("user32.dll")]
        private static extern void mouse_event(
        long dwFlags, // motion and click options
        long dx, // horizontal position or change
        long dy, // vertical position or change
        long dwData, // wheel movement
        long dwExtraInfo // application-defined information
        );
    }
}
