using IM.Imaging;
using IM.Lib.Descriptor.LocalPatternDescriptor.LocalPatternImage;
using IM.Lib.Descriptor.LocalPatternDescriptor.NeighborTopo;
using System.Collections.Generic;
using System.Drawing;

// CenterNeighborAvg LP = Centralized LP + Basic LP + Modified LP
//                      = (CenterSymmetric LP + CompareAvgAndCenter) + Basic LP + Modified LP

namespace IM.Lib.Descriptor.LocalPatternDescriptor.LocalPattern.CenterNeighborAvgLocalPattern
{
    public abstract class ACenterNeighborAvgLocalPattern : ALocalPattern
    {
        protected const int VALUE_FOR_THR = 10;

        protected int _stepToCompare;


        protected ACenterNeighborAvgLocalPattern(ANeighborTopo neighborTopo)
            : base(neighborTopo)
        {
            this._stepToCompare = this._neighborTopo.NPointsOnBorder() / 2;
        }


        public abstract void DoTransformNeighbors(          int x, int y, int encodingIdx,              /* encodingIdxOffset = 0 */                                 float[] valOfNeighbors,         LPImage lpImg);
        public abstract void DoTransformAvgAndCenter(       int x, int y, /* encodingIdxOffset = 0 */   int encodingIdxOffset,      float centerVal,    float avg,  /* valOfNeighbors notUsed */    LPImage lpImg);
        public abstract void DoTransformCenterAndNeighbors( int x, int y, int encodingIdx,              int encodingIdxOffset,      float centerVal,                float[] valOfNeighbors,         LPImage lpImg);
        public abstract void DoTransformAvgAndNeighbors(    int x, int y, int encodingIdx,              int encodingIdxOffset,                          float avg,  float[] valOfNeighbors,         LPImage lpImg);        


        // ---------------------------------------------------------


        public override void TransformWindow(Image3D srcImg, int srcBand, int x, int y, LPImage lpImg)
        {
            float centerVal = srcImg.Get(x, y, 0, srcBand); // for Image3D
            //float centerVal = srcImg.GetPixelValue(new System.Windows.Point(x, y), srcBand); // for Image3D4

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
            //      b20  b19  b18  b17  b16  b15  b14  b13      b12  b11  b10  b9  b8  b7  b6  b5      b4      b3  b2  b1 b0

            float avg = AverageValue(srcImg, srcBand, neighbors, centerVal);

            // b3  b2  b1 b0
            int halfNumOfNeighbors = valOfNeighbors.Length / 2;
            for (int i = 0; i < halfNumOfNeighbors; i++)
                this.DoTransformNeighbors(x, y, i, valOfNeighbors, lpImg);

            // b4
            this.DoTransformAvgAndCenter(x, y, halfNumOfNeighbors, centerVal, avg, lpImg);

            // b12  b11  b10  b9  b8  b7  b6  b5
            int offset1 = halfNumOfNeighbors + 1;
            for (int i = 0; i < valOfNeighbors.Length; i++)
                this.DoTransformCenterAndNeighbors(x, y, i, offset1, centerVal, valOfNeighbors, lpImg);

            // b20  b19  b18  b17  b16  b15  b14  b13
            int offset2 = valOfNeighbors.Length + offset1;
            for (int i = 0; i < valOfNeighbors.Length; i++)
                this.DoTransformAvgAndNeighbors(x, y, i, offset2, avg, valOfNeighbors, lpImg);
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
