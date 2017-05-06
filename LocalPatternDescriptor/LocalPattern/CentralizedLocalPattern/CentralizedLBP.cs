using IM.Lib.Descriptor.LocalPatternDescriptor.NeighborTopo;
using IM.Lib.Descriptor.LocalPatternDescriptor.LocalPatternImage;

// Reference:
// X. Fu, W. Wei, "Centralized Binary Patterns Embedded with Image Euclidean Distance for Facial Expression Recognition," Int. Conf. Natural Computation, 2008

// Centralized LP = CenterSymmetric LP + CompareAvgAndCenter

namespace IM.Lib.Descriptor.LocalPatternDescriptor.LocalPattern.CentralizedLocalPattern
{
    public class CentralizedLBP : ACentralizedLocalPattern
    {
        public CentralizedLBP(ANeighborTopo neighborTopo)
            : base(neighborTopo)
        {
            this._lengthOfEncoding = this._neighborTopo.NPointsOnBorder();
        }


        public override void DoTransform(int x, int y, int encodingIdx, float[] valOfNeighbors, LPImage lpImg)
        {
            // calculate the threshold automatically
            float percent = valOfNeighbors[encodingIdx + this._stepToCompare] / VALUE_FOR_THR;

            // Run
            if (valOfNeighbors[encodingIdx] >= valOfNeighbors[encodingIdx + this._stepToCompare] + percent)
                lpImg.Set(x, y, encodingIdx, BinaryValueOne.Singleton);
            else
                lpImg.Set(x, y, encodingIdx, BinaryValueZero.Singleton);
        }


        public override void DoTransformAvgAndCenter(int x, int y, int encodingIdx, float centerVal, float avg, LPImage lpImg)
        {
            float percent = avg / VALUE_FOR_THR;

            // 2. center vs. avg
            if (centerVal >= avg + percent)
                lpImg.Set(x, y, encodingIdx, BinaryValueOne.Singleton);
            else
                lpImg.Set(x, y, encodingIdx, BinaryValueZero.Singleton);
        }
    }
}
