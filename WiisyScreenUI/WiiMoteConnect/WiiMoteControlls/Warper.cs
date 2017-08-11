using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WiiMoteConnect.WiiMoteControlls
{
    public class Warper
    {
        private float[] m_SourceX = new float[4];
        private float[] m_SourceY = new float[4];
        private float[] m_DestinationX = new float[4];
        private float[] m_DestinationY = new float[4];

        private float[] m_SourcMatrix = new float[16];
        private float[] m_DestioationMatrix = new float[16];
        private float[] m_WarpMatrix = new float[16];
        private bool Isdirty;

        public Warper()
        {
            setIdentity();
        }

        public void setIdentity()
        {
            setSource(  0.0f, 0.0f,
                        1.0f, 0.0f,
                        0.0f, 1.0f,
                        1.0f, 1.0f);
            setDestination(0.0f, 0.0f,
                           1.0f, 0.0f,
                           0.0f, 1.0f,
                           1.0f, 1.0f);
            computeWarp();
        }

        public void setSource(float x0,
                        float y0,
                        float x1,
                        float y1,
                        float x2,
                        float y2,
                        float x3,
                        float y3)
        {
            m_SourceX[0] = x0;
            m_SourceY[0] = y0;
            m_SourceX[1] = x1;
            m_SourceY[1] = y1;
            m_SourceX[2] = x2;
            m_SourceY[2] = y2;
            m_SourceX[3] = x3;
            m_SourceY[3] = y3;
            Isdirty = true;
        }

        public void setDestination(float x0,
                                    float y0,
                                    float x1,
                                    float y1,
                                    float x2,
                                    float y2,
                                    float x3,
                                    float y3)
        {
            m_DestinationX[0] = x0;
            m_DestinationY[0] = y0;
            m_DestinationX[1] = x1;
            m_DestinationY[1] = y1;
            m_DestinationX[2] = x2;
            m_DestinationY[2] = y2;
            m_DestinationX[3] = x3;
            m_DestinationY[3] = y3;
            Isdirty = true;
        }


        public void computeWarp()
        {
            computeQuadToSquare(    m_SourceX[0], m_SourceY[0],
                                    m_SourceX[1], m_SourceY[1],
                                    m_SourceX[2], m_SourceY[2],
                                    m_SourceX[3], m_SourceY[3],
                                    m_SourcMatrix);
            computeSquareToQuad(    m_DestinationX[0], m_DestinationY[0],
                                    m_DestinationX[1], m_DestinationY[1],
                                    m_DestinationX[2], m_DestinationY[2],
                                    m_DestinationX[3], m_DestinationY[3],
                                    m_DestioationMatrix);
            multMats(m_SourcMatrix, m_DestioationMatrix, m_WarpMatrix);
            Isdirty = false;
        }

        public void multMats(float[] srcMat, float[] dstMat, float[] resMat)
        {
            for (int r = 0; r < 4; r++)
            {
                int ri = r * 4;
                for (int c = 0; c < 4; c++)
                {
                    resMat[ri + c] = (srcMat[ri] * dstMat[c] +
                              srcMat[ri + 1] * dstMat[c + 4] +
                              srcMat[ri + 2] * dstMat[c + 8] +
                              srcMat[ri + 3] * dstMat[c + 12]);
                }
            }
        }

        public void computeSquareToQuad(float x0,
                                            float y0,
                                            float x1,
                                            float y1,
                                            float x2,
                                            float y2,
                                            float x3,
                                            float y3,
                                            float[] mat)
        {

            float dx1 = x1 - x2, dy1 = y1 - y2;
            float dx2 = x3 - x2, dy2 = y3 - y2;
            float sx = x0 - x1 + x2 - x3;
            float sy = y0 - y1 + y2 - y3;
            float g = (sx * dy2 - dx2 * sy) / (dx1 * dy2 - dx2 * dy1);
            float h = (dx1 * sy - sx * dy1) / (dx1 * dy2 - dx2 * dy1);
            float a = x1 - x0 + g * x1;
            float b = x3 - x0 + h * x3;
            float c = x0;
            float d = y1 - y0 + g * y1;
            float e = y3 - y0 + h * y3;
            float f = y0;

            mat[0] = a; mat[1] = d; mat[2] = 0; mat[3] = g;
            mat[4] = b; mat[5] = e; mat[6] = 0; mat[7] = h;
            mat[8] = 0; mat[9] = 0; mat[10] = 1; mat[11] = 0;
            mat[12] = c; mat[13] = f; mat[14] = 0; mat[15] = 1;
        }

        public void computeQuadToSquare(float x0,
                                            float y0,
                                            float x1,
                                            float y1,
                                            float x2,
                                            float y2,
                                            float x3,
                                            float y3,
                                            float[] mat)
        {
            computeSquareToQuad(x0, y0, x1, y1, x2, y2, x3, y3, mat);

            // invert through adjoint

            float a = mat[0], d = mat[1],	/* ignore */		g = mat[3];
            float b = mat[4], e = mat[5],	/* 3rd col*/		h = mat[7];
            /* ignore 3rd row */
            float c = mat[12], f = mat[13];

            float A = e - f * h;
            float B = c * h - b;
            float C = b * f - c * e;
            float D = f * g - d;
            float E = a - c * g;
            float F = c * d - a * f;
            float G = d * h - e * g;
            float H = b * g - a * h;
            float I = a * e - b * d;

            // Probably unnecessary since 'I' is also scaled by the determinant,
            //   and 'I' scales the homogeneous coordinate, which, in turn,
            //   scales the X,Y coordinates.
            // Determinant  =   a * (e - f * h) + b * (f * g - d) + c * (d * h - e * g);
            float idet = 1.0f / (a * A + b * D + c * G);

            mat[0] = A * idet; mat[1] = D * idet; mat[2] = 0; mat[3] = G * idet;
            mat[4] = B * idet; mat[5] = E * idet; mat[6] = 0; mat[7] = H * idet;
            mat[8] = 0; mat[9] = 0; mat[10] = 1; mat[11] = 0;
            mat[12] = C * idet; mat[13] = F * idet; mat[14] = 0; mat[15] = I * idet;
        }

        public float[] getWarpMatrix()
        {
            return m_WarpMatrix;
        }

        public PointF Warp(float srcX, float srcY)
        {
            if (Isdirty)
                computeWarp();
            return Warper.Warp(m_WarpMatrix, srcX, srcY);
        }

        public static PointF Warp(float[] mat, float srcX, float srcY)
        {
            float[] result = new float[4];
            float z = 0;
            PointF warpedCoordinate = new PointF();
            result[0] = (float)(srcX * mat[0] + srcY * mat[4] + z * mat[8] + 1 * mat[12]);
            result[1] = (float)(srcX * mat[1] + srcY * mat[5] + z * mat[9] + 1 * mat[13]);
            result[2] = (float)(srcX * mat[2] + srcY * mat[6] + z * mat[10] + 1 * mat[14]);
            result[3] = (float)(srcX * mat[3] + srcY * mat[7] + z * mat[11] + 1 * mat[15]);
            warpedCoordinate.X = result[0] / result[3];
            warpedCoordinate.Y = result[1] / result[3];
            return warpedCoordinate;
        }
    }

}
