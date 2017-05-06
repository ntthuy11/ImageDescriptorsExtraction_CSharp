using System.Collections.Generic;
using IM.Lib.Descriptor.LocalPatternDescriptor.NeighborTopo;
using IM.Lib.Descriptor.LocalPatternDescriptor.LocalPatternImage;
using System.Drawing;
using IM.Imaging;

namespace IM.Lib.Descriptor.LocalPatternDescriptor.LocalPattern.CenterSymmetricLocalPattern
{
    public abstract class ACenterSymmetricLocalPattern : ALocalPattern
    {
        protected const int VALUE_FOR_THR = 10;

        protected float _thresholdForRobustnessOfFlatArea;
        protected int _stepToCompare;


        protected ACenterSymmetricLocalPattern(ANeighborTopo neighborTopo)
            : base(neighborTopo)
        {
            this._thresholdForRobustnessOfFlatArea = -1;
            this._stepToCompare = this._neighborTopo.NPointsOnBorder() / 2;
        }

        protected ACenterSymmetricLocalPattern(ANeighborTopo neighborTopo, float thresholdForRobustnessOfFlatArea)
            : base(neighborTopo)
        {
            this._thresholdForRobustnessOfFlatArea = thresholdForRobustnessOfFlatArea;
            this._stepToCompare = this._neighborTopo.NPointsOnBorder() / 2;
        }


        public abstract void DoTransform(int x, int y, int encodingIdx, float[] valOfNeighbors, LPImage lpImg);


        // ---------------------------------------------------------


        public override void TransformWindow(Image3D srcImg, int srcBand, int x, int y, LPImage lpImg)
        {
            float centerVal = srcImg.Get(x, y, 0, srcBand);
            //float centerVal = srcImg.GetPixelValue(new System.Windows.Point(x, y), srcBand);  // for Image3D

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

            // calculate the transform _value
            for (int i = 0; i < valOfNeighbors.Length / 2; i++)
                this.DoTransform(x, y, i, valOfNeighbors, lpImg);
        }
    }
}
