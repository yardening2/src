﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WiiSyScreen.WiiMoteControlls
{
    public class Smoother
    {
        private const int k_SmoothingBufferSize = 50;
        private int m_SmoothingBufferindex; 
        PointF[] m_SmoothingBuffer;
        public int SmoothingAmount { get; set; }

        public Smoother()
        {
            m_SmoothingBuffer = new PointF[k_SmoothingBufferSize];
            m_SmoothingBufferindex = 0;
        }

        private void addPointToSmoother(PointF i_coordinates)
        {
            m_SmoothingBuffer[m_SmoothingBufferindex % k_SmoothingBufferSize] = i_coordinates;
            m_SmoothingBufferindex++;
        }

        public void Reset()
        {
            m_SmoothingBufferindex = 0;
        }

        public PointF GetSmoothedCursor(PointF i_coordinates)
        {
            addPointToSmoother(i_coordinates);
            int start = m_SmoothingBufferindex - SmoothingAmount - 1;
            start = start < 0 ? 0 : start;
            return calculateCurserAverage(start);
        }

        private PointF calculateCurserAverage(int i_StartingIndex)
        {
            int count = 0;
            PointF smoothed = new PointF(0, 0);
            for (int i = i_StartingIndex; i < m_SmoothingBufferindex; i++)
            {
                smoothed.X += m_SmoothingBuffer[i % k_SmoothingBufferSize].X;
                smoothed.Y += m_SmoothingBuffer[i % k_SmoothingBufferSize].Y;
                count++;
            }
            smoothed.X /= count;
            smoothed.Y /= count;
            return smoothed;
        }
    }
}
