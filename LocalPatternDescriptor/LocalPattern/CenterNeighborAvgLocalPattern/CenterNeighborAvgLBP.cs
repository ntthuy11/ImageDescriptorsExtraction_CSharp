using IM.Lib.Descriptor.LocalPatternDescriptor.NeighborTopo;
using IM.Lib.Descriptor.LocalPatternDescriptor.LocalPatternImage;

namespace IM.Lib.Descriptor.LocalPatternDescriptor.LocalPattern.CenterNeighborAvgLocalPattern
{
    public class CenterNeighborAvgLBP : ACenterNeighborAvgLocalPattern
    {
        public CenterNeighborAvgLBP(ANeighborTopo neighborTopo)
            : base(neighborTopo)
        {
            int nPairOfNeighbors            = this._neighborTopo.NPointsOnBorder() / 2;
            int nCenter                     = 1;
            int nPairOfCenterAndNeighbors   = this._neighborTopo.NPointsOnBorder();
            int nPairOfAvgAndNeighbors      = this._neighborTopo.NPointsOnBorder();

            this._lengthOfEncoding           = nPairOfNeighbors + nCenter + nPairOfCenterAndNeighbors + nPairOfAvgAndNeighbors;
        }


        public override void DoTransformNeighbors(int x, int y, int encodingIdx, float[] valOfNeighbors, LPImage lpImg)
        {
            // calculate the threshold automatically
            float percent = valOfNeighbors[encodingIdx + this._stepToCompare] / VALUE_FOR_THR;

            // 1. neighborI vs. neighborJ
            ABinaryValue binVal = this.Compare(valOfNeighbors[encodingIdx], valOfNeighbors[encodingIdx + this._stepToCompare], percent);
            lpImg.Set(x, y, encodingIdx, binVal);

            /*if (valOfNeighbors[encodingIdx] >= valOfNeighbors[encodingIdx + this._stepToCompare] + percent)
                lpImg.Set(x, y, encodingIdx, BinaryValueOne.Singleton);
            else
                lpImg.Set(x, y, encodingIdx, BinaryValueZero.Singleton);*/
        }


        public override void DoTransformAvgAndCenter(int x, int y, int encodingIdxOffset, float centerVal, float avg, LPImage lpImg)
        {
            float percent = avg / VALUE_FOR_THR;

            // 2. center vs. avg
            ABinaryValue binVal = this.Compare(centerVal, avg, percent);
            lpImg.Set(x, y, /* 0 + */ encodingIdxOffset, binVal);

            /*if (centerVal >= avg + percent)
                lpImg.Set(x, y, encodingIdx, BinaryValueOne.Singleton);
            else
                lpImg.Set(x, y, encodingIdx, BinaryValueZero.Singleton);*/
        }


        public override void DoTransformCenterAndNeighbors(int x, int y, int encodingIdx, int encodingIdxOffset, float centerVal, float[] valOfNeighbors, LPImage lpImg)
        {
            // calculate the threshold automatically
            float percent = valOfNeighbors[encodingIdx] / VALUE_FOR_THR;

            // 3. neighborI vs. center
            ABinaryValue binVal = this.Compare(valOfNeighbors[encodingIdx], centerVal, percent);
            lpImg.Set(x, y, encodingIdx + encodingIdxOffset, binVal);

            /*if (valOfNeighbors[encodingIdx] >= centerVal + percent)
                lpImg.Set(x, y, encodingIdx, BinaryValueOne.Singleton);
            else
                lpImg.Set(x, y, encodingIdx, BinaryValueZero.Singleton);*/
        }


        public override void DoTransformAvgAndNeighbors(int x, int y, int encodingIdx, int encodingIdxOffset, float avg, float[] valOfNeighbors, LPImage lpImg)
        {
            // calculate the threshold automatically
            float percent = valOfNeighbors[encodingIdx] / VALUE_FOR_THR;

            // 4. neighborI vs. avg
            ABinaryValue binVal = this.Compare(valOfNeighbors[encodingIdx], avg, percent);
            lpImg.Set(x, y, encodingIdx + encodingIdxOffset, binVal);

            /*if (valOfNeighbors[encodingIdx] >= avg + percent)
                lpImg.Set(x, y, encodingIdx, BinaryValueOne.Singleton);
            else
                lpImg.Set(x, y, encodingIdx, BinaryValueZero.Singleton);*/
        }


        // ---------------------------------------------------------------------


        private ABinaryValue Compare(float val1, float val2, float percent)
        {
            if (val1 >= val2 + percent)
                return BinaryValueOne.Singleton;
            else
                return BinaryValueZero.Singleton;
        }
    }
}
