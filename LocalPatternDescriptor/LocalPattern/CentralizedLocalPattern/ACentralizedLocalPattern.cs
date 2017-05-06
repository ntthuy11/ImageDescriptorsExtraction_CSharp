using IM.Imaging;
using IM.Lib.Descriptor.LocalPatternDescriptor.LocalPatternImage;
using IM.Lib.Descriptor.LocalPatternDescriptor.NeighborTopo;
using System.Collections.Generic;
using System.Drawing;

// Centralized LP = CenterSymmetric LP + CompareAvgAndCenter

namespace IM.Lib.Descriptor.LocalPatternDescriptor.LocalPattern.CentralizedLocalPattern
{
    public abstract class ACentralizedLocalPattern : ALocalPattern
    {
        protected const int VALUE_FOR_THR = 10;

        protected int _stepToCompare;


        protected ACentralizedLocalPattern(ANeighborTopo neighborTopo)
            : base(neighborTopo)
        {
            this._stepToCompare = this._neighborTopo.NPointsOnBorder() / 2;
        }


        public abstract void DoTransform(               int x, int y, int encodingIdx,                              float[] valOfNeighbors, LPImage lpImg);
        public abstract void DoTransformAvgAndCenter(   int x, int y, int encodingIdx, float centerVal, float avg,                          LPImage lpImg);

        // ---------------------------------------------------------


        public override void TransformWindow(Image3D srcImg, int srcBand, int x, int y, LPImage lpImg)
        {
            float centerVal = srcImg.Get(x, y, 0, srcBand);
            //float centerVal = srcImg.GetPixelValue(new System.Windows.Point(x, y), srcBand); // for Image3D

            // find values of neighborhoods
            List<RectangleF> neighbors = this._neighborTopo.FindListOfNeighbors(x, y);
            float[] valOfNeighbors = new float[neighbors.Count];            

            for (int i = 0; i < neighbors.Count; i++)
            {
                float weightOfNeighbor = neighbors[i].Width;
                float weightOfCenter = neighbors[i].Height;
                valOfNeighbors[i] = srcImg.Get((int)neighbors[i].X, (int)neighbors[i].Y, 0, srcBand) * weightOfNeighbor + centerVal * weightOfCenter;
                //valOfNeighbors[i] = srcImg.GetPixelValue(new System.Windows.Point((int)neighbors[i].X, (int)neighbors[i].Y), srcBand) * weightOfNeighbor + centerVal * weightOfCenter;
            }

            // ---------- calculate Encode-bits ----------
            //      bit5    bit4    bit3    bit2    bit1

            // calculate the transform _value                                    (bit4    bit3    bit2    bit1)
            int halfNumOfNeighbors = valOfNeighbors.Length / 2;
            for (int i = 0; i < halfNumOfNeighbors; i++)
                this.DoTransform(x, y, i, valOfNeighbors, lpImg);

            // calculate the 'final' transform _value (weight for the center)    (bit5)
            float avg = AverageValue(srcImg, srcBand, neighbors, centerVal);
            this.DoTransformAvgAndCenter(x, y, halfNumOfNeighbors, centerVal, avg, lpImg);
        }


        private float AverageValue(Image3D srcImg, int srcBand, List<RectangleF> neighbors, float centerVal)
        {
            float sum = centerVal;
            foreach (RectangleF neighbor in neighbors)
            {
                float weightOfNeighbor = neighbor.Width;
                float weightOfCenter = neighbor.Height;
                float valOfNeighbor = srcImg.Get((int)neighbor.X, (int)neighbor.Y, 0, srcBand) * weightOfNeighbor + centerVal * weightOfCenter;
                //float valOfNeighbor = srcImg.GetPixelValue(new System.Windows.Point((int)neighbor.X, (int)neighbor.Y), srcBand) * weightOfNeighbor + centerVal * weightOfCenter;
                sum += valOfNeighbor;
            }
            return sum / (neighbors.Count + 1); // consider the centerVal
        }
    }
}
