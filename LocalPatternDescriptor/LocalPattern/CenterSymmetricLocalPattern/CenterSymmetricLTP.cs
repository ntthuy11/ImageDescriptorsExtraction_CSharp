using IM.Lib.Descriptor.LocalPatternDescriptor.NeighborTopo;
using IM.Lib.Descriptor.LocalPatternDescriptor.LocalPatternImage;

namespace IM.Lib.Descriptor.LocalPatternDescriptor.LocalPattern.CenterSymmetricLocalPattern
{
    public class CenterSymmetricLTP : ACenterSymmetricLocalPattern
    {
        private float _thresholdForTenary;
        private int _stepForTenary;


        public CenterSymmetricLTP(ANeighborTopo neighborTopo, float thresholdForTenaryPattern)
            : base(neighborTopo)
        {
            this._lengthOfEncoding      = this._neighborTopo.NPointsOnBorder() * 2;
            this._thresholdForTenary    = thresholdForTenaryPattern;
            this._stepForTenary         = this._neighborTopo.NPointsOnBorder();
        }


        public CenterSymmetricLTP(ANeighborTopo neighborTopo, float thresholdForRobustnessOfFlatArea, float thresholdForTenaryPattern)
            : base(neighborTopo, thresholdForRobustnessOfFlatArea)
        {
            this._lengthOfEncoding      = this._neighborTopo.NPointsOnBorder() * 2;
            this._thresholdForTenary    = thresholdForTenaryPattern;
            this._stepForTenary         = this._neighborTopo.NPointsOnBorder();
        }


        public override void DoTransform(int x, int y, int encodingIdx, float[] valOfNeighbors, LPImage lpImg)
        {
            // calculate the threshold automatically
            float percent = valOfNeighbors[encodingIdx + this._stepToCompare] / VALUE_FOR_THR;
            if (this._thresholdForRobustnessOfFlatArea != -1)
                percent = this._thresholdForRobustnessOfFlatArea;

            // Run
            if (valOfNeighbors[encodingIdx + this._stepToCompare] + percent + this._thresholdForTenary <= valOfNeighbors[encodingIdx])
            {
                lpImg.Set(x, y, encodingIdx, BinaryValueOne.Singleton);
                lpImg.Set(x, y, encodingIdx + this._stepForTenary, BinaryValueZero.Singleton);
            }
            else
            {
                if (valOfNeighbors[encodingIdx] <= valOfNeighbors[encodingIdx + this._stepToCompare] - percent - this._thresholdForTenary)
                {
                    lpImg.Set(x, y, encodingIdx, BinaryValueZero.Singleton);
                    lpImg.Set(x, y, encodingIdx + this._stepForTenary, BinaryValueOne.Singleton);
                }
                else
                {
                    lpImg.Set(x, y, encodingIdx, BinaryValueZero.Singleton);
                    lpImg.Set(x, y, encodingIdx + this._stepForTenary, BinaryValueZero.Singleton);
                }
            }
        }
    }
}
