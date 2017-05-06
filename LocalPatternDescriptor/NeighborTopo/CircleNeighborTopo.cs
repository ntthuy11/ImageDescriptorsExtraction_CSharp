using System;
using System.Collections.Generic;
using System.Drawing;
using IM.Lib.Descriptor.LocalPatternDescriptor.WinSize;

namespace IM.Lib.Descriptor.LocalPatternDescriptor.NeighborTopo
{
    public class CircleNeighborTopo : ANeighborTopo
    {
        private int _halfWinSize;
        private float[] _weightOfNeighbors;
        private float[] _weightOfCenters;

        public CircleNeighborTopo(AWinSize winSize)
            : base(winSize)
        {
            this._halfWinSize = this.GetWinSize() / 2;

            this._weightOfNeighbors = new float[this._relativePositions.Count];
            this._weightOfCenters = new float[this._relativePositions.Count];
            this.FindWeightsOfNeighborsAndCenters(this._halfWinSize);
        }


        public override List<RectangleF> FindListOfNeighbors(int x, int y)
        {
            List<RectangleF> result = new List<RectangleF>(); // clockwise adding
            for (int i = 0; i < this._relativePositions.Count; i++)
            {
                Point position = new Point(x + _relativePositions[i].X, y + _relativePositions[i].Y);
                result.Add(new RectangleF(position.X, position.Y, _weightOfNeighbors[i], _weightOfCenters[i]));
            }
            return result;
        }


        // ------------------------------------------------------------------


        private PointF Normalize(float val1, float val2)
        {
            float sum = val1 + val2;
            return new PointF(val1 / sum, val2 / sum);
        }


        private PointF FindWeightsWithCircleTopo(Point relativePosition, float euclideanDistFromCenterToCircleCorner)
        {
            // circle equation: x^2 + y^2 = r^2     (r: _halfWinSize)
            // for simplicity, we do not need to solve the system of two equation (1) circle eq. and (2) line eq.
            float euclideanDistFromCenterToCorner = (float)Math.Sqrt(relativePosition.X * relativePosition.X + relativePosition.Y * relativePosition.Y);                
            float euclideanDistFromCornerToCircleCorner = euclideanDistFromCenterToCorner - euclideanDistFromCenterToCircleCorner;
            PointF weightOfDist = new PointF(euclideanDistFromCornerToCircleCorner / euclideanDistFromCenterToCorner, 
                                             euclideanDistFromCenterToCircleCorner / euclideanDistFromCenterToCorner);

            return Normalize(weightOfDist.X, weightOfDist.Y);
        }


        private void FindWeightsOfNeighborsAndCenters(float euclideanDistFromCenterToCircleCorner)
        {
            for (int i = 0; i < this._relativePositions.Count; i++)
            {
                PointF normalizedWeight = FindWeightsWithCircleTopo(this._relativePositions[i], euclideanDistFromCenterToCircleCorner);
                this._weightOfNeighbors[i] = normalizedWeight.Y;
                this._weightOfCenters[i] = normalizedWeight.X;
            }
        }
    }
}
