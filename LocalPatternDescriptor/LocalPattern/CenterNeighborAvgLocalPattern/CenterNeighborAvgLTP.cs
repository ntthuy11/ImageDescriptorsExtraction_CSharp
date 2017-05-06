using IM.Lib.Descriptor.LocalPatternDescriptor.NeighborTopo;
using IM.Lib.Descriptor.LocalPatternDescriptor.LocalPatternImage;

namespace IM.Lib.Descriptor.LocalPatternDescriptor.LocalPattern.CenterNeighborAvgLocalPattern
{
    public class CenterNeighborAvgLTP : ACenterNeighborAvgLocalPattern
    {
        private float _thresholdForTenary;
        private int _stepForTenary;


        public CenterNeighborAvgLTP(ANeighborTopo neighborTopo, float thresholdForTenaryPattern)
            : base(neighborTopo)
        {
            int nPointsOnBorder             = this._neighborTopo.NPointsOnBorder();

            int nPairOfNeighbors            = nPointsOnBorder / 2;
            int nCenter                     = 1;
            int nPairOfCenterAndNeighbors   = nPointsOnBorder;
            int nPairOfAvgAndNeighbors      = nPointsOnBorder;

            this._lengthOfEncoding          = (nPairOfNeighbors + nCenter + nPairOfCenterAndNeighbors + nPairOfAvgAndNeighbors) * 2;
            this._thresholdForTenary        = thresholdForTenaryPattern;
            this._stepForTenary             = (nPairOfNeighbors + nCenter + nPairOfCenterAndNeighbors + nPairOfAvgAndNeighbors);
        }


        public override void DoTransformNeighbors(int x, int y, int encodingIdx, float[] valOfNeighbors, LPImage lpImg)
        {
            // calculate the threshold automatically
            float percent = valOfNeighbors[encodingIdx + this._stepToCompare] / VALUE_FOR_THR;

            // 1. neighborI vs. neighborJ
            if (valOfNeighbors[encodingIdx + this._stepToCompare] + percent + this._thresholdForTenary <= valOfNeighbors[encodingIdx])          // <---
            {
                lpImg.Set(x, y, encodingIdx, BinaryValueOne.Singleton);
                lpImg.Set(x, y, encodingIdx + this._stepForTenary, BinaryValueZero.Singleton);
            }
            else
            {
                if (valOfNeighbors[encodingIdx] <= valOfNeighbors[encodingIdx + this._stepToCompare] - percent - this._thresholdForTenary)      // <---
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


        public override void DoTransformAvgAndCenter(int x, int y, int encodingIdxOffset, float centerVal, float avg, LPImage lpImg)
        {
            float percent = avg / VALUE_FOR_THR;

            // 2. center vs. avg
            if (avg + percent + this._thresholdForTenary <= centerVal)                                                  // <---
            {
                lpImg.Set(x, y, /* 0 + */ encodingIdxOffset, BinaryValueOne.Singleton);
                lpImg.Set(x, y, /* 0 + */ encodingIdxOffset + this._stepForTenary, BinaryValueZero.Singleton);
            }
            else
            {
                if (centerVal <= avg - percent - this._thresholdForTenary)                                              // <---
                {
                    lpImg.Set(x, y, /* 0 + */ encodingIdxOffset, BinaryValueZero.Singleton);
                    lpImg.Set(x, y, /* 0 + */ encodingIdxOffset + this._stepForTenary, BinaryValueOne.Singleton);
                }
                else
                {
                    lpImg.Set(x, y, /* 0 + */ encodingIdxOffset, BinaryValueZero.Singleton);
                    lpImg.Set(x, y, /* 0 + */ encodingIdxOffset + this._stepForTenary, BinaryValueZero.Singleton);
                }
            }
        }


        public override void DoTransformCenterAndNeighbors(int x, int y, int encodingIdx, int encodingIdxOffset, float centerVal, float[] valOfNeighbors, LPImage lpImg)
        {
            // calculate the threshold automatically
            float percent = valOfNeighbors[encodingIdx] / VALUE_FOR_THR;

            // 3. neighborI vs. center
            if (centerVal + percent + this._thresholdForTenary <= valOfNeighbors[encodingIdx])                          // <---
            {
                lpImg.Set(x, y, encodingIdx + encodingIdxOffset, BinaryValueOne.Singleton);
                lpImg.Set(x, y, encodingIdx + encodingIdxOffset + this._stepForTenary, BinaryValueZero.Singleton);
            }
            else
            {
                if (valOfNeighbors[encodingIdx] <= centerVal - percent - this._thresholdForTenary)                      // <---
                {
                    lpImg.Set(x, y, encodingIdx + encodingIdxOffset, BinaryValueZero.Singleton);
                    lpImg.Set(x, y, encodingIdx + encodingIdxOffset + this._stepForTenary, BinaryValueOne.Singleton);
                }
                else
                {
                    lpImg.Set(x, y, encodingIdx + encodingIdxOffset, BinaryValueZero.Singleton);
                    lpImg.Set(x, y, encodingIdx + encodingIdxOffset + this._stepForTenary, BinaryValueZero.Singleton);
                }
            }
        }


        public override void DoTransformAvgAndNeighbors(int x, int y, int encodingIdx, int encodingIdxOffset, float avg, float[] valOfNeighbors, LPImage lpImg)
        {
            // calculate the threshold automatically
            float percent = valOfNeighbors[encodingIdx] / VALUE_FOR_THR;

            // 4. neighborI vs. avg
            if (avg + percent + this._thresholdForTenary <= valOfNeighbors[encodingIdx])                                // <---
            {
                lpImg.Set(x, y, encodingIdx + encodingIdxOffset, BinaryValueOne.Singleton);
                lpImg.Set(x, y, encodingIdx + encodingIdxOffset + this._stepForTenary, BinaryValueZero.Singleton);
            }
            else
            {
                if (valOfNeighbors[encodingIdx] <= avg - percent - this._thresholdForTenary)                            // <---
                {
                    lpImg.Set(x, y, encodingIdx + encodingIdxOffset, BinaryValueZero.Singleton);
                    lpImg.Set(x, y, encodingIdx + encodingIdxOffset + this._stepForTenary, BinaryValueOne.Singleton);
                }
                else
                {
                    lpImg.Set(x, y, encodingIdx + encodingIdxOffset, BinaryValueZero.Singleton);
                    lpImg.Set(x, y, encodingIdx + encodingIdxOffset + this._stepForTenary, BinaryValueZero.Singleton);
                }
            }
        }
    }
}
