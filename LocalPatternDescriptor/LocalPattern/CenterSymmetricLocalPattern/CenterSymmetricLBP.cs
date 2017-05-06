using IM.Lib.Descriptor.LocalPatternDescriptor.NeighborTopo;
using IM.Lib.Descriptor.LocalPatternDescriptor.LocalPatternImage;

// Reference: 
// Heikkila, M., Pietikainen, M.: A Texture-Based Method for Modeling the Background and Detecting Moving Objects. IEEE Trans. PAMI.28(4), 657–662 (2006)

namespace IM.Lib.Descriptor.LocalPatternDescriptor.LocalPattern.CenterSymmetricLocalPattern
{
    public class CenterSymmetricLBP : ACenterSymmetricLocalPattern
    {
        public CenterSymmetricLBP(ANeighborTopo neighborTopo)
            : base(neighborTopo)
        {
            this._lengthOfEncoding = this._neighborTopo.NPointsOnBorder();
        }

        public CenterSymmetricLBP(ANeighborTopo neighborTopo, float thresholdForRobustnessOfFlatArea)
            : base(neighborTopo, thresholdForRobustnessOfFlatArea)
        {
            this._lengthOfEncoding = this._neighborTopo.NPointsOnBorder();
        }


        public override void DoTransform(int x, int y, int encodingIdx, float[] valOfNeighbors, LPImage lpImg)
        {
            // calculate the threshold automatically
            float percent = valOfNeighbors[encodingIdx + this._stepToCompare] / VALUE_FOR_THR;
            if (this._thresholdForRobustnessOfFlatArea != -1)
                percent = this._thresholdForRobustnessOfFlatArea;

            // Run
            if (valOfNeighbors[encodingIdx] >= valOfNeighbors[encodingIdx + this._stepToCompare] + percent)
                lpImg.Set(x, y, encodingIdx, BinaryValueOne.Singleton);
            else
                lpImg.Set(x, y, encodingIdx, BinaryValueZero.Singleton);
        }
    }
}
