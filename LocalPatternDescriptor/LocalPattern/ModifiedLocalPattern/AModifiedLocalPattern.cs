using System.Collections.Generic;
using IM.Lib.Descriptor.LocalPatternDescriptor.NeighborTopo;
using System.Drawing;
using IM.Lib.Descriptor.LocalPatternDescriptor.LocalPatternImage;
using IM.Imaging;

namespace IM.Lib.Descriptor.LocalPatternDescriptor.LocalPattern.ModifiedLocalPattern
{
    public abstract class AModifiedLocalPattern : ALocalPattern
    {
        protected AModifiedLocalPattern(ANeighborTopo neighborTopo)
            : base(neighborTopo)
        {
        }

        public abstract void DoTransform(int x, int y, int encodingIdx, float avg, float valOfNeighbor, LPImage lpImg);


        // ---------------------------------------------------------


        public override void TransformWindow(Image3D srcImg, int srcBand, int x, int y, LPImage lpImg)
        {
            float centerVal = srcImg.Get(x, y, 0, srcBand);
            //float centerVal = srcImg.GetPixelValue(new System.Windows.Point(x, y), srcBand); // for Image3D4

            List<RectangleF> neighbors = this._neighborTopo.FindListOfNeighbors(x, y);
            float avg = AverageValue(srcImg, srcBand, neighbors, centerVal);

            for (int i = 0; i < neighbors.Count; i++)
            {
                float weightOfNeighbor = neighbors[i].Width;
                float weightOfCenter = neighbors[i].Height;
                float valOfNeighbor = srcImg.Get((int)neighbors[i].X, (int)neighbors[i].Y, 0, srcBand) * weightOfNeighbor + centerVal * weightOfCenter;
                //float valOfNeighbor = srcImg.GetPixelValue(new System.Windows.Point((int)neighbors[i].X, (int)neighbors[i].Y), srcBand) * weightOfNeighbor + centerVal * weightOfCenter;
                this.DoTransform(x, y, i, avg, valOfNeighbor, lpImg);
            }
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
