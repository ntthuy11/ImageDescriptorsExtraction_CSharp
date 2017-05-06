using IM.Lib.Descriptor.LocalPatternDescriptor.NeighborTopo;
using IM.Lib.Descriptor.LocalPatternDescriptor.LocalPatternImage;

// Centralized LP = CenterSymmetric LP + CompareAvgAndCenter

namespace IM.Lib.Descriptor.LocalPatternDescriptor.LocalPattern.CentralizedLocalPattern
{
    public class CentralizedLTP : ACentralizedLocalPattern
    {
        private float _thresholdForTenary;
        private int _stepForTenary;


        public CentralizedLTP(ANeighborTopo neighborTopo, float thresholdForTenaryPattern)
            : base(neighborTopo)
        {
            this._lengthOfEncoding      = this._neighborTopo.NPointsOnBorder() * 2;
            this._thresholdForTenary    = thresholdForTenaryPattern;
            this._stepForTenary         = this._neighborTopo.NPointsOnBorder();
        }


        public override void DoTransform(int x, int y, int encodingIdx, float[] valOfNeighbors, LPImage lpImg)
        {
            // calculate the threshold automatically
            float percent = valOfNeighbors[encodingIdx + this._stepToCompare] / VALUE_FOR_THR;

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


        public override void DoTransformAvgAndCenter(int x, int y, int encodingIdx, float centerVal, float avg, LPImage lpImg)
        {
            float percent = avg / VALUE_FOR_THR;

            // 2. center vs. avg
            if (avg + percent + this._thresholdForTenary <= centerVal)                                                  // <---
            {
                lpImg.Set(x, y, encodingIdx, BinaryValueOne.Singleton);
                lpImg.Set(x, y, encodingIdx + this._stepForTenary, BinaryValueZero.Singleton);
            }
            else
            {
                if (centerVal <= avg - percent - this._thresholdForTenary)                                              // <---
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
