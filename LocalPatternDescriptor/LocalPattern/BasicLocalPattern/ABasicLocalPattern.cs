using System.Collections.Generic;
using IM.Lib.Descriptor.LocalPatternDescriptor.NeighborTopo;
using IM.Lib.Descriptor.LocalPatternDescriptor.LocalPatternImage;
using System.Drawing;
using IM.Imaging;

namespace IM.Lib.Descriptor.LocalPatternDescriptor.LocalPattern.BasicLocalPattern
{
    public abstract class ABasicLocalPattern : ALocalPattern
    {
        protected ABasicLocalPattern(ANeighborTopo neighborTopo)
            : base(neighborTopo)
        {
        }

        public abstract void DoTransform(int x, int y, int encodingIdx, float centerVal, float valOfNeighbor, LPImage lpImg);


        // ---------------------------------------------------------


        public override void TransformWindow(Image3D srcImg, int srcBand, int x, int y, LPImage lpImg)
        {
            float centerVal = srcImg.Get(x, y, 0, srcBand); // for Image3D
            //float centerVal = srcImg.GetPixelValue(new System.Windows.Point(x, y), srcBand); // for Image3D4

            List<RectangleF> neighbors = this._neighborTopo.FindListOfNeighbors(x, y);
            for (int i = 0; i < neighbors.Count; i++)
            {
                float weightOfNeighbor = neighbors[i].Width;
                float weightOfCenter = neighbors[i].Height;
                float valOfNeighbor = srcImg.Get((int)neighbors[i].X, (int)neighbors[i].Y, 0, srcBand) * weightOfNeighbor + centerVal * weightOfCenter; // for Image3D
                //float valOfNeighbor = srcImg.GetPixelValue(new System.Windows.Point((int)neighbors[i].X, (int)neighbors[i].Y), srcBand) * weightOfNeighbor + centerVal * weightOfCenter;
                this.DoTransform(x, y, i, centerVal, valOfNeighbor, lpImg);
            }
        }
    }
}
