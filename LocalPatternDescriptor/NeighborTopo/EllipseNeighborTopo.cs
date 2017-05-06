using System.Collections.Generic;
using System.Drawing;
using IM.Lib.Descriptor.LocalPatternDescriptor.WinSize;
using IM.Lib.Utils;
using IM.Lib.Mathematics;

namespace IM.Lib.Descriptor.LocalPatternDescriptor.NeighborTopo
{
    public class EllipseNeighborTopo : ANeighborTopo
    {
        private int _halfWinSize;
        private float[] _weightOfNeighbors;
        private float[] _weightOfCenters;

        public EllipseNeighborTopo(AWinSize winSize)
            : base(winSize)
        {
            this._halfWinSize = this.GetWinSize() / 2;

            this._weightOfNeighbors = new float[this._relativePositions.Count];
            this._weightOfCenters = new float[this._relativePositions.Count];
            this.FindWeightsOfNeighborsAndCenters();
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


        private PointF FindWeightsWithEllipseTopo(Point relativePosition)
        {
            // ellipse equation:    x^2 / a^2 + y^2 / b^2 = 1      (a, b: major and minor axes)
            // line equation:       rho = x*cosTheta + y*sinTheta
            // we do not need to solve the system of 2 equations (ellipse eq. and line eq.)

            int semiMajorAxisLength = _halfWinSize;
            //float semiMinorAxisLength = (float)Math.Ceiling(semiMajorAxisLength / 2);
                       
            //
            PointF weightOfDist = new PointF(-1, -1);

            switch (semiMajorAxisLength)
            {
                case 1: // semiMinorAxisLength = 1      (the case of circle)
                    if(     (relativePosition.X == -1 && relativePosition.Y == -1) || (relativePosition.X == -1 && relativePosition.Y == 1)
                        ||  (relativePosition.X == 1 && relativePosition.Y == -1)  || (relativePosition.X == 1 && relativePosition.Y == 1)      )
                        weightOfDist = new PointF(0.293f, 0.707f); // 0.707 = 1 / sqrt(2)                        
                    else
                        weightOfDist = new PointF(0, 1);
                    break;

                case 2: // semiMinorAxisLength = 1

                    // 45*, 135*, 225*, 315*
                    if(     (relativePosition.X == -2 && relativePosition.Y == -2)  || (relativePosition.X == -2 && relativePosition.Y == 2)
                        ||  (relativePosition.X == 2 && relativePosition.Y == -2)   || (relativePosition.X == 2 && relativePosition.Y == 2)     ) 
                    {
                        float distEllipseToCenter = MathUtility.DistanceToO(new PointF(0.8f, 0.916f)); // this point is on ellipse. It has x=0.8 on x-axis
                        float distCornerToCenter = 2.828f; // sqrt(2^2 + 2^2)
                        float w = distEllipseToCenter / distCornerToCenter;
                        weightOfDist = new PointF(1 - w, w);
                    } 
                    else 
                    {
                        // 22.5*
                        if ( (relativePosition.X == -1 && relativePosition.Y == -2)  || (relativePosition.X == -1 && relativePosition.Y == 2)
                          || (relativePosition.X == 1 && relativePosition.Y == -2)   || (relativePosition.X == 1 && relativePosition.Y == 2)  )
                        {
                            float distEllipseToCenter = MathUtility.DistanceToO(new PointF(0.2f, 0.949f)); // this point is on ellipse. It has x=0.2 on x-axis
                            float distCornerToCenter = 2.236f; // sqrt(2^2 + 1^2)
                            float w = distEllipseToCenter / distCornerToCenter;
                            weightOfDist = new PointF(1 - w, w);
                        } 
                        else 
                        {
                            // 67.5*
                            if ((relativePosition.X == -2 && relativePosition.Y == -1) || (relativePosition.X == -2 && relativePosition.Y == 1)
                             || (relativePosition.X == 2 && relativePosition.Y == -1) || (relativePosition.X == 2 && relativePosition.Y == 1))
                            {
                                float distEllipseToCenter = MathUtility.DistanceToO(new PointF(1.4f, 0.714f)); // this point is on ellipse. It has x=1.4 on x-axis
                                float distCornerToCenter = 2.236f; // sqrt(2^2 + 1^2)
                                float w = distEllipseToCenter / distCornerToCenter;
                                weightOfDist = new PointF(1 - w, w);
                            }
                            else
                            {
                                // 0*, 180*
                                if ((relativePosition.X == 0 && relativePosition.Y == -2) || (relativePosition.X == 0 && relativePosition.Y == 2))
                                {
                                    weightOfDist = new PointF(0.5f, 0.5f);
                                }
                                else
                                {
                                    // 90*, 270*
                                    weightOfDist = new PointF(0, 1);
                                }
                            }
                        }
                    }
                    break;
            }

            return Normalize(weightOfDist.X, weightOfDist.Y);
        }


        private void FindWeightsOfNeighborsAndCenters()
        {
            for (int i = 0; i < this._relativePositions.Count; i++)
            {
                PointF normalizedWeight = FindWeightsWithEllipseTopo(this._relativePositions[i]);
                this._weightOfNeighbors[i] = normalizedWeight.Y;
                this._weightOfCenters[i] = normalizedWeight.X;
            }
        }
    }
}
